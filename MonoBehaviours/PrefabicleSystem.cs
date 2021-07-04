using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PvpArena.MonoBehaviours
{
    public class PrefabicleSystem : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnsPerSecond = 10f;
        public bool spawnGlobally = true;
        public float spawnRadius = 0f;
        public float maxVelocity = 0f;
        public int maxPrefabicles = 100;
        public LayerMask layersToRemoveFrom;

        private float timer = 0f;
        private List<GameObject> instances = new List<GameObject>();

        private void FixedUpdate()
        {
            if (prefab == null) return;

            timer -= Time.fixedDeltaTime;
            if (timer < 0)
            {
                Spawn();
                timer = 1f / spawnsPerSecond;
            }

            while (instances.Count > maxPrefabicles)
            {
                GameObject.DestroyImmediate(instances[0], true);
                instances.RemoveAt(0);
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
                var tmpGo = Instantiate(prefab, transform.position + posOffset, prefab.transform.rotation);

                tmpGo.GetComponent<Rigidbody2D>().velocity = velocity;

                var nic = tmpGo.AddComponent<NotInCollider>();
                nic.layersToRemoveFrom = layersToRemoveFrom;
                nic.callback = RemoveGo;

                instances.Add(tmpGo);
            }
            else
            {
                var tmpGo = Instantiate(prefab, transform);

                tmpGo.transform.localPosition = posOffset;
                tmpGo.transform.localRotation = prefab.transform.rotation;
                tmpGo.GetComponent<Rigidbody2D>().velocity = velocity;

                var nic = tmpGo.AddComponent<NotInCollider>();
                nic.layersToRemoveFrom = layersToRemoveFrom;
                nic.callback = RemoveGo;

                instances.Add(tmpGo);
            }
        }

        public void RemoveGo(GameObject go)
        {
            instances.Remove(go);
            timer = 0f;
        }

        public class NotInCollider : MonoBehaviour
        {
            public LayerMask layersToRemoveFrom;
            public Action<GameObject> callback;

            private void Start()
            {
                bool t = false;
                foreach (var c in this.GetComponentsInChildren<Collider2D>())
                    t |= c.IsTouchingLayers(layersToRemoveFrom);

                if (t)
                {
                    callback.Invoke(gameObject);
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }
}