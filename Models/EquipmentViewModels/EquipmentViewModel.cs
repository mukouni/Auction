using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auction.Entities;
using Auction.Entities.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models.EquipmentViewModels
{
    public class EquipmentViewModel
    {
        private EquipmentPhotoViewModel _coverPhoto;

        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();


        /// <summary>
        /// 序列号
        /// </summary>
        [Display(Name = "序列号")]
        public string Code { get; set; }


        /// <summary>
        /// 第三方序列号
        /// </summary>
        [Display(Name = "第三方序列号")]
        public string RBCode { get; set; }

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
        [Display(Name = "已被拍卖")]
        public IsSold? IsSold { get; set; }

        ///<summary>
        /// 是否是采购设备
        /// </summary>
        [Display(Name = "是采购设备")]
        public IsPurchase? IsPurchase { get; set; }

        ///<summary>
        /// 是否被删除
        /// </summary>
        [Display(Name = "已删除")]
        public IsDeleted? IsDeleted { get; set; }

        ///<summary>
        /// 生产年份
        /// </summary>
        [Required]
        [Display(Name = "生产年份")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ProductionDate { get; set; }

        ///<summary>
        /// 拍卖日期
        /// </summary>
        [Display(Name = "拍卖日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SoldAt { get; set; }

        ///<summary>
        /// 工作小时
        /// </summary>
        [Display(Name = "工作小时")]
        public long WorkingTime { get; set; }


        ///<summary>
        /// 工作里程
        /// </summary>
        [Display(Name = "工作里程")]
        public long? WorkingDistance { get; set; }

        ///<summary>
        /// 工作里程
        /// </summary>
        [Display(Name = "里程单位")]
        public string WorkingDistanceUnit { get; set; } = "km";

        ///<summary>
        /// 成交价格
        /// </summary>
        [Display(Name = "成交价格")]
        [DataType(DataType.Currency)]
        public Decimal? DealPrice { get; set; }

        ///<summary>
        /// 成交价格币种
        /// </summary>
        [Display(Name = "成交货币币种")]
        public int? DealPriceCurrencyId { get; set; }

        ///<summary>
        /// 成交价格币种
        /// </summary>
        [Display(Name = "成交货币币种")]
        public Currency DealPriceCurrency { get; set; }

        /// <summary>
        /// lotNo
        /// </summary>
        [Display(Name = "LotNo")]

        public string LotNo { get; set; }

        /// <summary>
        /// 引擎号
        /// </summary>
        [Display(Name = "引擎号")]
        public string EngineNo { get; set; }

        /// <summary>
        /// 架子号
        /// </summary>
        [Display(Name = "架子号")]
        public string FrameNo { get; set; }

        ///<summary>
        /// 预估价格
        /// </summary>
        [Display(Name = "预估价格")]
        [DataType(DataType.Currency)]
        public Decimal? Price { get; set; }

        ///<summary>
        /// 成交价格
        /// </summary>
        [Display(Name = "成交价格币种")]
        public int? PriceCurrencyId { get; set; }

        ///<summary>
        /// 成交价格
        /// </summary>
        [Display(Name = "成交价格币种")]
        public Currency PriceCurrency { get; set; }

        ///<summary>
        /// 折合人民币
        /// </summary>
        [Display(Name = "折合人民币")]
        [DataType(DataType.Currency)]
        public Decimal? DealPriceRMB { get; set; }

        ///<summary>
        /// 长(毫米)
        /// </summary>
        [Display(Name = "长(毫米)")]
        public long? Long { get; set; }

        ///<summary>
        /// 宽(毫米)
        /// </summary>
        [Display(Name = "宽(毫米)")]
        public long? Width { get; set; }

        ///<summary>
        /// 高(毫米)
        /// </summary>
        [Display(Name = "高(毫米)")]
        public long? Height { get; set; }

        ///<summary>
        /// 重量(kg) 
        /// </summary>
        [Display(Name = "重量(kg) ")]
        public double? Weight { get; set; }

        ///<summary>
        /// 体积(立方米)
        /// </summary>
        [Display(Name = "体积(立方米)")]
        public Decimal? Volume { get; set; }


        ///<summary>
        /// 备注
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Display(Name = "备注")]
        public string Remark { get; set; }

        public ICollection<EquipmentPhotoViewModel> Photos { get; set; } = new List<EquipmentPhotoViewModel>();

        [Display(Name = "封面图片")]
        public EquipmentPhotoViewModel CoverPhoto //{get; set;} // 修改页面里面如果很有可能会将这个新的临时对象一并保存，但是Photo中的EquipmentId是{0000-...}造成保存失败！！
        {
            set
            {
                this._coverPhoto = value;
            }
            get
            {

                if (this._coverPhoto == null)
                {
                    this._coverPhoto = new EquipmentPhotoViewModel()
                    {
                        SavePath = "\\images\\Equipment\\default.jpg",
                        RequestPath = "/images/Equipment/default.jpg",
                        FileName = "default.jpg"
                    };
                }
                return _coverPhoto;
            }
        }


        [Display(Name = "外观照片")]
        public ICollection<EquipmentPhotoViewModel> ExteriorPhotos { get; set; } = new List<EquipmentPhotoViewModel>();

        [Display(Name = "履带底架照片")]
        public ICollection<EquipmentPhotoViewModel> TrackedChassisPhotos { get; set; } = new List<EquipmentPhotoViewModel>();
        [Display(Name = "臂架照片")]
        public ICollection<EquipmentPhotoViewModel> BoomPhotos { get; set; } = new List<EquipmentPhotoViewModel>();


        [Display(Name = "驾驶室照片")]
        public ICollection<EquipmentPhotoViewModel> CabPhotos { get; set; } = new List<EquipmentPhotoViewModel>();

        [Display(Name = "引擎照片")]
        public ICollection<EquipmentPhotoViewModel> EnginePhotos { get; set; } = new List<EquipmentPhotoViewModel>();

        public ICollection<SelectListItem> Currencies { get; set; } = new List<SelectListItem>();

    }
}
