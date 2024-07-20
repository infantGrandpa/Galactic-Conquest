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
    public Vector3 planetPlanePosition = new Vector3(0, 0, 0);

    private void Awake()
    {
        CreateDynamicTransform();

        MainCamera = Camera.main;
    }

    private void CreateDynamicTransform()
    {
        GameObject dynamicGameObject = new() { name = "_Dynamic" };
        DynamicTransform = dynamicGameObject.transform;
    }

    private void OnDrawGizmosSelected()
    {
        DrawPlaneGizmo();
    }

    private void DrawPlaneGizmo()
    {
        /*
        I copied this code from Stop the Sniper.
        I have no idea why the planeNormal matters.
        I'm keeping it because it works.
        */
        Gizmos.color = Color.blue;

        Vector3 planeNormal = Vector3.up;

        float planeDistance = 10f;
        float shortDistance = 0f;
        Vector3 cubeSizeVector;
        if (planeNormal == Vector3.up || planeNormal == Vector3.down)
        {
            cubeSizeVector = new Vector3(planeDistance, shortDistance, planeDistance);
        }
        else if (planeNormal == Vector3.right || planeNormal == Vector3.left)
        {
            cubeSizeVector = new Vector3(shortDistance, planeDistance, planeDistance);
        }
        else
        {
            cubeSizeVector = new Vector3(planeDistance, planeDistance, shortDistance);
        }

        Gizmos.DrawWireCube(transform.position, cubeSizeVector);
    }
}
