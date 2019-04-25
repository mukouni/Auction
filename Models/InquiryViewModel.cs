using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Auction.Models
{
    public class InquiryViewModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Index(0)]
        [Name("姓名")]
        [Display(Name = "姓名")]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Index(1)]
        [Name("手机号")]
        [Display(Name = "手机号")]
        public string Tel { get; set; }
        /// <summary>
        /// email
        /// </summary>
        [Index(2)]
        [Name("邮箱")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 启运港
        /// </summary>
        [Index(3)]
        [Name("启运港")]
        [Display(Name = "启运港")]
        public string StartDeparture { get; set; }

        /// <summary>
        /// 目的港
        /// </summary>
        [Index(4)]
        [Name("目的港")]
        [Display(Name = "目的港")]
        public string EndDeparture { get; set; }

        /// <summary>
        /// 特种箱
        /// </summary>
        [Index(4)]
        [Name("特种箱列表")]
        [Display(Name = "特种箱列表")]
        public ICollection<Cabinet> Cabinets { get; set; } = new List<Cabinet>();

        [Index(5)]
        [Name("Gp20")]
        [Display(Name = "20'Gp")]
        public string Gp20 { get; set; }
        [Index(6)]
        [Name("Gp40")]
        [Display(Name = "40'Gp")]
        public string Gp40 { get; set; }
        [Display(Name = "40'Hc")]
        [Index(7)]
        [Name("Hc40")]
        public string Hc40 { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        [Index(8)]
        [Name("到厂拖车")]
        [Display(Name = "到厂拖车")]
        public string MotorcycleFactory { get; set; }

        /// <summary>
        /// 仓库装柜
        /// </summary>
        [Index(9)]
        [Name("仓库装柜")]
        [Display(Name = "仓库装柜")]
        public string WarehouseLoading { get; set; }
        /// <summary>
        /// 上门绑扎
        /// </summary>
        [Index(10)]
        [Name("上门绑扎")]
        [Display(Name = "上门绑扎")]
        public string HomeLashing { get; set; }
        /// <summary>
        /// 代理报关
        /// </summary>
        [Index(11)]
        [Name("代理报关")]
        [Display(Name = "代理报关")]
        public string CustomsDeclaration { get; set; }

        /// <summary>
        /// 出运时间
        /// </summary>
        [Index(12)]
        [Name("出运时间")]
        [Display(Name = "出运时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShipmentTime { get; set; }
    }

    public class Cabinet
    {
        [Index(0)]
        [Name("类型")]
        public string CabinetType { get; set; }
        [Index(1)]
        [Name("数量")]
        public int CabinetCount { get; set; }
        [Index(2)]
        [Name("长度单位")]
        public int CabinetLength { get; set; }
        [Index(2)]
        [Name("长度单位")]
        public string CabinetLengthUnit { get; set; }
        [Index(3)]
        [Name("宽度")]
        public int CabinetWidth { get; set; }
        [Index(4)]
        [Name("宽度单位")]
        public string CabinetWidthUnit { get; set; }
        [Index(5)]
        [Name("高度")]
        public int CabinetHeight { get; set; }
        [Index(5)]
        [Name("高度单位")]
        public string CabinetHeightUnit { get; set; }
        [Index(6)]
        [Name("重量")]
        public int CabinetWeight { get; set; }
        [Index(6)]
        [Name("重量单位")]
        public string CabinetWeightUnit { get; set; }
    }

    public class CabinetMap : ClassMap<Cabinet>
    {
        public CabinetMap()
        {
            Map(m => m.CabinetCount).Name("数量").Index(0);
            Map(m => m.CabinetLength + m.CabinetLengthUnit).Name("长度").Index(1);
            Map(m => m.CabinetWidth + m.CabinetWidthUnit).Name("宽度").Index(2);
            Map(m => m.CabinetHeight + m.CabinetHeightUnit).Name("高度").Index(3);
            Map(m => m.CabinetWeight + m.CabinetWeightUnit).Name("重量").Index(4);
        }
    }
}
