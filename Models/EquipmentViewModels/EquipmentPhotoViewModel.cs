using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Auction.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.EquipmentViewModels
{
    public class EquipmentPhotoViewModel : PhotoViewModel
    {

        // 文件排序
        public int file_id { get; set; }
        public IFormFile photo { get; set; }
    }
}