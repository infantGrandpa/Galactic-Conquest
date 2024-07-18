using DG.Tweening;
using UnityEngine;


public class FleetBehaviour : MonoBehaviour
{
    public PlanetBehaviour currentPlanet;

    [SerializeField] float lookTweenDuration = 0.5f;
    [SerializeField] float moveTweenDuration = 1f;

    public void MoveFleetToPlanetPosition(PlanetBehaviour targetPlanet)
    {
        currentPlanet = targetPlanet;
        Transform targetTransform = targetPlanet.transform;

        Sequence moveSequence = DOTween.Sequence();

        moveSequence.Append(transform.DOLookAt(targetTransform.position, lookTweenDuration));
        moveSequence.Append(transform.DOMove(targetPlanet.transform.position, moveTweenDuration, false).SetEase(Ease.InOutExpo));
    }
}
