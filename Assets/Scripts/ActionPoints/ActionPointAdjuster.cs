using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Abraham.GalacticConquest.ActionPoints
{
    public class ActionPointAdjuster : MonoBehaviour
    {
        [System.Serializable]
        private class APAdjustment
        {
            public int apAdjustValue;
            public string adjustReason;

            public APAdjustment(int apAdjustValue, string adjustReason)
            {
                this.apAdjustValue = apAdjustValue;
                this.adjustReason = adjustReason;
            }
        }

        public int baseApPerTurn = 0;

        [ShowInInspector, ReadOnly] public int TotalApPerTurn { get; private set; }

        [SerializeField] List<APAdjustment> apAdjustments = new List<APAdjustment>();

        void OnEnable()
        {
            ActionPointManager.Instance.actionPointAdjusters.Add(this);
        }

        void OnDisable()
        {
            if (ActionPointManager.Instance == null) {
                return;
            }
            ActionPointManager.Instance.actionPointAdjusters.Remove(this);
        }

        void Start()
        {
            CalculateAp();
        }

        private void CalculateAp()
        {
            int apThisTurn = baseApPerTurn;

            foreach (APAdjustment adjustment in apAdjustments) {
                apThisTurn += adjustment.apAdjustValue;
            }

            TotalApPerTurn = apThisTurn;
        }

        public void AddApAdjustment(int adjustBy, string reason)
        {
            APAdjustment newAdjustment = new APAdjustment(adjustBy, reason);

            apAdjustments.Add(newAdjustment);

            CalculateAp();
        }
    }
}
