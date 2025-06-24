using PetalsOfHope.Core.Persistence.Interfaces;
using UnityEngine;

namespace PetalsOfHope.Core.Persistence.Services
{
    [CreateAssetMenu(menuName = "Petals of Hope/Data Services/PlayerPrefs Data Service")]
    public class PlayerPrefsDataServiceSO : IDataServiceSO
    {
        private readonly PlayerPrefsDataService _service = new PlayerPrefsDataService();

        public override bool Save<T>(string key, T data, bool encrypt = false) 
            => _service.Save(key, data, encrypt);

        public override T Load<T>(string key, bool decrypt = false) 
            => _service.Load<T>(key, decrypt);

        public override bool HasKey(string key) 
            => _service.HasKey(key);

        public override bool Delete(string key) 
            => _service.Delete(key);

        public override bool DeleteAll() 
            => _service.DeleteAll();
    }
}