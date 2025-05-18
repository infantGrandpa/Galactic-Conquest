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
            [FormerlySerializedAs("apAdjustValue")] public int apModificationValue;
            [FormerlySerializedAs("adjustReason")] public string apModificationReason;

            public APModifier(int apModificationValue, string apModificationReason)
            {
                this.apModificationValue = apModificationValue;
                this.apModificationReason = apModificationReason;
            }
        }

        public int baseApPerTurn = 0;

        [ShowInInspector, ReadOnly] public int TotalApPerTurn { get; private set; }

        [FormerlySerializedAs("apAdjustments")] [SerializeField] List<APModifier> apModifiers = new();

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

        public void AddApModifier(int adjustBy, string reason)
        {
            APModifier newModifier = new(adjustBy, reason);

            apModifiers.Add(newModifier);

            CalculateAp();
        }
    }
}