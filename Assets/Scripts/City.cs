using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class City : MonoBehaviour
{
    [Header("City info")]
    [SerializeField]
    private int money;
    [SerializeField]
    private int day;
    [SerializeField]
    private int curPopulation;
    [SerializeField]
    private int curJobs;
    [SerializeField]
    private int curFood;
    [SerializeField]
    private int maxPopulation;
    [SerializeField]
    private int maxJob;
    [SerializeField]
    private int incomePerJob;
    [Header("Other info")]
    [SerializeField]
    private TextMeshProUGUI statsText;
    [SerializeField]
    private List<Building> buildings = new();

    public static City Instance { get; private set; }
    public List<Building> Buildings { get => buildings; set => buildings = value; }

    private void Awake()
    {
        Instance = this;

        statsText.text = string.Format("Day: {0}\nMoney: {1}\nPopulation: {2}/{3}\nJob: {4}/{5}\nFood: {6}", new object[] { day, money, curPopulation, maxPopulation, curJobs, maxJob, curFood });
    }

    public void OnPlaceBuilding(Building building)
    {
        money -= building.Preset.Cost;
        maxPopulation += building.Preset.Population;
        maxJob += building.Preset.Jobs;

        buildings.Add(building);

        UpdateStats();
    }

    public void OnPlaceLiveCell(Building building)
    {
        money -= building.Preset.Cost;
        maxPopulation += building.Preset.Population;
        maxJob += building.Preset.Jobs;

        buildings.Add(building);

        UpdateStats();
    }

    public void OnRemoveBuilding(Building building)
    {
        money += building.Preset.Cost;
        maxPopulation -= building.Preset.Population;
        maxJob -= building.Preset.Jobs;

        buildings.Remove(building);

        Destroy(building.gameObject);

        UpdateStats();
    }

    private void UpdateStats()
    {
        statsText.text = string.Format("Day: {0}\nMoney: {1}\nPopulation: {2}/{3}\nJob: {4}/{5}\nFood: {6}", new object[] { day, money, curPopulation, maxPopulation, curJobs, maxJob, curFood });
    }
}
