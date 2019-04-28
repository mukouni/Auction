using System.ComponentModel.DataAnnotations;

namespace Auction.Models.EmailViewModels
{
    public class ContactUsEmailViewModel
    {
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Message { get; set; }
    }
}