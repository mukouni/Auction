using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Auction.Models
{
    /// <summary>
    /// 请求实体
    /// </summary>
    public class RequestPayload
    {

        /// <summary>
        /// 
        /// </summary>
        public RequestPayload()
        {
            Sorts = new Sort[]{};
            KeyWord = "";
        }

        public List<SelectListItem> PageSizeOptions { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// 排序对象集合(支持多个字段排序)
        /// </summary>
        public Sort[] Sorts { get; set; }

        /// <summary>
        /// 排序对象集合(支持多个字段排序)
        /// </summary>
        public Sort Sort { get; set; }

        /// <summary>
        /// 组合后的排序字符串
        /// </summary>
        public string OrderBy
        {
            get
            {
                var orderBy = "";
                var sorts = Sorts.Where(x => string.IsNullOrEmpty(x.Field) && string.IsNullOrEmpty(x.Direction)).ToList();
                if (sorts.Count > 0)
                {
                    orderBy = "ORDER BY " + string.Join(",", sorts.Select(x => $"{x.Field} {x.Direction}"));
                }
                return orderBy;
            }
        }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string KeyWord { get; set; }

        public int Count { get; set; }
    }

    /// <summary>
    /// 排序实体对象
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// 排序实体对象构造函数
        /// </summary>
        public Sort()
        {
            Direction = "DESC";
        }
        
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// 排序方向
        /// </summary>
        public string Direction { get; set; }
    }

    public class Filter
    {
        /// <summary>
        /// 筛选字符串
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 匹配字符串的总行数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 条件是否选中
        /// </summary>
        public bool Selected { get; set; }

        
        /// <summary>
        /// 再filter[]中的排序
        /// </summary>
        public int SortNumber { get; set; }
    }
}
