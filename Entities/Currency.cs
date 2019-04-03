using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Entities.Enums;
using static Auction.Entities.Enums.CommonEnum;

namespace Auction.Entities
{
    [Table("st_currency")]
    public class Currency
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 符号
        /// </summary>
        public string SymbolCode { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// iso
        /// </summary>
        public string ISOCode { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}