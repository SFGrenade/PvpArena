using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PvpArena.MonoBehaviours;

[UsedImplicitly]
public class PrefabicleSystem : MonoBehaviour
{
    public GameObject prefab;
    public float spawnsPerSecond = 10f;
    public bool spawnGlobally = true;
    public float spawnRadius = 0f;
    public float maxVelocity = 0f;
    public int maxPrefabicles = 100;
    public LayerMask layersToRemoveFrom;

    private float _timer = 0f;
    private List<GameObject> _instances = new();

    private void FixedUpdate()
    {
        if (prefab == null) return;

        _timer -= Time.fixedDeltaTime;
        if (_timer < 0)
        {
            Spawn();
            _timer = 1f / spawnsPerSecond;
        }

        while (_instances.Count > maxPrefabicles)
        {
            DestroyImmediate(_instances[0], true);
            _instances.RemoveAt(0);
        }
    }

    private void Spawn()
    {
        Vector3 posOffset = Random.insideUnitCircle;
        posOffset *= spawnRadius;
        Vector3 velocity = Random.insideUnitCircle;
        velocity *= maxVelocity;
        if (spawnGlobally)
        {
            GameObject tmpGo = Instantiate(prefab, transform.position + posOffset, prefab.transform.rotation);

            tmpGo.GetComponent<Rigidbody2D>().velocity = velocity;

            NotInCollider nic = tmpGo.AddComponent<NotInCollider>();
            nic.layersToRemoveFrom = layersToRemoveFrom;
            nic.callback = RemoveGo;

            _instances.Add(tmpGo);
        }
        else
        {
            GameObject tmpGo = Instantiate(prefab, transform);

            tmpGo.transform.localPosition = posOffset;
            tmpGo.transform.localRotation = prefab.transform.rotation;
            tmpGo.GetComponent<Rigidbody2D>().velocity = velocity;

            NotInCollider nic = tmpGo.AddComponent<NotInCollider>();
            nic.layersToRemoveFrom = layersToRemoveFrom;
            nic.callback = RemoveGo;

            _instances.Add(tmpGo);
        }
    }

    public void RemoveGo(GameObject go)
    {
        _instances.Remove(go);
        _timer = 0f;
    }

    public class NotInCollider : MonoBehaviour
    {
        public LayerMask layersToRemoveFrom;
        public Action<GameObject> callback;

        private void Start()
        {
            bool t = false;
            foreach (Collider2D c in GetComponentsInChildren<Collider2D>())
                t |= c.IsTouchingLayers(layersToRemoveFrom);

            if (t)
            {
                callback.Invoke(gameObject);
                Destroy(gameObject);
            }
        }
    }
}