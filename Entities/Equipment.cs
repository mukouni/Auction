using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities.Enums;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    [Table("ac_equipment")]
    public class Equipment : BaseEntity
    {
        /// <summary>
        /// 利氏兄弟序列号
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string RBCode { get; set; }

        /// <summary>
        /// 拍卖号
        /// </summary>

        public string LotNo { get; set; }

        /// <summary>
        /// 引擎号
        /// </summary>
        public string EngineNo { get; set; }

        /// <summary>
        /// 架子号
        /// </summary>
        public string FrameNo { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
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

        /// <summary>
        /// 是否被拍卖
        /// </summary>
        public IsSold? IsSold { get; set; }

        /// <summary>
        /// 拍卖时间，用于区分是否被拍卖
        /// </summary>
        public DateTime? SoldAt { get; set; }

        /// <summary>
        /// 是否是采购设备
        /// </summary>
        public IsPurchase? IsPurchase { get; set; }

        /// <summary>
        /// 生产年份
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// 工作小时
        /// </summary>
        public long? WorkingTime { get; set; }

        /// <summary>
        /// 工作里程数
        /// </summary>
        public long? WorkingDistance { get; set; }

        /// <summary>
        /// 工作里程数单位
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        public string WorkingDistanceUnit { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal? DealPrice { get; set; }

        /// <summary>
        /// 成交价格币种
        /// </summary>
        public virtual Currency DealPriceCurrency { get; set; }
        /// <summary>
        /// 成交价格币种Id
        /// </summary>
        public int? DealPriceCurrencyId { get; set; }

        /// <summary>
        /// 成交价格
        /// </summary>
        public Decimal? Price { get; set; }

        /// <summary>
        /// 价格币种
        /// </summary>
        public virtual Currency PriceCurrency { get; set; }

        /// <summary>
        /// 价格币种Id
        /// </summary>
        public int? PriceCurrencyId { get; set; }

        /// <summary>
        /// 折合人民币
        /// </summary>
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal? DealPriceRMB { get; set; }

        /// <summary>
        /// 长(毫米)
        /// </summary>
        public long? Long { get; set; }

        /// <summary>
        /// 宽(毫米)
        /// </summary>
        public long? Width { get; set; }

        /// <summary>
        /// 高(毫米)
        /// </summary>
        public long? Height { get; set; }

        /// <summary>
        /// 重量(kg) 
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// 体积(立方米) 
        /// </summary>
        [Column(TypeName = "decimal(18, 3)")]
        public Decimal? Volume { get; set; }

        /// <summary>
        /// 备注
        /// </summary>

        [MaxLength(5000)]
        public string Remark { get; set; }

        // 导航属性
        public virtual ICollection<Photo> Photos { get; set; } = new List<Photo>();

        /// <summary>
        /// 封皮照片 
        /// </summary>
        public virtual Photo CoverPhoto { get; set; }

        /// <summary>
        /// 外观照片 
        /// </summary>
        // [InverseProperty(nameof(Photo.Exterior))]
        public virtual ICollection<Photo> ExteriorPhotos { get; set; } = new List<Photo>();

        /// <summary>
        /// 履带底架照片 
        /// </summary>
        // [InverseProperty(nameof(Photo.TrackedChassis))]
        public virtual ICollection<Photo> TrackedChassisPhotos { get; set; } = new List<Photo>();

        /// <summary>
        /// 驾驶室照片
        /// </summary>
        // [InverseProperty(nameof(Photo.Cab))]
        public virtual ICollection<Photo> CabPhotos { get; set; } = new List<Photo>();

        /// <summary>
        /// 臂架照片 
        /// </summary>
        // [InverseProperty(nameof(Photo.Boom))]
        public virtual ICollection<Photo> BoomPhotos { get; set; } = new List<Photo>();

        /// <summary>
        /// 引擎照片
        /// </summary>
        // [InverseProperty(nameof(Photo.Engine))]
        public virtual ICollection<Photo> EnginePhotos { get; set; } = new List<Photo>();
    }
}