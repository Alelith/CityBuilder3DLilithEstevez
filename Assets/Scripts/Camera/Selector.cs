using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    private Camera cam;

    public static Selector Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Get the tile that the mouse is hovering over
    /// </summary>
    /// <returns>The position of the current tile</returns>
    public Vector3 GetCurrentTilePos()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return new Vector3(0, -99, 0);

        Plane plane = new(Vector3.up, Vector3.zero);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(ray, out float rayOut))
        {
            Vector3 newPos = ray.GetPoint(rayOut);

            return new Vector3(Mathf.CeilToInt(newPos.x) - 0.5f, 0, Mathf.CeilToInt(newPos.z) - 0.5f);
        }

        return new Vector3(0, -99, 0);
    }
}
