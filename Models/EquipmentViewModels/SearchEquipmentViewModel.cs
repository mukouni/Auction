using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities;
using Auction.Models;
using X.PagedList;
using System.ComponentModel.DataAnnotations;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.EquipmentViewModels
{

    public class SearchEquipmentViewModel : RequestPayload
    {

        [Display(Name = "第三方序列号")]
        public string RBCode { get; set; }

        [Display(Name = "序列号")]
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "设备名")]
        public Filter[] Names { get; set; }

        /// <summary>
        /// 生产商 品牌
        /// </summary>
        [Display(Name = "生产商")]
        public Filter[] Manufacturers { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Display(Name = "型号")]
        public Filter[] Models { get; set; }

        /// <summary>
        /// 拍卖行
        /// </summary>
        [Display(Name = "拍卖行")]
        public Filter[] AuctionHouses { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [Display(Name = "国家")]
        public Filter[] Countries { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "城市")]
        public Filter[] Cities { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Display(Name = "是否已删除")]
        public IsDeleted? IsDeleted { get; set; }

        /// <summary>
        /// 是否被拍卖
        /// </summary>
        [Display(Name = "拍卖时间")]
        public IsSold? IsSold { get; set; }


        [Display(Name = "拍卖时间")]
        public DateTime? SoldAt { get; set; }

        /// <summary>
        /// 是否是采购设备
        /// </summary>
        [Display(Name = "是否是采购设备")]
        public IsPurchase? IsPurchase { get; set; }

        /// <summary>
        /// 生产年份范围
        /// </summary>
        [Display(Name = "生产年份范围")]
        public int?[] ProductionDateRange { get; set; }
        public int? ProductionDateMax { get; set; }

        public int? ProductionDateMin { get; set; }

        /// <summary>
        /// 工作小时范围
        /// </summary>
        [Display(Name = "工作小时范围")]
        public long?[] WorkingTimeRange { get; set; }

        /// <summary>
        /// 工作小时最小值
        /// </summary>
        public long? WorkingTimeMin { get; set; }

        /// <summary>
        /// 工作小时最大值
        /// </summary>
        public long? WorkingTimeMax { get; set; }

        /// <summary>
        /// 拍卖日期范围
        /// </summary>
        [Display(Name = "拍卖日期")]
        public int?[] SoldAtRange { get; set; }

        public int? SoldAtMax { get; set; }

        public int? SoldAtMin { get; set; }

        /// <summary>
        /// 成交价格范围
        /// </summary>
        [Display(Name = "成交价格范围")]
        public Decimal?[] DealPriceRange { get; set; }

        /// <summary>
        /// 成交价最小值
        /// </summary>
        public Decimal? DealPriceMin { get; set; }

        /// <summary>
        /// 成交价最大值
        /// </summary>
        public Decimal? DealPriceMax { get; set; }

        /// <summary>
        /// 折合人民币
        /// </summary>
        [Display(Name = "折合人民币")]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal DealPriceRMB { get; set; }

        public virtual StaticPagedList<EquipmentViewModel> Equipments { get; set; }
    }
}