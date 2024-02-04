using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField]
    private GameObject placementIndicator;
    [SerializeField]
    private GameObject bulldozerIndicator;
    
    private bool isPlacing;
    private bool isBulldozering;

    private BuildingPreset curPreset;
    private readonly float indicatorUpdateRate = 0.05f;

    private float lastUpdateTime;

    private float tempSpeedFactor;

    private PlayerInputActions action;

    private Vector3 curIndicatorPos;
    private Vector3 originalScale;
    private Mesh originalMesh;

    private void Awake()
    {
        action = new();

        action.Player.Cancel.performed += e => OnCancelPlacement();

        action.Player.Rotate.performed += e => OnRotateBuilding();

        action.Player.Place.performed += e => OnPlaceBuilding();

        action.Enable();

        originalMesh = bulldozerIndicator.GetComponentInChildren<MeshFilter>().mesh;
        originalScale = bulldozerIndicator.GetComponentInChildren<Transform>().localScale;
    }

    private void Update()
    {
        if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;

            curIndicatorPos = Selector.Instance.GetCurrentTilePos();

            if (isPlacing)
                placementIndicator.transform.position = curIndicatorPos;
            else if (isBulldozering)
            {
                bulldozerIndicator.transform.position = curIndicatorPos;
                Building hovered = City.Instance.Buildings.Find(x => x.transform.position == curIndicatorPos);
                if (hovered != null)
                {
                    bulldozerIndicator.GetComponentInChildren<MeshFilter>().mesh = hovered.GetComponentInChildren<MeshFilter>().sharedMesh;
                    bulldozerIndicator.GetComponentInChildren<Transform>().localScale = hovered.transform.GetChild(0).localScale * 1.01f;
                }
                else
                {
                    bulldozerIndicator.GetComponentInChildren<MeshFilter>().mesh = originalMesh;
                    bulldozerIndicator.GetComponentInChildren<Transform>().localScale = new(1, 1, 1);
                }
            }
        }
    }

    /// <summary>
    /// Triggered when a building button is pressed
    /// </summary>
    /// <param name="preset">The preset thet will be instantiated</param>
    public void OnBeginNewPlacement (BuildingPreset preset)
    {
        tempSpeedFactor = City.Instance.SpeedFactor;
        City.Instance.SpeedFactor = 0;

        if (isPlacing)
            OnCancelPlacement();
        else if (isBulldozering)
            OnToggleBullozer();
        isPlacing = true;

        curPreset = preset;
        placementIndicator.GetComponentInChildren<MeshFilter>().mesh = preset.Prefab.GetComponentInChildren<MeshFilter>().sharedMesh;
        placementIndicator.GetComponentInChildren<Transform>().localScale = preset.Prefab.transform.GetChild(0).localScale;
        placementIndicator.SetActive(true);

        placementIndicator.transform.position = new Vector3(0, -99, 0);
    }

    /// <summary>
    /// Cancel the placement selection
    /// </summary>
    private void OnCancelPlacement()
    {
        City.Instance.SpeedFactor = tempSpeedFactor;

        if (isPlacing)
        {
            isPlacing = false;

            curPreset = null;

            placementIndicator.SetActive(false);
        }
        else if (isBulldozering)
        {
            isBulldozering = false;
            bulldozerIndicator.SetActive(false);
        }
    }

    /// <summary>
    /// Changes the bulldozer state
    /// </summary>
    public void OnToggleBullozer()
    {
        if (isPlacing)
        {
            OnCancelPlacement();
        }
        isBulldozering = !isBulldozering;
        bulldozerIndicator.transform.position = isBulldozering ? new(0, -99, 0) : new Vector3(0, 0.5f, 0);
        bulldozerIndicator.SetActive(isBulldozering);
    }

    private void OnRotateBuilding()
    {
        if (isPlacing)
            placementIndicator.transform.eulerAngles += new Vector3(0, 90, 0);
    }

    private void OnPlaceBuilding()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Building existingBuilding = City.Instance.Buildings.Find(x => x.transform.position == curIndicatorPos);
            Building existingLifeCell = City.Instance.LifeCells.Find(x => x.transform.position == curIndicatorPos);
            if (isPlacing && placementIndicator.transform.position.y >= 0 && existingBuilding == null && curPreset.Type != BuildingType.LifeCell)
                City.Instance.OnPlaceBuilding(Instantiate(curPreset.Prefab, placementIndicator.transform.position, placementIndicator.transform.rotation).GetComponent<Building>());
            else if (isPlacing && placementIndicator.transform.position.y >= 0 && existingLifeCell == null && curPreset.Type == BuildingType.LifeCell)
                City.Instance.OnPlaceLifeCell(Instantiate(curPreset.Prefab, placementIndicator.transform.position, placementIndicator.transform.rotation).GetComponent<Building>());
            else if (isBulldozering && existingBuilding != null)
                City.Instance.OnRemoveBuilding(existingBuilding);
        }
    }
}
