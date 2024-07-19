using Modding;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PvpArena
{
    class PvpArena : Mod
    {
        public Consts.LanguageStrings LangStrings { get; private set; }
        public SceneChanger SceneChanger { get; private set; }

        public override string GetVersion() => SFCore.Utils.Util.GetVersion(Assembly.GetExecutingAssembly());

        public override List<ValueTuple<string, string>> GetPreloadNames()
        {
            return new List<ValueTuple<string, string>>
            {
                new ValueTuple<string, string>("Town", "_SceneManager"),
                new ValueTuple<string, string>("White_Palace_18", "White Palace Fly"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw"),
                new ValueTuple<string, string>("White_Palace_18", "saw_collection/wp_saw (2)"),
                new ValueTuple<string, string>("White_Palace_18", "Soul Totem white_Infinte"),
                new ValueTuple<string, string>("White_Palace_18", "Area Title Controller"),
                new ValueTuple<string, string>("White_Palace_18", "glow response lore 1/Glow Response Object (11)"),
                new ValueTuple<string, string>("White_Palace_18", "Inspect Region"),
                new ValueTuple<string, string>("White_Palace_18", "_Managers/PlayMaker Unity 2D"),
                new ValueTuple<string, string>("White_Palace_18", "Music Region (1)"),
                new ValueTuple<string, string>("White_Palace_17", "WP Lever"),
                new ValueTuple<string, string>("White_Palace_17", "White_ Spikes"),
                new ValueTuple<string, string>("White_Palace_17", "Cave Spikes Invis"),
                new ValueTuple<string, string>("White_Palace_09", "Quake Floor"),
                new ValueTuple<string, string>("Grimm_Divine", "Charm Holder"),
                new ValueTuple<string, string>("White_Palace_03_hub", "WhiteBench"),
                new ValueTuple<string, string>("Crossroads_07", "Breakable Wall_Silhouette"),
                new ValueTuple<string, string>("Deepnest_East_Hornet_boss", "Hornet Outskirts Battle Encounter"),
                new ValueTuple<string, string>("White_Palace_03_hub", "door1"),
                new ValueTuple<string, string>("White_Palace_03_hub", "Dream Entry")
            };
        }

        public PvpArena() : base("PvP Arena")
        {
            LangStrings = new Consts.LanguageStrings();

            InitCallbacks();
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            SceneChanger = new SceneChanger(preloadedObjects);

            Log("Initialized");
        }

        private void InitCallbacks()
        {
            // Hooks
            ModHooks.LanguageGetHook += OnLanguageGetHook;
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;

            On.HealthManager.TakeDamage += HealthManagerOnTakeDamage;
        }

        private void HealthManagerOnTakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hitinstance)
        {
            Log($"HealthManager '{self.gameObject}' is about to get hit!");

            orig(self, hitinstance);

            Log($"HealthManager '{self.gameObject}' got hit!");
        }

        private string OnLanguageGetHook(string key, string sheet, string orig)
        {
            if (LangStrings.ContainsKey(key, sheet))
            {
                return LangStrings.Get(key, sheet);
            }
            return orig;
        }

        private void OnSceneChanged(Scene from, Scene to)
        {
            if (!to.name.Equals("Town")) return;
            SceneChanger.Change_Town(to);
        }
    }
}
