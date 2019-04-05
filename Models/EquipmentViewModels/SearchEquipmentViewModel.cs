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

        /// <summary>
        /// 是否是采购设备
        /// </summary>
        [Display(Name = "是否是采购设备")]
        public IsPurchase? IsPurchase { get; set; }


        /// <summary>
        /// 用户选择的生产年份最小值
        /// </summary>
        public int? ProductionDateRangeMin { get; set; }
        /// <summary>
        /// 用户选择的生产年份最大值
        /// </summary>
        public int? ProductionDateRangeMax { get; set; }
        /// <summary>
        /// 插件默认选择的生产年份最小值
        /// </summary>
        public int? ProductionDateRangeDefaultMin { get; set; }
        /// <summary>
        /// 插件默认选择的生产年份最大值
        /// </summary>
        public int? ProductionDateRangeDefaultMax { get; set; }
        /// <summary>
        /// 数据库读取到的生产年份最小值
        /// </summary>
        public int? ProductionDateMin { get; set; }
        /// <summary>
        /// 数据库读取到的生产年份最大值
        /// </summary>
        public int? ProductionDateMax { get; set; }


        /// <summary>
        /// 用户选择的工作小时最小值
        /// </summary>
        public long? WorkingTimeRangeMin { get; set; }
        /// <summary>
        /// 用户选择的工作小时最大值
        /// </summary>
        public long? WorkingTimeRangeMax { get; set; }
        /// <summary>
        /// 插件默认选择的工作小时最小值
        /// </summary>
        public long? WorkingTimeRangeDefaultMin { get; set; }
        /// <summary>
        /// 插件默认选择的工作小时最大值
        /// </summary>
        public long? WorkingTimeRangeDefaultMax { get; set; }
        /// <summary>
        /// 数据库读取到的工作小时最小值
        /// </summary>
        public long? WorkingTimeMin { get; set; }
        /// <summary>
        /// 数据库读取到的工作小时最大值
        /// </summary>
        public long? WorkingTimeMax { get; set; }


        /// <summary>
        /// 用户选择的拍卖日期最小值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtRangeMin { get; set; }
        /// <summary>
        /// 用户选择的拍卖日期最大值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtRangeMax { get; set; }
        /// <summary>
        /// 页面插件默认选择的拍卖日期最小值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtRangeDefaultMin { get; set; }
        /// <summary>
        /// 页面插件默认选择的拍卖日期最大值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtRangeDefaultMax { get; set; }
        /// <summary>
        /// 数据库读取到的拍卖日期最小值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtMin { get; set; }
        /// <summary>
        /// 数据库读取到的拍卖日期最大值
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAtMax { get; set; }


        /// <summary>
        /// 用户输入的成交价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceRangeMin { get; set; }
        /// <summary>
        /// 用户输入的成交价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceRangeMax { get; set; }
        /// <summary>
        /// 插件默认选择的成交价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceRangeDefaultMin { get; set; }
        /// <summary>
        /// 插件默认选择的成交价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceRangeDefaultMax { get; set; }
        /// <summary>
        /// 数据库读取到的成交价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceMin { get; set; }
        /// <summary>
        /// 数据库读取到的成交价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? DealPriceMax { get; set; }


        /// <summary>
        /// 用户选择的预估价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceRangeMin { get; set; }
        /// <summary>
        /// 用户选择的预估价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceRangeMax { get; set; }
        /// <summary>
        /// 插件默认选择的预估价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceRangeDefaultMin { get; set; }
        /// <summary>
        /// 插件默认选择的预估价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceRangeDefaultMax { get; set; }
        /// <summary>
        /// 数据库读取到的预估价格最小值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceMin { get; set; }
        /// <summary>
        /// 数据库读取到的预估价格最大值
        /// </summary>
        [DataType(DataType.Currency)]
        public Decimal? PriceMax { get; set; }

        /// <summary>
        /// 折合人民币
        /// </summary>
        [Display(Name = "折合人民币")]
        [DataType(DataType.Currency)]
        public Decimal DealPriceRMB { get; set; }

        public virtual StaticPagedList<EquipmentViewModel> Equipments { get; set; }
    }
}