using NA.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NA.Common.Extentions
{
    public static class LinqExtention
    {
        public static (IList<T> data, PagingModel paging) ToPaging<T>(this IQueryable<T> source, IPagingModel model)
        {
            var skip = model.page * model.size - model.size;
            //DUCNV: tong so trang
            var totalRecord = source.Count();//select count
            var totalPage = totalRecord / model.size;
            if (totalRecord % model.size != 0)
            {
                totalPage++;
            }
            var data = source.Skip(skip).Take(model.size).ToList();
            var paging = new PagingModel()
            {
                page = model.page,
                size = model.size,
                totalPage = totalPage,
                count = totalRecord,
                order = model.order
            };
            return (data, paging);            
        }

        public static (IList<T> data, PagingModel paging) ToPaging<T>(this IEnumerable<T> source, IPagingModel model)
        {
            var skip = model.page * model.size - model.size;
            //DUCNV: tong so trang
            var totalRecord = source.Count();//select count
            var totalPage = totalRecord / model.size;
            if (totalRecord % model.size != 0)
            {
                totalPage++;
            }
            var data = source.Skip(skip).Take(model.size).ToList();
            var paging = new PagingModel()
            {
                page = model.page,
                size = model.size,
                totalPage = totalPage,
                count = totalRecord,
                order = model.order
            };
            return (data, paging);
        }
    }
}
