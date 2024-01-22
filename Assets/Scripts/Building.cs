using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField]
    private BuildingPreset preset;

    public BuildingPreset Preset { get => preset; set => preset = value; }
}
