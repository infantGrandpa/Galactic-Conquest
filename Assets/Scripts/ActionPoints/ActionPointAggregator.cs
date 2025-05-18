using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class ActionPointAggregator : MonoBehaviour
    {

        public int baseApPerTurn = 0;

        [ShowInInspector, ReadOnly] public int TotalApPerTurn { get; private set; }

        [FormerlySerializedAs("apAdjustments")] [SerializeField]
        List<ActionPointModifier> apModifiers = new();

        void OnEnable()
        {
            ActionPointManager.Instance.actionPointAggregators.Add(this);
        }

        void OnDisable()
        {
            if (ActionPointManager.Instance == null)
            {
                return;
            }

            ActionPointManager.Instance.actionPointAggregators.Remove(this);
        }

        void Start()
        {
            // We do this in Start() instead of Awake() because we need to wait for TraitHandlers to apply each Trait
            CalculateAp();
        }

        protected virtual void CalculateAp()
        {
            int apThisTurn = baseApPerTurn;

            foreach (ActionPointModifier modifier in apModifiers)
            {
                apThisTurn += modifier.apModificationValue;
            }

            TotalApPerTurn = apThisTurn;
        }

        public void AddApModifier(string reason, int modifyValue)
        {
            ActionPointModifier newModifier = new(reason, modifyValue);
            apModifiers.Add(newModifier);
            
            /* We used to call CalculateAp() here, but that meant that on Start(), we were calculating AP repeatedly for no reason.
                As of right now, AP modifiers are only added on Awake, so we don't need to calculate AP each time.
                If that changes, feel free to add CalculateAp() back in here. 
                The performance hit probably won't be too bad, but definitely keep it in mind.
            */ 
        }
    }
}