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

        [Display(Name = "外部序列号")]
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Display(Name = "设备名")]
        public Filter[] Names { get; set; }

        /// <summary>
        /// 生产商
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
        public IsDeleted IsDeleted { get; set; }


        /// <summary>
        /// 是否被拍卖
        /// </summary>
        [Display(Name = "拍卖时间")]
        public IsSold IsSold { get; set; }


        [Display(Name = "拍卖时间")]
        public DateTime SoldAt { get; set; }

        /// <summary>
        /// 是否是采购设备
        /// </summary>
        [Display(Name = "是否是采购设备")]
        public IsPurchase IsPurchase { get; set; }

        /// <summary>
        /// 生产年份范围
        /// </summary>
        [Display(Name = "生产年份范围")]
        public DateTime[] ProductionDateRange { get; set; }


        /// <summary>
        /// 工作小时范围
        /// </summary>
        [Display(Name = "工作小时范围")]
        public long[] WorkingTimeRange { get; set; }


        private long? _workingTimeMin;
        /// <summary>
        /// 工作小时最小值
        /// </summary>
        public long? WorkingTimeMin
        {
            get { return _workingTimeMin; }
            set
            {
                if (value == null)
                {
                    _workingTimeMin = 0;
                }
                else
                {
                    _workingTimeMin = value;
                }
            }
        }


        private long? _workingTimeMax;
        /// <summary>
        /// 工作小时最大值
        /// </summary>
        public long? WorkingTimeMax
        {
            get { return _workingTimeMax; }
            set
            {
                if (value == null)
                {
                    _workingTimeMax = 0;
                }
                else
                {
                    _workingTimeMax = value;
                }
            }
        }

        /// <summary>
        /// 成交价格范围
        /// </summary>
        [Display(Name = "成交价格范围")]
        public Decimal[] DealPriceRange { get; set; }


        private Decimal? _dealPriceMin;
        /// <summary>
        /// 成交价最小值
        /// </summary>
        public Decimal? DealPriceMin
        {
            get { return _dealPriceMin; }
            set
            {
                if (value == null)
                {
                    _dealPriceMin = 0;
                }
                else
                {
                    _dealPriceMin = value;
                }
            }
        }

        private Decimal? _dealPriceMax;
        /// <summary>
        /// 成交价最大值
        /// </summary>
        public Decimal? DealPriceMax
        {
            get { return _dealPriceMax; }
            set
            {
                if (value == null)
                {
                    _dealPriceMax = 0;
                }
                else
                {
                    _dealPriceMax = value;
                }
            }
        }

        /// <summary>
        /// 折合人民币
        /// </summary>
        [Display(Name = "折合人民币")]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal DealPriceRMB { get; set; }

        public virtual StaticPagedList<EquipmentViewModel> Equipments { get; set; }
    }

    public class Filter
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public int SelectedCount { get; set; }

        public bool Selected { get; set; }

        public int SortNumber { get; set; }
    }
}