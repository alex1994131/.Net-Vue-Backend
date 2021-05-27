
using Events.Api.Models.General;
using Events.Core.Models;
using Events.Core.Models.Services;
using Events.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Events.Service.Service.DataServices
{
    public abstract class DbServiceImpl<T, V> : DbService<T, V> where T : Model
    {
        
        protected readonly AppDbContext context;

        public DbServiceImpl(AppDbContext ctx) => context = ctx;

        public async Task<T> AddItem(T entity)
        {
           context.Add(entity);
           await context.SaveChangesAsync();
           return GetLastAdded();
        }
        public async Task<T> AddItemAsync(T entity)
        {
            context.Add(entity);
            await context.SaveChangesAsync();
            return GetLastAdded();
        }
        public async Task<T> UpdateEntity(T entity) {
            context.Entry(entity).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
            return Find(entity.Id);
            
        }
        public void UpdateEntity(Expression<Func<T, bool>> predicate, T entity) {
            T t = Find(predicate);
            context.Entry(t).CurrentValues.SetValues(entity);
            context.SaveChangesAsync();
        }
        public T GetLastAdded()
        => Find(x => x.Id == GetLastId());
        public T Find(long id)
        => GetQuery().Where(x => x.Id == id).SingleOrDefault();
        public void Delete(T entity)
        => context.Remove(entity);
        public T Find(Expression<Func<T, bool>> predicate)
        => GetQuery().Where(predicate).FirstOrDefault();
        public T GetFirst(Expression<Func<T, bool>> predicate)
        => GetQuery().Where(predicate).FirstOrDefault();
        public List<V> GetItems(Expression<Func<V, bool>> predicate,Int16 size =0)
        => size > 0 ? GetViewQuery().Where(predicate).Take(size).ToList() : GetViewQuery().Where(predicate).ToList();
        public abstract IOrderedQueryable<T> GetQuery();
        public long GetLastId()
        => GetQuery().Max(x => x.Id);
        public abstract IOrderedQueryable<V> GetViewQuery();

        public long GetCount(Expression<Func<V, bool>> predicate)
        => GetViewQuery().Where(predicate).Count();

        public String UploadFile(Attachment file, IWebHostEnvironment host) {
            try
            {
                var uploadsRootFolder = Path.Combine(host.ContentRootPath, "uploads");
               
                Random random = new Random();
                int length = 16;
                var rString = "";
                for (var i = 0; i < length; i++)
                {
                    rString += ((char)(random.Next(1, 26) + 64)).ToString().ToLower();
                }
                

                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }

                var filePath = Path.Combine(uploadsRootFolder, rString + "."+file.Extension);
                File.WriteAllBytes(filePath, file.Content);
                return rString;
                
            }
            catch ( Exception exception)
            {
                return exception.Message;
            }
        }
        public FileStreamResult DownloadFile(long file, IWebHostEnvironment host)
        {
            Attachment attachment = context.Attachments.Find(file);

            try
            {
                var uploadsRootFolder = Path.Combine(host.ContentRootPath, "uploads");
                var filePath = Path.Combine(uploadsRootFolder, attachment.Url + "." + attachment.Extension);

                var stream = File.OpenRead(filePath);
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/octet-stream");
                return fileStreamResult;

            }
            catch (Exception exception)
            {
 
                throw new FileNotFoundException();
            }
        }



    }
}
