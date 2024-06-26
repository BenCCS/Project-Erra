using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Steamworks;

namespace MT.HexTiles.Data
{
    //[CreateAssetMenu(menuName = "Steam/DLC", order = 2)]
    public class DlcVar : MonoBehaviour
    {
        [SerializeField] public AppId_t _appID;
        [SerializeField] bool _owned;
        
        public bool Owned { get { return (this._owned); } }

        [ContextMenu("Initialize")]

        public void OnEnable()
        {
            this.Load();
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if (!this._owned)
            {
                this._owned = SteamApps.BIsDlcInstalled(this._appID);
            }
        }
    }
}
