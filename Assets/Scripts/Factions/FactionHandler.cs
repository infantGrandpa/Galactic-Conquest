using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FactionHandler : MonoBehaviour
{
    public Faction myFaction;

    [SerializeField] List<Renderer> renderersToChangeOnSetFaction = new();
    [SerializeField] List<Image> uiImagesToChangeOnSetFaction = new();

    private void Start()
    {
        SetFaction(myFaction);
    }

    public void SetFaction(Faction newFaction)
    {
        myFaction = newFaction;
        foreach (Renderer thisRenderer in renderersToChangeOnSetFaction)
        {
            thisRenderer.material.color = myFaction.FactionColor;
        }

        foreach (Image thisImage in uiImagesToChangeOnSetFaction)
        {
            thisImage.color = myFaction.FactionColor;
        }

        Debug.Log(gameObject.name + ": Set Faction to " + myFaction.name);
    }
}
