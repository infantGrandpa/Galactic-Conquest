using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(LevelManager)) as LevelManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static LevelManager instance;

    public Transform DynamicTransform { get; private set; }
    public Camera MainCamera { get; private set; }

    private void Awake() {
        CreateDynamicTransform();

        MainCamera = Camera.main;
    }

    private void CreateDynamicTransform()
    {
        GameObject dynamicGameObject = new() { name = "_Dynamic" };
        DynamicTransform = dynamicGameObject.transform;
    }
}
