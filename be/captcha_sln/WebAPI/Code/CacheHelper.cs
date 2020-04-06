using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace WebAPI.Code
{
    public class CacheHelper
    {
        /// <summary>
        /// 索引器，提供一个全局读写
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return Cache.Get(key); }
            set { Add(key, value); }
        }

        protected MemoryCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }


        public int Count => (int)Cache.GetCount();

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime">分钟</param>
        public void Add(string key, object data, int cacheTime = 30)
        {
            if (Cache.Contains(key))
            {
                this.Remove(key);
            }
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 判断cache是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return Cache.Contains(key);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            if (Cache.Contains(key))
            {
                return (T)Cache[key];
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            Cache.Remove(key);
        }
        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public void RemoveAll()
        {
            foreach (var item in Cache)
            {
                this.Remove(item.Key);
            }
        }
    }
}