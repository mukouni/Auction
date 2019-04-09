using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System.Linq;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Auction.Models.EquipmentViewModels;
using System.Linq.Expressions;
using Auction.Entities;
using Auction.Identity.Entities;

namespace Auction.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class QueryableExtension
    {
        /// <summary>
        /// IQueryable分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> Paged<T>(this IQueryable<T> query, int currentPage = 1, int pageSize = 20)
        {
            if (currentPage < 1)
            {
                currentPage = 1;
            }
            query = query.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return query;
        }

        public static IQueryable<T> AuctionSort<T>(this IQueryable<T> query, SearchEquipmentViewModel searchEquipment) where T : Equipment
        {
            if (searchEquipment.Sort?.Field != null && searchEquipment.Sort?.Direction != null)
            {
                Expression<Func<T, Object>> orderByFunc = null;
                if ("WorkingTime" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.WorkingTime;
                else if ("ProductionDate" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.ProductionDate;
                else if ("SoldAt" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.SoldAt;
                else if ("Name" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.Name;
                else if ("Price" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.Price;

                if (searchEquipment.Sort.Direction == "desc")
                {
                    query = query.OrderByDescending(orderByFunc);
                }
                else
                {
                    query = query.OrderBy(orderByFunc);
                }
            }
            else
            {
                query = query.OrderBy(e => e.CreatedAt);
            }
            return query;
        }


        public static IQueryable<T> UsersSort<T>(this IQueryable<T> query, SearchEquipmentViewModel searchEquipment) where T : ApplicationUser
        {
            if (searchEquipment.Sort?.Field != null && searchEquipment.Sort?.Direction != null)
            {
                Expression<Func<T, Object>> orderByFunc = null;
                if ("RealName" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.RealName;
                else if ("DeadlineAt" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.DeadlineAt;
                else if ("CreatedAt" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.CreatedAt;
                else if ("PhoneNumber" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.PhoneNumber;
                else if ("Email" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.Email;
                else if ("IsDeleted" == searchEquipment.Sort?.Field)
                    orderByFunc = item => item.IsDeleted;

                if (searchEquipment.Sort.Direction == "desc")
                {
                    query = query.OrderByDescending(orderByFunc);
                }
                else
                {
                    query = query.OrderBy(orderByFunc);
                }
            }
            else
            {
                query = query.OrderBy(e => e.CreatedAt);
            }
            return query;
        }



        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");
        private static readonly FieldInfo QueryModelGeneratorField = typeof(QueryCompiler).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryModelGenerator");
        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");
        private static readonly PropertyInfo DatabaseDependenciesField = typeof(Microsoft.EntityFrameworkCore.DbLoggerCategory.Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ToSql<TEntity>(this IQueryable<TEntity> query)
        {
            var queryCompiler = (QueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var queryModelGenerator = (QueryModelGenerator)QueryModelGeneratorField.GetValue(queryCompiler);
            var queryModel = queryModelGenerator.ParseQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var databaseDependencies = (DatabaseDependencies)DatabaseDependenciesField.GetValue(database);
            var queryCompilationContext = databaseDependencies.QueryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }
}
