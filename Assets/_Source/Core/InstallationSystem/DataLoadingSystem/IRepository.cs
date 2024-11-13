using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace Core.InstallationSystem.DataLoadingSystem
{
    public interface IRepository<T> : IDisposable where T : class
    {
        Dictionary<Type, List<T>> Data { get; }
        void Create(Type key, Object[] res);
        void Delete(Type key, T item);
        List<R> GetItem<R>() where R : class;
        int GetCount();
    }
}