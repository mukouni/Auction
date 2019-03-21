using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Auction.Entities;
using Auction.Identity.Entities;
using Auction.Models.EquipmentViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using Auction.Data;
using Auction.Entities.Filters;
using Auction.Extensions;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using Microsoft.Net.Http.Headers;
using Auction.Extensions.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Auction.Extensions.Alerts;
using Auction.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Auction.Models;
using Auction.Extensions.AuthContext;

namespace Auction.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class EquipmentController : Controller
    {

        private readonly AuctionDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AuctionSettings _appSettings { get; }

        private static readonly FormOptions _defaultFormOptions = new FormOptions();

        public EquipmentController(AuctionDbContext context,
                    UserManager<ApplicationUser> userManager,
                    SignInManager<ApplicationUser> signInManager,
                    IMapper mapper,
                    ILoggerFactory loggerFactory,
                    IOptions<AuctionSettings> appSettings)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<EquipmentController>();
            _appSettings = appSettings.Value;
        }

        [HttpGet("[action]")]
        [HttpGet("")]
        public IActionResult Index()
        {
            var equipmentVMs = _mapper.Map<IList<EquipmentViewModel>>(_context.Equipments);
            return View(equipmentVMs);
        }

        [HttpGet("[action]")]
        public IActionResult New()
        {

            return View(new EquipmentViewModel());
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(EquipmentViewModel EquipmentVM)
        {

            Equipment equipment = _mapper.Map<Equipment>(EquipmentVM);

            await _context.Equipments.AddAsync(equipment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index)).WithSuccess("创建成功", "");
        }

        [HttpGet("{id:Guid}")]
        [HttpGet("{id:Guid}/[action]")]
        [GenerateAntiforgeryTokenCookieForAjax]
        [CustomAuthorize]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == id);
            var equipmentVM = _mapper.Map<EquipmentViewModel>(equipment);
            return View(equipmentVM);
        }

        // [HttpPost("[action]")]
        // [DisableFormValueModelBinding]
        // // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> UploadPhoto()
        // {
        //     if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        //     {
        //         return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
        //     }

        //     // Used to accumulate all the form url encoded key value pairs in the 
        //     // request.
        //     var formAccumulator = new KeyValueAccumulator();
        //     string targetFilePath = null;

        //     var boundary = MultipartRequestHelper.GetBoundary(
        //         MediaTypeHeaderValue.Parse(Request.ContentType),
        //         _defaultFormOptions.MultipartBoundaryLengthLimit);
        //     var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        //     var section = await reader.ReadNextSectionAsync();
        //     while (section != null)
        //     {
        //         ContentDispositionHeaderValue contentDisposition;
        //         var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

        //         if (hasContentDispositionHeader)
        //         {
        //             if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        //             {
        //                 string photoDir = _appSettings.FilesRootDir;
        //                 string fileExt = Path.GetExtension(HttpContext.Request.Form.Files[0].FileName);
        //                 string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt;
        //                 string uri = Path.Combine("equipment", newFileName);
        //                 string pathString = Path.Combine(photoDir, uri);
        //                 Directory.CreateDirectory(pathString);
        //                 if (!File.Exists(pathString))
        //                 {
        //                     using (var targetStream = File.Create(pathString))
        //                     {
        //                         await section.Body.CopyToAsync(targetStream);
        //                         _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
        //                     }
        //                 }

        //                 targetFilePath = Path.GetTempFileName();
        //                 using (var targetStream = File.Create(targetFilePath))
        //                 {
        //                     await section.Body.CopyToAsync(targetStream);

        //                     _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
        //                 }

        //             }
        //             else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
        //             {
        //                 // Content-Disposition: form-data; name="key"
        //                 //
        //                 // value

        //                 // Do not limit the key name length here because the 
        //                 // multipart headers length limit is already in effect.
        //                 var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
        //                 var encoding = GetEncoding(section);
        //                 using (var streamReader = new StreamReader(
        //                     section.Body,
        //                     encoding,
        //                     detectEncodingFromByteOrderMarks: true,
        //                     bufferSize: 1024,
        //                     leaveOpen: true))
        //                 {
        //                     // The value length limit is enforced by MultipartBodyLengthLimit
        //                     var value = await streamReader.ReadToEndAsync();
        //                     if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
        //                     {
        //                         value = string.Empty;
        //                     }
        //                     formAccumulator.Append(key, value);

        //                     if (formAccumulator.ValueCount > _defaultFormOptions.ValueCountLimit)
        //                     {
        //                         throw new InvalidDataException($"Form key count limit {_defaultFormOptions.ValueCountLimit} exceeded.");
        //                     }
        //                 }
        //             }
        //         }

        //         // Drains any remaining section body that has not been consumed and
        //         // reads the headers for the next section.
        //         section = await reader.ReadNextSectionAsync();
        //     }

        //     var photo = new Photo();
        //     photo.RequestPath = targetFilePath;

        //     // List<Photo> eqphotos = new List<Photo>();
        //     // photos.Add(eqphotos);

        //     // await _context.AddRangeAsync(photos);

        //     var formValueProvider = new FormValueProvider(
        //             BindingSource.Form,
        //             new FormCollection(formAccumulator.GetResults()),
        //             CultureInfo.CurrentCulture);

        //     var equipmentViewModel = new CreateEquipmentViewModel()
        //     {
        //         // FilePath = targetFilePath
        //     };
        //     return Json(equipmentViewModel);
        // }


        [Authorize(Roles="Admin, Staff")]
        [HttpPost("[action]")]
        public async Task<IActionResult> UploadPhotoAsync(EquipmentPhotoViewModel equipmentPhoto)
        {
            var response = ResponseModelFactory.CreateInstance;
            var photo = equipmentPhoto.photo;
            if (photo == null)
            {
                response.SetNotFound("没有可上传的文件");
                return Ok(response);
            }

            var equipment = await _context.Equipments.FirstOrDefaultAsync(e => e.Id == equipmentPhoto.EquipmentId);
            if (equipment == null)
            {
                response.SetNotFound("没有找到设备");
                return Ok(response);
            }

            string FilesRootDir = _appSettings.FilesRootDir;
            string pathString = Path.Combine(FilesRootDir, "images", typeof(Equipment).Name);
            if (!Directory.Exists(pathString))
            {
                try
                {
                    Directory.CreateDirectory(pathString);
                }
                catch (System.IO.IOException e)
                {
                    response.SetFailed("创建目录失败");
                    _logger.LogError(e.Message);
                    return Ok(response);
                }
            }

            string fileExt = Path.GetExtension(equipmentPhoto.photo?.FileName);
            string newFileName = System.Guid.NewGuid().ToString() + fileExt;

            string filePathString = Path.Combine(pathString, newFileName);

            var newPhoto = _mapper.Map<Photo>(equipmentPhoto);
            using (var stream = System.IO.File.Create(filePathString))
            {
                await photo.CopyToAsync(stream);
                newPhoto = newPhoto.MapFrom(photo,
                                            newFileName,
                                            RequestPath: "/images/" + typeof(Equipment).Name + "/" + newFileName,
                                            SavePath: "\\images\\" + typeof(Equipment).Name + "\\" + newFileName);
                equipment.Photos.Add(newPhoto);
                await _context.SaveChangesAsync();
            }

            var pho = _mapper.Map<PhotoViewModel>(await _context.Photos.FirstOrDefaultAsync(p => p.FileName == newPhoto.FileName));
            response.SetData(Json(pho).Value);
            return Ok(response);
        }

        /// <summary>
        /// <param name="photoName">包含图片名字和后缀</param>
        /// </summary>
        [HttpPost("[action]")]
        public async Task<IActionResult> DeletePhoto(string photoName, Guid key)
        {
            var response = ResponseModelFactory.CreateInstance;

            var photo = _context.Photos.FirstOrDefault(p => p.FileName == photoName);
            if (photo == null)
            {
                response.SetNotFound("没有找到图片");
                return Ok(response);
            }

            string sourceFile = System.IO.Path.Combine(_appSettings.FilesRootDir + photo.SavePath);
            if (System.IO.File.Exists(sourceFile))
            {
                try
                {
                    System.IO.File.Delete(sourceFile);
                }
                catch (System.IO.IOException e)
                {
                    response.SetFailed("删除图片失败");
                    _logger.LogError(e.Message);
                    return Ok(response);
                }

                _context.Photos.Remove(photo);
                await _context.SaveChangesAsync();
                return Ok(response);
            }
            response.SetNotFound("删除时磁盘上没有找到图片");
            return Ok(response);
        }

        /// <summary>
        /// <param name="photoName">包含图片名字和后缀</param>
        /// </summary>
        [HttpPost("[action]")]
        public async Task<IActionResult> SetCoverPhoto(string photoName, Guid equipmentId)
        {
            var response = ResponseModelFactory.CreateInstance;
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.FileName == photoName && p.IsCover != true);
            if (photo == null)
            {
                response.SetNotFound("没有找到图片");
                return Ok(response);
            }

            var originCoverPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.EquipmentId == equipmentId && p.IsCover == true);
            if (originCoverPhoto != null)
            {
                originCoverPhoto.IsCover = false;
                _context.Entry(originCoverPhoto).State = EntityState.Modified;
            }

            photo.IsCover = true;
            _context.Entry(photo).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(response);
        }
        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}