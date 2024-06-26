using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

namespace MT.HexTiles.Data
{
    //[CreateAssetMenu(menuName = "Steam/Achievement", order = 2)]
    public class AchievementVar : MonoBehaviour
    {
        [SerializeField] public string _key = "add achievement name here";
        [SerializeField] bool _done;
        [SerializeField] bool _autoSave = true;

        public bool Done { get { return (this._done); } }

        static bool Initialized;

        [ContextMenu("Initialize")]
        public void Awake()
        {
            if (!Initialized)
            {
                Initialized = SteamUserStats.RequestCurrentStats();
            }
            this.Load();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            SteamUserStats.StoreStats();
        }

        [ContextMenu("Unlock")]
        public void Load()
        {
            if (!SteamUserStats.GetAchievement(this._key, out this._done))
            {
                Debug.LogError("Failed to load achievement");
            }
        }

        [ContextMenu("Unlock")]
        public void Unlock()
        {
            if (SteamUserStats.SetAchievement(this._key))
            {
                this._done = true;
                if (this._autoSave)
                {
                    SteamUserStats.StoreStats();
                }
            }
            else
            {
                Debug.LogError("Failed to set achievement");
            }
        }

        [ContextMenu("Reset")]
        public void ResetAchievement()
        {
            SteamUserStats.ClearAchievement(this._key);
        }
    }
}
