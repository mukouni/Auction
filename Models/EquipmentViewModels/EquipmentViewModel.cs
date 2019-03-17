using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auction.Entities;
using Microsoft.Extensions.Options;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.EquipmentViewModels
{
    public class EquipmentViewModel
    {
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "设备名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        [Display(Name = "生产商")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Display(Name = "型号")]
        [Required]
        public string Model { get; set; }

        /// <summary>
        /// 拍卖行
        /// </summary>
        [Display(Name = "拍卖行")]
        public string AuctionHouse { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家")]
        public string Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        public string City { get; set; }

        ///<summary>
        /// 是否被拍卖
        /// </summary>
        [Display(Name = "是否被拍卖")]
        public IsSold? IsSold { get; set; }

        ///<summary>
        /// 是否是采购设备
        /// </summary>
        [Display(Name = "是否是采购设备")]

        public string IsPurchase { get; set; }

        ///<summary>
        /// 生产年份
        /// </summary>
        [Display(Name = "生产年份")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ProductionDate { get; set; }

        ///<summary>
        /// 工作小时
        /// </summary>
        [Display(Name = "工作小时")]
        [Required]
        public int? WorkingTime { get; set; }

        ///<summary>
        /// 成交价格
        /// </summary>
        [Display(Name = "成交价格")]
        [DataType(DataType.Currency)]
        public Decimal? DealPrice { get; set; }

        ///<summary>
        /// 折合人民币
        /// </summary>
        [Display(Name = "折合人民币")]
        public Decimal? DealPriceRMB { get; set; }

        ///<summary>
        /// 长(毫米)
        /// </summary>
        [Display(Name = "长(毫米)")]
        public int? Long { get; set; }

        ///<summary>
        /// 宽(毫米)
        /// </summary>
        [Display(Name = "宽(毫米)")]
        public int? Width { get; set; }

        ///<summary>
        /// 高(毫米)
        /// </summary>
        [Display(Name = "高(毫米)")]
        public int? Height { get; set; }

        ///<summary>
        /// 重量(kg) 
        /// </summary>
        [Display(Name = "重量(kg) ")]
        public double? Weight { get; set; }

        ///<summary>
        /// 体积(立方米)
        /// </summary>
        [Display(Name = "体积(立方米) ")]
        public Decimal? Volume { get; set; }

        
        public Photo CoverPhoto { get; set; }

        public ICollection<EquipmentPhotoViewModel> Photos { get; set; } = new List<EquipmentPhotoViewModel>();
    }
}
