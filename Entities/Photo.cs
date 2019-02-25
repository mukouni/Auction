using System;
using Auction.Entities;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Models
{
    public class Photo : BaseEntity
    {
        public int AttachmentId { get; set; }

        public string AttachmentType { get; set; }

        /// <summary>
        /// 存储的相对路径
        /// </summary>
        public string StoreDir { get; set; }

        /// <summary>
        /// 上传后名字
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 上传前名字
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 图片排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 是否是展示图片
        /// </summary>
        public Boolean IsHome { get; set; } = false;

        public ICollection<EquipmentPhoto> EquipmentPhotos { get; } = new List<EquipmentPhoto>(); 

    }
}