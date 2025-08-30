using System.Collections;
using System.Collections.Generic;
using PetalsOfHope.Contracts;
using PetalsOfHope.Core.Events;
using PetalsOfHope.Data;
using PetalsOfHope.Data.VFX;
using UnityEngine;
using UnityEngine.Serialization;

namespace PetalsOfHope.Systems.VFX
{
    public class VFXSystem : MonoBehaviour
    {
        [Tooltip("The event channel to listen for VFX requests.")]
        [SerializeField] private VFXRequestEvent vfxRequestEvent;

        
        [Tooltip("List of VFX data to pre-warm the pool with.")]
        [SerializeField] private List<VFXData> initialVfxData;

        
        [Tooltip("Default initial pool size for each VFX type.")]
        [SerializeField] private int initialPoolSize = 10;

        private Dictionary<VFXType, Queue<ParticleSystem>> _vfxPool = new Dictionary<VFXType, Queue<ParticleSystem>>();
        private Dictionary<VFXType, GameObject> _vfxPrefabs = new Dictionary<VFXType, GameObject>();

        private void OnEnable()
        {
            if (vfxRequestEvent != null)
            {
                vfxRequestEvent.OnEventRaised += PlayEffect;
            }
        }

        private void OnDisable()
        {
            if (vfxRequestEvent != null)
            {
                vfxRequestEvent.OnEventRaised -= PlayEffect;
            }
        }

        private void Awake()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            foreach (var vfxData in initialVfxData)
            {
                if (vfxData.VfxPrefab == null) continue;

                RegisterVFXPrefab(vfxData.VfxType, vfxData.VfxPrefab);

                if (!_vfxPool.ContainsKey(vfxData.VfxType))
                {
                    _vfxPool[vfxData.VfxType] = new Queue<ParticleSystem>();
                }

                for (int i = 0; i < initialPoolSize; i++)
                {
                    GameObject vfxInstanceObj = Instantiate(vfxData.VfxPrefab, transform);
                    ParticleSystem ps = vfxInstanceObj.GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        vfxInstanceObj.SetActive(false);
                        _vfxPool[vfxData.VfxType].Enqueue(ps);
                    }
                    else
                    {
                        Debug.LogWarning($"VFX Prefab '{vfxData.VfxPrefab.name}' for '{vfxData.VfxType}' does not have a ParticleSystem component.", vfxData.VfxPrefab);
                        Destroy(vfxInstanceObj);
                    }
                }
            }
        }

        public void RegisterVFXPrefab(VFXType vfxType, GameObject prefab)
        {
            if (!_vfxPrefabs.ContainsKey(vfxType))
            {
                _vfxPrefabs[vfxType] = prefab;
            }
        }

        public void PlayEffect(IVFXRequestData requestData)
        {
            if (!_vfxPool.ContainsKey(requestData.VfxType) || !_vfxPrefabs.ContainsKey(requestData.VfxType))
            {
                Debug.LogWarning($"VFX with type '{requestData.VfxType}' not registered or pooled.", this);
                return;
            }

            ParticleSystem psToPlay = null;
            if (_vfxPool[requestData.VfxType].Count > 0)
            {
                psToPlay = _vfxPool[requestData.VfxType].Dequeue();
                psToPlay.gameObject.SetActive(true);
            }
            else
            {
                GameObject newVfxObj = Instantiate(_vfxPrefabs[requestData.VfxType]);
                psToPlay = newVfxObj.GetComponent<ParticleSystem>();
                if (psToPlay == null)
                {
                    Debug.LogError($"Instantiated VFX '{requestData.VfxType}' but it has no ParticleSystem component!");
                    Destroy(newVfxObj);
                    return;
                }
            }

            if (psToPlay != null)
            {
                psToPlay.transform.position = requestData.Position;
                psToPlay.transform.rotation = requestData.Rotation;

                psToPlay.Clear();
                psToPlay.Play();

                StartCoroutine(ReturnToPoolAfterDuration(requestData.VfxType, psToPlay, psToPlay.main.duration + psToPlay.main.startLifetime.constantMax));
            }
        }

        private IEnumerator ReturnToPoolAfterDuration(VFXType vfxType, ParticleSystem psInstance, float duration)
        {
            yield return new WaitForSeconds(duration);
            if (psInstance != null && psInstance.gameObject.activeSelf)
            {
                psInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                psInstance.gameObject.SetActive(false);
                psInstance.transform.SetParent(transform);

                if (_vfxPool.ContainsKey(vfxType))
                {
                    _vfxPool[vfxType].Enqueue(psInstance);
                }
                else
                {
                    Destroy(psInstance.gameObject);
                }
            }
        }
    }
}
