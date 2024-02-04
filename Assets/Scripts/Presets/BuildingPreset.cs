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
    private int elf;
    [SerializeField]
    private int fairy;
    [SerializeField]
    private int food;
    [SerializeField]
    private int lifeCells;
    [Header("Prefab")]
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private BuildingType type;

    public int Cost { get => cost; set => cost = value; }
    public int CostPerTurn { get => costPerTurn; set => costPerTurn = value; }
    public int Elf { get => elf; set => elf = value; }
    public int Fairy { get => fairy; set => fairy = value; }
    public int Food { get => food; set => food = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }
    public BuildingType Type { get => type; set => type = value; }
    public int LifeCells { get => lifeCells; set => lifeCells = value; }
}

public enum BuildingType
{
    LifeCell,
    Building
}
