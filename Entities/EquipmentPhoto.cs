using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auction.Entities
{
    /// <summary>
    /// 设备-图片映射
    /// </summary>
    [Table("ac_equipment_photo")]
    public class EquipmentPhoto
    {
        /// <summary>
        /// 设备GUID
        /// </summary>
        public Guid EquipmentGuid { get; set; }
        /// <summary>
        /// 设备实体
        /// </summary>
        public Equipment Equipment { get; set; }

        /// <summary>
        /// 图片Guid
        /// </summary>
        public Guid PhotoGuid { get; set; }
        /// <summary>
        /// 图片实体
        /// </summary>
        public Photo Photo { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedOn { get; set; }
    }
}
