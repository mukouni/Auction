using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    /// <summary>
    /// 基础模型
    /// </summary>
    public abstract class BaseEntity
    {
        [Column(Order = 100)]
        /// <summary>
        /// GUID
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [DefaultValue("newid()")]
        public Guid Guid { get; set; }

        [Column(Order = 101)]
        public IsDeleted? IsDelete { get; set; } = IsDeleted.No;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Order = 102)]
        public DateTime? CreatedOn { get; set; }


         /// <summary>
        /// 最近修改时间
        /// </summary>
        [Column(Order = 103)]
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        [Column(Order = 104)]
        public Guid? CreatedByUserGuid { get; set; }

        /// <summary>
        /// 创建者姓名
        /// </summary>
        [Column(Order = 105, TypeName = "nvarchar(50)")]
        public string CreatedByUserName { get; set; }

        /// <summary>
        /// 最近修改者ID
        /// </summary>

        [Column(Order = 106)]
        public Guid? ModifiedByUserGuid { get; set; }

        /// <summary>
        /// 最近修改者姓名
        /// </summary>
        [Column(Order = 107, TypeName = "nvarchar(50)")]
        public string ModifiedByUserName { get; set; }
    }
}