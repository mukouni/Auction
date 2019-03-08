using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Models.ManageViewModels
{
    public class DisplayRecoveryCodesViewModel
    {
        [Required]
        public IEnumerable<string> Codes { get; set; }

    }
}
