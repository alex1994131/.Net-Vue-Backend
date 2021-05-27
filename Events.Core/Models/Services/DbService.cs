

using Events.Api.Models.General;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Events.Core.Models.Services
{
    public interface DbService<T,V> where T : Model
    {

        long GetLastId();
        T GetFirst(Expression<Func<T, bool>> predicate);
        T Find(Expression<Func<T, bool>> predicate);
        T Find(long id);
        void Delete(T entity);
        List<V> GetItems(Expression<Func<V, bool>> predicate, Int16 size = 0);
        Int64 GetCount(Expression<Func<V, bool>> predicate);
        Task<T> AddItem(T entity);
        Task<T> AddItemAsync(T entity);
        void UpdateEntity(Expression<Func<T, bool>> predicate, T entity);
        Task<T> UpdateEntity(T entity);
        IOrderedQueryable<V> GetViewQuery();
        IOrderedQueryable<T> GetQuery();
        String UploadFile(Attachment file, IWebHostEnvironment host);
        FileStreamResult DownloadFile(long file, IWebHostEnvironment host);
    }
}
