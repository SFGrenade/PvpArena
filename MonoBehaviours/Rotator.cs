using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SFCore.Utils;
using UnityEngine;
using Logger = Modding.Logger;

namespace PvpArena.MonoBehaviours
{
    public class Rotator : MonoBehaviour
    {
        public int divider = 4;
        public bool rotating = false;
        public Transform[] transformsToRotate = new Transform[0];

        public void Awake()
        {
            var hmPrefab = Resources.FindObjectsOfTypeAll<HealthManager>()[0];
            var hm = this.gameObject.AddComponent<HealthManager>();

            Dictionary<string, FieldInfo> fis = new Dictionary<string, FieldInfo>();
            foreach (var fi in typeof(HealthManager).GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                fis.Add(fi.Name, fi);
                fi.SetValue(hm, fi.GetValue(hmPrefab));
            }

            fis["boxCollider"].SetValue(hm, null);
            fis["recoil"].SetValue(hm, null);
            fis["hitEffectReceiver"].SetValue(hm, null);
            fis["enemyDeathEffects"].SetValue(hm, null);
            fis["animator"].SetValue(hm, null);
            fis["sprite"].SetValue(hm, null);
            fis["damageHero"].SetValue(hm, null);
            fis["audioPlayerPrefab"].SetValue(hm, null);
            fis["regularInvincibleAudio"].SetValue(hm, null);
            fis["enemyType"].SetValue(hm, 0);
            fis["effectOrigin"].SetValue(hm, Vector3.zero);
            fis["ignoreKillAll"].SetValue(hm, true);
            fis["invincible"].SetValue(hm, false);
            fis["invincibleFromDirection"].SetValue(hm, -1);
            fis["preventInvincibleEffect"].SetValue(hm, true);
            fis["hasAlternateInvincibleSound"].SetValue(hm, false);
            fis["alternateInvincibleSound"].SetValue(hm, null);
            fis["deathAudioSnapshot"].SetValue(hm, null);
            fis["semiPersistent"].SetValue(hm, false);
            fis["stunControlFSM"].SetValue(hm, null);

            hm.hp = 99999;
        }

        private void FixedUpdate()
        {
            if (!rotating) return;
            foreach (var t in transformsToRotate)
            {
                t.localEulerAngles = new Vector3(0, 0, (Mathf.Round(t.localEulerAngles.z * (float) divider) / (float) divider) - (1f / (float) divider));
            }
        }

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            Log($"Collider: '{otherCollider.gameObject}'");
            //otherCollider.transform.Log();
            if (otherCollider.gameObject.GetComponentInChildren<NailSlash>() == null) return;

            rotating = true;
        }

        private void Log(string message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }

        private void Log(object message)
        {
            Logger.Log($"[{GetType().FullName?.Replace(".", "]:[")}] - {message}");
        }
    }
}
