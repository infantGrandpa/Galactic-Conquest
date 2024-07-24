using UnityEngine;
using System.Collections.Generic;

namespace Abraham.GalacticConquest
{
    public class MovementManager : MonoBehaviour
    {
        //Singleton
        public static MovementManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType(typeof(MovementManager)) as MovementManager;

                return instance;
            }
            set
            {
                instance = value;
            }
        }
        private static MovementManager instance;

        public List<PlanetBehaviour> planets = new();

        public void MoveToPlanet()
        {
            //Cancel if nothing is selected
            List<Selectable> objectsToMove = new(SelectionManager.Instance.selectedObjects);
            if (objectsToMove.Count == 0)
            {
                return;
            }

            //Get Move To Target
            RaycastHit? nullableHitInfo = InputManager.Instance.SphereCastFromCameraToCursor();

            if (nullableHitInfo == null)
            {
                return;
            }

            //Convert so we can get the transform of the hit object
            RaycastHit hitInfo = (RaycastHit)nullableHitInfo;

            //Get Selectable
            PlanetBehaviour targetPlanet = hitInfo.transform.GetComponentInParent<PlanetBehaviour>();
            if (!targetPlanet)
            {
                return;
            }

            //Send Move Command to selected objects
            foreach (Selectable thisObjectToMove in objectsToMove)
            {
                if (thisObjectToMove.TryGetComponent(out Moveable moveableObject))
                {
                    moveableObject.MoveToPlanet(targetPlanet);
                }
            }

        }
    }
}