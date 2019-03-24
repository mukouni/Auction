using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities;
using Auction.Models;
using X.PagedList;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.EquipmentViewModels
{

    public class SearchEquipmentViewModel : RequestPayload
    {
        

        public ICollection<string> Codes { get; set; } = new List<string>();

        /// <summary>
        /// 设备名称
        /// </summary>
        public ICollection<string> Names { get; set; } = new List<string>();

        /// <summary>
        /// 生产商
        /// </summary>
        public ICollection<string> Manufacturers { get; set; } = new List<string>();

        /// <summary>
        /// 型号
        /// </summary>
        public ICollection<string> Models { get; set; } = new List<string>();

        /// <summary>
        /// 拍卖行
        /// </summary>
        public ICollection<string> AuctionHouses { get; set; } = new List<string>();

        /// <summary>
        /// 国家
        /// </summary>
        public ICollection<string> Countries { get; set; } = new List<string>();

        /// <summary>
        /// 城市
        /// </summary>
        public ICollection<string> cities { get; set; } = new List<string>();

        ///<summary>
        /// 是否删除
        /// </summary>
        public IsDeleted? IsDeleted { get; set; }


        ///<summary>
        /// 是否被拍卖
        /// </summary>
        public IsSold? IsSold { get; set; }

        ///<summary>
        /// 是否是采购设备
        /// </summary>
        public IsPurchase? IsPurchase { get; set; }

        ///<summary>
        /// 生产年份范围
        /// </summary>
        public DateTime?[] ProductionDateRang { get; set; } = new DateTime?[] { };


        ///<summary>
        /// 工作小时范围
        /// </summary>
        public long?[] WorkingTimeRang { get; set; } = new long?[] { };

        ///<summary>
        /// 成交价格范围
        /// </summary>
        public Decimal?[] DealPriceRang { get; set; } = new Decimal?[] { };

        ///<summary>
        /// 折合人民币
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal? DealPriceRMB { get; set; }

        public virtual StaticPagedList<EquipmentViewModel> Equipments { get; set; }
    }
}