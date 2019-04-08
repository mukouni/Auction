using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities;
using Auction.Models;
using X.PagedList;
using System.ComponentModel.DataAnnotations;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.AccountViewModels
{

    public class SearchApplicationUserViewModel : RequestPayload
    {
        public virtual StaticPagedList<ApplicationUserViewModel> ApplicationUsers { get; set; }
    }
}