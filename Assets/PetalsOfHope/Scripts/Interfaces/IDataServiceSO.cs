// IDataServiceSO.cs
using UnityEngine;

namespace PetalsOfHope.Interfaces
{
    /// <summary>
    /// Base class for ScriptableObject-based data services
    /// </summary>
    public abstract class IDataServiceSO : ScriptableObject, IDataService
    {
        public abstract bool Save<T>(string key, T data, bool encrypt = false);
        public abstract T Load<T>(string key, bool decrypt = false);
        public abstract bool HasKey(string key);
        public abstract bool Delete(string key);
        public abstract bool DeleteAll();
    }
}
