using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class ActionPointModifier : MonoBehaviour
    {
        [Serializable]
        class APModifier
        {
            public string apModificationSource;
            [FormerlySerializedAs("adjustReason")] public string apModificationReason;

            [FormerlySerializedAs("apAdjustValue")]
            public int apModificationValue;

            public APModifier(string apModificationSource, string apModificationReason, int apModificationValue)
            {
                this.apModificationSource = apModificationSource;
                this.apModificationReason = apModificationReason;
                this.apModificationValue = apModificationValue;
            }
            
            public override string ToString()
            {
                string apString = apModificationValue < 0 ? apModificationValue.ToString() : "+" + apModificationValue; 
                return $"{apString} AP for {apModificationReason} at {apModificationSource}.";
            }
        }

        public int baseApPerTurn = 0;

        [ShowInInspector, ReadOnly] public int TotalApPerTurn { get; private set; }

        [FormerlySerializedAs("apAdjustments")] [SerializeField]
        List<APModifier> apModifiers = new();

        void OnEnable()
        {
            ActionPointManager.Instance.actionPointModifiers.Add(this);
        }

        void OnDisable()
        {
            if (ActionPointManager.Instance == null)
            {
                return;
            }

            ActionPointManager.Instance.actionPointModifiers.Remove(this);
        }

        void Start()
        {
            // We do this in Start() instead of Awake() because we need to wait for TraitHandlers to apply each Trait
            CalculateAp();
        }

        protected virtual void CalculateAp()
        {
            int apThisTurn = baseApPerTurn;

            foreach (APModifier modifier in apModifiers)
            {
                apThisTurn += modifier.apModificationValue;
            }

            TotalApPerTurn = apThisTurn;
        }

        public void AddApModifier(string reason, int modifyValue)
        {
            GenericInfo info = GetComponent<GenericInfo>();
            string source = info ? info.myName : "Unknown";

            APModifier newModifier = new(source, reason, modifyValue);
            apModifiers.Add(newModifier);
            
            /* We used to call CalculateAp() here, but that meant that on Start(), we were calculating AP repeatedly for no reason.
                As of right now, AP modifiers are only added on Awake, so we don't need to calculate AP each time.
                If that changes, feel free to add CalculateAp() back in here. 
                The performance hit probably won't be too bad, but definitely keep it in mind.
            */ 
        }
    }
}