using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Models
{
    public class InquiryViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string Tel { get; set; }
        /// <summary>
        /// email
        /// </summary>
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 启运港
        /// </summary>
        [Display(Name = "启运港")]
        public string StartDeparture { get; set; }

        /// <summary>
        /// 目的港
        /// </summary>
        [Display(Name = "目的港")]
        public string EndDeparture { get; set; }

        /// <summary>
        /// 特种箱
        /// </summary>
        [Display(Name = "特种箱列表")]
        public ICollection<Cabinet> Cabinets { get; set; } = new List<Cabinet>();

        [Display(Name = "Gp20")]
        public string Gp20 { get; set; }
        [Display(Name = "Gp40")]
        public string Gp40 { get; set; }
        [Display(Name = "Hc40")]
        public string Hc40 { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        [Display(Name = "到厂拖车")]
        public bool MotorcycleFactory { get; set; } = false;

        /// <summary>
        /// 仓库装柜
        /// </summary>
        [Display(Name = "仓库装柜")]
        public bool WarehouseLoading { get; set; } = false;
        /// <summary>
        /// 上门绑扎
        /// </summary>
        [Display(Name = "上门绑扎")]
        public bool HomeLashing { get; set; } = false;
        /// <summary>
        /// 代理报关
        /// </summary>
        [Display(Name = "代理报关")]
        public bool CustomsDeclaration { get; set; } = false;

        /// <summary>
        /// 出运时间
        /// </summary>
        [Display(Name = "出运时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShipmentTime { get; set; }
    }

    public class Cabinet
    {
        public string CabinetType { get; set; }
        public int CabinetCount { get; set; }
        public int CabinetLength { get; set; }
        public string CabinetLengthUnit { get; set; }
        public int CabinetWidth { get; set; }
        public string CabinetWidthUnit { get; set; }
        public int CabinetHeight { get; set; }
        public string CabinetHeightUnit { get; set; }
        public int CabinetWeight { get; set; }
        public string CabinetWeightUnit { get; set; }
    }
}
