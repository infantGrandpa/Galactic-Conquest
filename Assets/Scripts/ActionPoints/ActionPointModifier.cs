using System;
using UnityEngine;

namespace Abraham.GalacticConquest.ActionPoints
{
    [Serializable]
    public class ActionPointModifier
    {
        
        public string apModificationReason;
        public int apModificationValue;

        public ActionPointModifier(string apModificationReason, int apModificationValue)
        {
            this.apModificationReason = apModificationReason;
            this.apModificationValue = apModificationValue;
        }
        
        public override string ToString()
        {
            string apString = apModificationValue < 0 ? apModificationValue.ToString() : "+" + apModificationValue; 
            return $"{apString} AP for {apModificationReason}.";
        }
    }
}
