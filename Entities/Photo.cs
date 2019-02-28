using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Entities
{
    [Table("ac_photo")]
    public class Photo : BaseEntity
    {
        /// 存储的相对路径
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string StoreDir { get; set; }

        /// <summary>
        /// 上传后名字
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string FileName { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Extension { get; set; }

        /// <summary>
        /// 上传前名字
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string OriginName { get; set; }

        /// <summary>
        /// 图片排序
        /// </summary>
        public int? Ranking { get; set; }

        /// <summary>
        /// 是否是展示图片
        /// </summary>
        public Boolean? IsHome { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string ContentType { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        public int? FileSize { get; set; }

        //导航属性
        public Equipment EquipmentPhoto { get; set; }
    }
}