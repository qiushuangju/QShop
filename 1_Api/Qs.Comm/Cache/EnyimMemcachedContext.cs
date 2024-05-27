// ***********************************************************************
// Assembly         : Qs.Comm
// Author           : Qs
// Created          : 06-21-2016
//
// Last Modified By : Qs
// Last Modified On : 06-21-2016
// Contact : Add services.AddEnyimMemcached(...)
//                  and app.UseEnyimMemcached() in Startup. 
// File: EnyimMemcachedContext.cs
// ***********************************************************************


using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Qs.Comm.Cache;

namespace Qs.Comm.Cache
{
    public sealed class EnyimMemcachedContext : ICacheContext
    {
        private  IMemcachedClient _memcachedClient;

        public EnyimMemcachedContext(IMemcachedClient client)
        {
            _memcachedClient = client;
        }

        public override T Get<T>(string key)
        {
            return _memcachedClient.Get<T>(key);
        }

        public override bool Set<T>(string key, T t, DateTime expire)
        {
            return _memcachedClient.Store(StoreMode.Set, key, t, expire);
        }

        public override bool Remove(string key)
        {
            return _memcachedClient.Remove(key);
        }
    }
}