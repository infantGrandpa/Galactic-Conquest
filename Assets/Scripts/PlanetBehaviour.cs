using UnityEngine;
using System.Collections.Generic;

public class PlanetBehaviour : MonoBehaviour
{
    public Transform fleetSlotTransform;

    private void OnEnable()
    {
        MovementManager.Instance.planets.Add(this);
    }

    private void OnDisable()
    {
        if (MovementManager.Instance == null)
        {
            return;
        }

        MovementManager.Instance.planets.Remove(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(fleetSlotTransform.position, 0.25f);
    }
}
