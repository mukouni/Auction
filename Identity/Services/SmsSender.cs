using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Auction.Identity.Options;
using System;
using System.Collections.Generic;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Microsoft.Extensions.Logging;

namespace Auction.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message, string type);
    }

    public class SmsSender : ISmsSender
    {
        public SmsSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions _options { get; }  // set only via Secret Manager


        public Task SendSmsAsync(string number, string code, string type)
        {
            if (type == "Register")
            {
                return Execute(number, code, "SMS_162455260");
            }

            if (type == "ResetPassword")
            {
                return Execute(number, code, "SMS_161475362");
            }

            return Task.FromResult(0);
        }

        public Task Execute(string number, string code, string templateCode)
        {

            IClientProfile profile = DefaultProfile.GetProfile("cn-beijing", _options.SMSAppid, _options.SMSsecretKey);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", number);
            request.AddQueryParameters("SignName", "中机国际进出口");
            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("TemplateParam", "{'code':" + code + "}");
            try
            {
                Task.Run(() =>
                {
                    CommonResponse response = client.GetCommonResponse(request);
                    Console.WriteLine(System.Text.Encoding.Default.GetString(response.HttpResponse.Content));
                    // {
                    // 	"Message": "OK",
                    // 	"RequestId": "873043ac-bcda-44db-9052-2e204c6ed20f",
                    // 	"BizId": "607300000000000000^0",
                    // 	"Code": "OK"
                    // }
                    return response.HttpResponse.Content;
                });

            }
            catch (ServerException e)
            {
                Console.WriteLine(e);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e);
            }

            // TwilioClient.Init(accountSid, authToken);

            // return MessageResource.CreateAsync(
            //   to: new PhoneNumber(number),
            //   from: new PhoneNumber(Options.SMSAccountFrom),
            //   body: message);
            return Task.FromResult(0);
        }
    }
}