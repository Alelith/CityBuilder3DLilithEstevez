using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building Preset", menuName = ("City Builder/New Building"))]
public class BuildingPreset : ScriptableObject
{
    [Header("Costs")]
    [SerializeField]
    private int cost;
    [SerializeField]
    private int costPerTurn;
    [Header("Generation")]
    [SerializeField]
    private int population;
    [SerializeField]
    private int jobs;
    [SerializeField]
    private int food;
    [Header("Prefab")]
    [SerializeField]
    private GameObject prefab;

    public int Cost { get => cost; set => cost = value; }
    public int CostPerTurn { get => costPerTurn; set => costPerTurn = value; }
    public int Population { get => population; set => population = value; }
    public int Jobs { get => jobs; set => jobs = value; }
    public int Food { get => food; set => food = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
}
