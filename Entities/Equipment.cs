using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities.Enums;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    [Table("ac_equipment")]
    public class Equipment : BaseEntity
    {
        [Column(TypeName = "nvarchar(50)")]
        public string Code { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Model { get; set; }

        /// <summary>
        /// 拍卖行
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string AuctionHouse { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Country { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }

        ///<summary>
        /// 是否被拍卖
        /// </summary>
        public IsSold? IsSold { get; set; } = CommonEnum.IsSold.No;

        ///<summary>
        /// 拍卖时间，用于区分是否被拍卖
        /// </summary>
        public DateTime SoldAt { get; set; }

        ///<summary>
        /// 是否是采购设备
        /// </summary>
        public IsPurchase? IsPurchase { get; set; } = CommonEnum.IsPurchase.No;

        ///<summary>
        /// 生产年份
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        ///<summary>
        /// 工作小时
        /// </summary>
        public long? WorkingTime { get; set; }

        ///<summary>
        /// 成交价格
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal? DealPrice { get; set; }

        ///<summary>
        /// 折合人民币
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal? DealPriceRMB { get; set; }

        ///<summary>
        /// 长(毫米)
        /// </summary>
        public long? Long { get; set; }

        ///<summary>
        /// 宽(毫米)
        /// </summary>
        public long? Width { get; set; }

        ///<summary>
        /// 高(毫米)
        /// </summary>
        public long? Height { get; set; }

        ///<summary>
        /// 重量(kg) 
        /// </summary>
        public double? Weight { get; set; }

        ///<summary>
        /// 体积(立方米) 
        /// </summary>
        [Column(TypeName = "decimal(18, 3)")]
        public Decimal? Volume { get; set; }

        // 导航属性
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();
    }
}