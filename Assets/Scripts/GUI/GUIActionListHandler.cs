using System;
using Abraham.GalacticConquest.Planets;
using TMPro;
using UnityEngine;

namespace Abraham.GalacticConquest.GUI
{
    public class GUIActionListHandler : MonoBehaviour
    {
        [SerializeField] TMP_Text header;

        void Awake()
        {
            HideActionList();
        }

        public void ShowActionList(PlanetBehaviour planet)
        {
            gameObject.SetActive(true);
            header.text = planet.planet.planetName;

        }

        public void HideActionList()
        {
            gameObject.SetActive(false);
        }
    }
}
