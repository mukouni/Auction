using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities.Enums;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    /// <summary>
    /// 基础模型
    /// </summary>
    public abstract class BaseEntity
    {
        // private Guid id = Guid.Empty;

        [Column(Order = 1)]
        /// <summary>
        /// GUID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(Order = 101)]
        public IsDeleted? IsDeleted { get; set; } = CommonEnum.IsDeleted.No;

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(Order = 102)]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// 最近修改时间
        /// EF.Property<DateTime>(b, "LastUpdated")
        /// context.Entry(myBlog).Property("LastUpdated").CurrentValue
        /// EF Code Model 中定义的shadow(隐藏、卷影，直译阴影)属性LastUpdated和这个字段是同一个作用。两者不确定相同，但是可以肯定时间相近
        /// </summary>
        [Column(Order = 103)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdatedAt { get; set; }

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