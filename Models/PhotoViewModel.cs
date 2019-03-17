using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auction.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models
{
    public class PhotoViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 存储的相对路径
        /// </summary>
        [Display(Name = "相对路径")]
        public string RequestPath { get; set; }

        /// <summary>
        /// 图片存储的相对路径，包含文件名和后缀, 不包含FilesRootDir
        /// </summary>
        public string SavePath { get; set; }

        /// <summary>
        /// 上传后名字(不含后缀)
        /// </summary>
        [Display(Name = "序列化文件名")]
        public string FileName { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [Display(Name = "文件后缀")]
        public string Extension { get; set; }

        /// <summary>
        /// 上传前名字
        /// </summary>
        [Display(Name = "上传前名字")]
        public string OriginName { get; set; }

        /// <summary>
        /// 图片排序
        /// </summary>
        [Display(Name = "图片排序")]
        public int? Ranking { get; set; }

        /// <summary>
        /// 是否是展示图片
        /// </summary>
        [Display(Name = "是否是展示图片")]
        public Boolean? IsCover { get; set; }

        /// <summary>
        /// 图片格式
        /// </summary>
        [Display(Name = "图片格式")]
        public string ContentType { get; set; }

        /// <summary>
        /// 图片大小
        /// </summary>
        [Display(Name = "图片大小")]
        public long? FileSize { get; set; }

        [Display(Name = "设备ID")]
        public Guid EquipmentId { get; set; }

        // public string FullFileName { get; set; }

        public string FileRequestPath { get; set; }

        public IsDeleted? IsDelete { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        // [JsonIgnore]
        public virtual Equipment Equipment { get; set; }

        // public string FullFileName(string size = null)
        // {
        //     if (size == null)
        //     {
        //         return FileName + Extension;
        //     }
        //     return FileName + "_" + size + "x" + size + "_" + Extension;
        // }

        // public string FileRequestPath(string size = null)
        // {

        //     return Path.Combine(_appSettings.ImagesRequestPath + RequestPath, FullFileName(size));
        // }

    }
}