using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using GlobalEnums;
using HutongGames.PlayMaker;
using On;
using Logger = Modding.Logger;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using HutongGames.PlayMaker.Actions;
using SFCore.MonoBehaviours;
using PvpArena.MonoBehaviours;
using PvpArena.Consts;
using PvpArena.MonoBehaviours.Patcher;
using UnityEngine.UI;
using UObject = UnityEngine.Object;
using SFCore.Utils;

namespace PvpArena
{
    public class SceneChanger : MonoBehaviour
    {
        private const bool _DEBUG = true;
        private const string _AB_PATH = "E:\\Github_Projects\\PvpArena Assets\\Assets\\AssetBundles\\";

        public AssetBundle AbPvpScene { get; private set; } = null;
        public AssetBundle AbPvpMat { get; private set; } = null;

        public SceneChanger(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            On.GameManager.RefreshTilemapInfo += OnGameManagerRefreshTilemapInfo;

            PrefabHolder.preloaded(preloadedObjects);

            Assembly _asm;

            #region Load AssetBundles
#pragma warning disable CS0162 // Unerreichbarer Code wurde entdeckt.
            Log("Loading AssetBundles");
            _asm = Assembly.GetExecutingAssembly();
            if (AbPvpScene == null)
            {
                if (!_DEBUG)
                {
                    using (Stream s = _asm.GetManifestResourceStream("PvpArena.Resources.pvparenascenes"))
                    {
                        if (s != null)
                        {
                            AbPvpScene = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbPvpScene = AssetBundle.LoadFromFile(_AB_PATH + "pvparenascenes");
                }
            }
            if (AbPvpMat == null)
            {
                if (!_DEBUG)
                {
                    using (Stream s = _asm.GetManifestResourceStream("PvpArena.Resources.pvparenaassets"))
                    {
                        if (s != null)
                        {
                            AbPvpMat = AssetBundle.LoadFromStream(s);
                        }
                    }
                }
                else
                {
                    AbPvpMat = AssetBundle.LoadFromFile(_AB_PATH + "pvparenaassets");
                }
            }
            Log("Finished loading AssetBundles");
#pragma warning restore CS0162 // Unerreichbarer Code wurde entdeckt.
            #endregion
        }

        private void OnGameManagerRefreshTilemapInfo(On.GameManager.orig_RefreshTilemapInfo orig, GameManager self, string targetScene)
        {
            orig(self, targetScene);
            if (targetScene == "PvP_Arena")
            {
                float width = 128;
                float height = 128;
                self.tilemap.width = (int)width;
                self.tilemap.height = (int)height;
                self.sceneWidth = width;
                self.sceneHeight = height;
                FindObjectOfType<GameMap>().SetManualTilemap(0, 0, width, height);
            }
        }

        public void Change_Town(Scene scene)
        {
            if (scene.name != "Town")
                return;

            Log("Change_Town()");

            CreateDoor("PvpArena-door1", new Vector3(137, 10.71355f, 0.2f), new Vector2(1, 4), "PvP_Arena", "left1", GameManager.SceneLoadVisualizations.Default);

            Log("Change_Town Done");
        }

        private void CreateDoor(string gateName, Vector3 pos, Vector2 size, string toScene, string entryGate, GameManager.SceneLoadVisualizations vis)
        {
            Log("!CreateDoor");

            GameObject gate = GameObject.Instantiate(GameObject.Find("door_mapper"));
            gate.name = gateName;
            gate.transform.position = pos;
            var tp = gate.GetComponent<TransitionPoint>();
            var bc = gate.GetComponent<BoxCollider2D>();
            bc.size = size;
            bc.isTrigger = true;
            tp.SetTargetScene(toScene);
            tp.entryPoint = entryGate;
            tp.alwaysEnterLeft = false;
            tp.alwaysEnterRight = false;
            tp.isADoor = true;

            GameObject rm = new GameObject("Hazard Respawn Marker");
            rm.transform.parent = gate.transform;
            rm.transform.SetPosition2D(pos.x, pos.y);
            var tmp = rm.AddComponent<HazardRespawnMarker>();
            tmp.respawnFacingRight = false;
            tp.respawnMarker = rm.GetComponent<HazardRespawnMarker>();
            tp.sceneLoadVisualization = vis;

            var doorFsm = gate.LocateMyFSM("Door Control");
            var doorFsmVars = doorFsm.FsmVariables;
            doorFsmVars.GetFsmString("New Scene").Value = toScene;
            doorFsmVars.GetFsmString("Entry Gate").Value = entryGate;

            Log("~CreateDoor");
        }

        private void printDebug(GameObject go, string tabindex = "")
        {
            Log(tabindex + "Name: " + go.name);
            foreach (var comp in go.GetComponents<Component>())
            {
                Log(tabindex + "Component: " + comp.GetType());
            }
            for (int i = 0; i < go.transform.childCount; i++)
            {
                printDebug(go.transform.GetChild(i).gameObject, tabindex + "\t");
            }
        }

        private void Log(String message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message}");
        }
        private void Log(System.Object message)
        {
            Logger.Log($"[{this.GetType().FullName.Replace(".", "]:[")}] - {message.ToString()}");
        }

        private static void SetInactive(GameObject go)
        {
            if (go != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
                go.SetActive(false);
            }
        }
        private static void SetInactive(UnityEngine.Object go)
        {
            if (go != null)
            {
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
        }
    }
}
