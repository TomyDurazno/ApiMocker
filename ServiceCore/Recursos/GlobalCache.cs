using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore.Recursos
{
    public class GlobalCache : IGlobalCache
    {
        #region Constantes

        public string Name { get; set; }

        #endregion

        public GlobalCache(string name)
        {
            Name = name;
        }

        /// <summary> Método usado para guardar un objeto en caché
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="userName"></param>
        /// <param name="nombreCache"></param>
        public void SetItem<T>(T item, int expiresIn)
        {
            var cache = MemoryCache.Default;
            var policy = new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expiresIn) };
            cache.Set(Name, item, policy);
        }

        /// <summary> Método usado para traer un objeto previamente guardado en caché
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userName"></param>
        /// <param name="nombreCache"></param>
        /// <returns></returns>
        public T GetItem<T>()
        {
            var cache = MemoryCache.Default;
            return (T)cache[Name];
        }

        /// <summary> Método usado para borrar un objeto previamente guardado en caché
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="nombreCache"></param>
        public void RemoveItem()
        {
            var cache = MemoryCache.Default;
            cache.Remove(Name);
        }
    }

    public interface IGlobalCache
    {
        string Name { get; set; }
        void SetItem<T>(T item, int expiresIn);
        T GetItem<T>();
        void RemoveItem();
    }
}
