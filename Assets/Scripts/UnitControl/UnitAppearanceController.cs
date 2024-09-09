using System;
using System.Collections.Generic;
using Abraham.GalacticConquest.Factions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Abraham.GalacticConquest.UnitControl
{
    public class UnitAppearanceController : SerializedMonoBehaviour
    {
        [OdinSerialize] public Dictionary<Faction, GameObject> FactionSkins = new Dictionary<Faction, GameObject>();

        void Start()
        {
            Faction myFaction = GetFactionFromParent();
            GameObject factionSkin = GetSkinFromFaction(myFaction);
            ActivateSkin(factionSkin);
        }

        private Faction GetFactionFromParent()
        {
            FactionHandler factionHandler = GetComponentInParent<FactionHandler>();
            if (factionHandler == null) {
                Debug.LogError("ERROR UnitAppearanceController GetFactionFromParent(): Parent is missing faction handler component.", this);
                return null;
            }

            Faction faction = factionHandler.myFaction;
            return faction;
        }

        private GameObject GetSkinFromFaction(Faction faction)
        {
            return FactionSkins.GetValueOrDefault(faction);
        }

        private void ActivateSkin(GameObject skinToActivate)
        {
            foreach (GameObject skin in FactionSkins.Values) {
                skin.SetActive(false);
            }

            skinToActivate.SetActive(true);
        }
    }
}
