using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abraham.GalacticConquest.UnitControl
{
    public class RandomAppearanceController : MonoBehaviour
    {
        public List<GameObject> possibleSkins = new List<GameObject>();
        public List<Material> possibleMaterials = new List<Material>();

        private GameObject activeSkin;

        void Start()
        {
            ActivateRandomSkin();
            ApplyRandomMaterial();
        }

        void ApplyRandomMaterial()
        {
            if (possibleMaterials.Count == 0) {
                return;
            }

            MeshRenderer meshRenderer = activeSkin.GetComponent<MeshRenderer>();
            if (meshRenderer == null) {
                Debug.LogError("ERROR RandomAppearanceController ApplyRandomMaterial(): The active skin is missing a Mesh Renderer component.", this);
                return;
            }

            Material chosenMaterial = ChooseRandomMaterial();
            if (chosenMaterial == null) {
                Debug.LogError("ERROR RandomAppearanceController ApplyRandomMaterial(): Chosen Material is null.", this);
                return;
            }
            meshRenderer.material = chosenMaterial;
        }

        void ActivateRandomSkin()
        {
            //Deactivate All skins
            foreach (GameObject thisSkin in possibleSkins) {
                thisSkin.SetActive(false);
            }

            //Activate 1 random skin
            GameObject chosenSkin = ChooseRandomSkin();
            if (chosenSkin == null) {
                Debug.LogError("ERROR RandomAppearanceController ActivateRandomSkin(): Chosen Skin was null.", this);
                return;
            }
            chosenSkin.SetActive(true);
            activeSkin = chosenSkin;
        }

        GameObject ChooseRandomSkin()
        {
            if (possibleSkins.Count == 0) {
                return null;
            }

            int randomIndex = Random.Range(0, possibleSkins.Count);
            return possibleSkins[randomIndex];
        }

        Material ChooseRandomMaterial()
        {
            if (possibleMaterials.Count == 0) {
                return null;
            }

            int randomIndex = Random.Range(0, possibleMaterials.Count);
            return possibleMaterials[randomIndex];
        }

    }
}
