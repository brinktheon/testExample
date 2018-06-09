using CommonLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Repositories
{
    public class CachedRepositary<T> : BaseRepository<T> where T : IIntegerKey, new()
    {
        protected static IDictionary<int, T> LocalCache = new Dictionary<int, T>();

        public CachedRepositary() { }

        /// <summary>
        /// Возвращает объет по ключу(Id) из кэша,
        /// если такого объекта нету в кэше, то выгружает 
        /// объект из БД и кладет в кэш.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T GetByKey(int id)
        {
            if (!LocalCache.TryGetValue(id, out T loaclObject))
            {
                loaclObject = base.GetByKey(id);
            }
            return loaclObject;
        }

        /// <summary>
        /// Возвращает список объектов по Linq запросу, 
        /// проверяет есть ли эти объекты в кэше, 
        /// если нет, то кладет их в кэш.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override IList<T> GetByLinq(Func<T, bool> predicate)
        {
            IList<T> localObjects = base.GetByLinq(predicate);
            foreach (T item in localObjects)
            {
                if (!LocalCache.ContainsKey(item.Id))
                {
                    LocalCache[item.Id] = item;
                }
            }
            return localObjects;
        }

        /// <summary>
        /// Возвращает список объектов по Sql запросу, 
        /// проверяет есть ли эти объекты в кэше, 
        /// если нет, то кладет их в кэш.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public override IList<T> GetByQuery(string query)
        {
            IList<T> localList = base.GetByQuery(query);
            foreach (T value in localList)
            {
                if (!LocalCache.ContainsKey(value.Id))
                {
                    LocalCache[value.Id] = value;
                }
            }
            return localList;
        }

        /// <summary>
        /// Возвращает списко объектов из кэша по Linq запросу
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IList<T> GetFromCacheByLinq(Func<T, bool> predicate)
        {
            return LocalCache.Values.Where(predicate).ToList();
        }

        /// <summary>
        /// Возвращает список объектов по из БД, 
        /// проверяет есть ли эти объекты в кэше, 
        /// если нет, то кладет их в кэш.
        /// </summary>
        /// <returns></returns>
        public override IList<T> Load()
        {
            IList<T> localList = base.Load();
            foreach (T value in localList)
            {
                LocalCache[value.Id] = value;
            }
            return localList;
        }
    }
}
