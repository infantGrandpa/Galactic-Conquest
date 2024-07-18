using UnityEngine;
using System.Collections.Generic;

public class PlanetBehaviour : MonoBehaviour
{
    public Transform fleetSlotTransform;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(fleetSlotTransform.position, 0.25f);
    }
}
