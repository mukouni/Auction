using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    /// <summary>
    /// 二手设备
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }

        public IsDeleted IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        public Guid CreatedByUserGuid { get; set; }

        /// <summary>
        /// 创建者姓名
        /// </summary>
        public string CreatedByUserName { get; set; }

        /// <summary>
        /// 最近修改时间
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// 最近修改者ID
        /// </summary>
        public Guid? ModifiedByUserGuid { get; set; }

        /// <summary>
        /// 最近修改者姓名
        /// </summary>
        public string ModifiedByUserName { get; set; }
    }
}