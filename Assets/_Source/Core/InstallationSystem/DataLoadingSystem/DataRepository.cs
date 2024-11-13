using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Core.InstallationSystem.DataLoadingSystem
{
    public class DataRepository<T> : IRepository<T> where T : Object
    {
        public Dictionary<Type, List<T>> Data { get; private set; } = new();

        public void Create(Type key, Object[] res)
        {
            Data.Add(key, new List<T>());
            for (int i = 0; i < res.Length; i++)
            {
                Data[key].Add(res[i] as T);
            }
        }

        public void Delete(Type key, T item)
        {
            if (Data.ContainsKey(key))
                Data[key].Remove(item);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<R> GetItem<R>() where R : class
        {
            List<R> newList = new();
            foreach (T so in Data[typeof(R)])
            {
                newList.Add(so as R);
            }
            return newList;
        }

        public int GetCount()
        {
            return Data.Count;
        }
    }
}