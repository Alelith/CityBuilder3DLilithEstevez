using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    [Header("Time")]
    [SerializeField]
    private float curDayTime;
    [SerializeField] 
    private float speedFactor;
    [SerializeField]
    private TextMeshProUGUI timeText;
    private float dayTime = 24;
    private float minutes;
    private float speedFactorTemp;

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
    [SerializeField]
    private int lifeCellsAvailable;
    [Header("Other info")]
    [SerializeField]
    private TextMeshProUGUI statsText;
    [SerializeField]
    private List<Building> buildings = new();
    [SerializeField]
    private List<Building> lifeCells = new();

    [Header("Buttons")]
    [SerializeField]
    private GameObject tempButton;
    [SerializeField]
    private Color buttonColor;

    [Header("Lighting")]
    [SerializeField]
    private Light sun;

    public static City Instance { get; private set; }
    public List<Building> Buildings { get => buildings; private set => buildings = value; }
    public List<Building> LifeCells { get => lifeCells; private set => lifeCells = value; }
    public float SpeedFactor { get => speedFactor; set => speedFactor = value; }

    private void Awake()
    {
        Instance = this;

        UpdateStats();
    }

    private void FixedUpdate()
    {
        DayCicle();
    }

    private void DayCicle()
    {
        curDayTime += Time.deltaTime * speedFactor;

        if (curDayTime >= dayTime)
            CalculateStats();

        int hour = (int) curDayTime;
        minutes += speedFactor * Time.deltaTime * 60;
        int minutesint = (int) minutes;

        if (minutes > 60)
            minutes = 0;

        timeText.text = hour.ToString("00") + ":" + minutesint.ToString("00");

        sun.transform.rotation = Quaternion.Euler(((curDayTime - 7) / dayTime) * 360, 0, 0);

        RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + Time.deltaTime * (2 * speedFactor));
    }

    public void OnBeginNewDay() => CalculateStats();

    private void CalculateStats()
    {
        day++;
        curDayTime = 0;

        foreach (Building building in buildings)
        {
            money -= building.Preset.CostPerTurn;
            lifeCellsAvailable += building.Preset.LifeCells;
            if (lifeCells.Find(x => x.transform.position == building.transform.position) != null)
            {
                maxPopulation += building.Preset.Population;
                maxJob += building.Preset.Jobs;
                curFood += building.Preset.Food;
            }
            else if (lifeCells.Find(x => x.transform.position == building.transform.position) == null)
            {
                maxPopulation -= building.Preset.Population;
                maxJob -= building.Preset.Jobs;
                curPopulation = maxPopulation < curPopulation ? maxPopulation : curPopulation;
                curJobs = maxJob < curJobs ? maxJob : curJobs;
            }
        }

        money += curJobs * incomePerJob;

        if (curFood >= curPopulation && curPopulation < maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        }

        else if (curFood <= curPopulation)
            curPopulation = curFood;

        curJobs = Mathf.Min(curPopulation, maxJob);

        UpdateStats();
    }

    public void OnPlaceBuilding(Building building)
    {
        money -= building.Preset.Cost;
        maxPopulation += building.Preset.Population;
        maxJob += building.Preset.Jobs;

        buildings.Add(building);

        UpdateStats();
    }

    public void OnPlaceLifeCell(Building building)
    {
        money -= building.Preset.Cost;

        lifeCells.Add(building);

        UpdateStats();

        GameOfLifeController.Instance.AddNewLifeCell(building.gameObject);
    }

    public void OnInicializeLifeCell(Building building) => lifeCells.Add(building);
    public void OnUpdateLifeCell(Building building) => lifeCells.Remove(building);

    public void OnRemoveBuilding(Building building)
    {
        money += building.Preset.Cost;
        maxPopulation -= building.Preset.Population;
        maxJob -= building.Preset.Jobs;

        buildings.Remove(building);

        Destroy(building.gameObject);

        UpdateStats();
    }

    private void UpdateStats() => statsText.text = string.Format("Day: {0}\t\tMoney: {1}\nPopulation: {2}/{3}\tJob: {4}/{5}\nFood: {6}\t\tLife Cells: {7}", new object[] { day, money, curPopulation, maxPopulation, curJobs, maxJob, curFood, lifeCellsAvailable });

    public void ModifyDaySpeed(int factor) => speedFactor = factor;

    public void ChangeColor(GameObject button)
    {
        tempButton.GetComponent<Image>().color = Color.white;
        button.GetComponent<Image>().color = buttonColor;
        tempButton = button;
    }
}
