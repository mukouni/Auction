using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    [Table("ac_photo")]
    public class Photo : BaseEntity
    {
        /// <summary>
        /// 浏览器中请求的路径，包含文件名和后缀
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string RequestPath { get; set; }

        /// <summary>
        /// 图片存储的相对路径，包含文件名和后缀, 不包含FilesRootDir
        /// </summary>
        [Column(TypeName = "nvarchar(255)")]
        public string SavePath { get; set; }

        /// <summary>
        /// 上传后名字(含后缀)
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

        /// <summary
        /// 图片排序
        /// </summary>
        public int? Ranking { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string ContentType { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        public long? FileSize { get; set; }

        public Guid EquipmentId { get; set; }

        /// <summary>
        /// 是否在售出后隐藏
        /// </summary>
        public bool? IsHiddenAfterSold { get; set; }

        //导航属性
        // [JsonIgnore]
        public virtual Equipment Equipment { get; set; }
        public Guid? CoverEquipmentId { get; set; }
        /// <summary>
        /// 封皮照片的设备
        /// </summary>
        [ForeignKey(nameof(CoverEquipmentId))]
        public virtual Equipment CoverEquipment { get; set; }

        public Guid? ExteriorEquipmentId { get; set; }
        /// <summary>
        /// 外观照片的设备
        /// </summary>
        [ForeignKey(nameof(ExteriorEquipmentId))]
        public virtual Equipment ExteriorEquipment { get; set; }


        public Guid? TrackedChassisEquipmentId { get; set; }
        /// <summary>
        /// 履带底架照片的设备
        /// </summary>
        [ForeignKey(nameof(TrackedChassisEquipmentId))]
        public virtual Equipment TrackedChassisEquipment { get; set; }

        public Guid? CabEquipmentId { get; set; }
        /// <summary>
        /// 驾驶室照片的设备
        /// </summary>
        [ForeignKey(nameof(CabEquipmentId))]
        public virtual Equipment CabEquipment { get; set; }

        public Guid? BoomEquipmentId { get; set; }
        /// <summary>
        /// 臂架照片的设备
        /// </summary>
        [ForeignKey(nameof(BoomEquipmentId))]
        public virtual Equipment BoomEquipment { get; set; }


        public Guid? EngineEquipmentId { get; set; }
        /// <summary>
        /// 引擎照片的设备
        /// </summary>
        [ForeignKey(nameof(EngineEquipmentId))]
        public virtual Equipment EngineEquipment { get; set; }


        /// IFormFile中的FileName是有后缀名的，数据库中的FileName是没有后缀名的
        /// <summary>
        /// 从IFormFile中取数据赋值到Photo中
        /// <param name="serializaName">序列化后的文件名，包含后缀名</param>
        /// <param name="ImagesRequestPath">浏览器中请求的路径，包含文件名和后缀</param>
        /// <param name="ImagesSavePath">图片存储的相对路径，包含文件名和后缀, 不包含FilesRootDir</param>
        /// </summary>
        public Photo MapFrom(IFormFile photo, string serializaName, string RequestPath, string SavePath)
        {
            this.RequestPath = RequestPath;
            this.SavePath = SavePath;
            this.FileName = serializaName;
            this.Extension = Path.GetExtension(photo.FileName);
            this.OriginName = photo.FileName;
            this.ContentType = photo.ContentType;
            this.FileSize = photo.Length;
            return this;
        }

        // public string FullFileName(string size = null)
        // {
        //     if (size == null)
        //     {
        //         return FileName + "." + Extension;
        //     }
        //     return FileName + "_" + size + "x" + size + "_" + "." + Extension;
        // }

        // public string FileRequestPath(string size = null)
        // {

        //     return Path.Combine(RequestPath, FullFileName(size));
        // }


    }
}