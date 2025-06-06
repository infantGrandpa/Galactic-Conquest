using System.Collections.Generic;
using UnityEngine;

namespace Abraham.GalacticConquest
{
    public class GarbageCollector : MonoBehaviour
    {
        public static GarbageCollector Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType(typeof(GarbageCollector)) as GarbageCollector;

                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private static GarbageCollector _instance;
        
        private readonly List<GameObject> _objectsToDeleteAtEndOfTurn = new();

        public void AddToGarbageCollection(GameObject objectToDelete)
        {
            _objectsToDeleteAtEndOfTurn.Add(objectToDelete);
        }

        public void RemoveFromGarbageCollection(GameObject objectToRemove)
        {
            _objectsToDeleteAtEndOfTurn.Remove(objectToRemove);
        }

        public void ClearGarbage()
        {
            foreach (GameObject thisObject in _objectsToDeleteAtEndOfTurn)
            {
                if (!thisObject) continue;
                
                Destroy(thisObject);
            }
            
            _objectsToDeleteAtEndOfTurn.Clear();
        }
    }
}
