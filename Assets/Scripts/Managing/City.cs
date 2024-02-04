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
    private readonly float dayTime = 24;
    private float minutes;
    private float speedFactorTemp;

    [Header("City info")]
    [SerializeField]
    private int mana;
    [SerializeField]
    private int day;
    [SerializeField]
    private int curElfs;
    [SerializeField]
    private int curFairies;
    [SerializeField]
    private int curFood;
    [SerializeField]
    private int maxElfs;
    [SerializeField]
    private int maxFairies;
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
    }

    public void OnBeginNewDay() => CalculateStats();

    private void CalculateStats()
    {
        day++;
        curDayTime = 0;

        foreach (Building building in buildings)
        {
            mana -= building.Preset.CostPerTurn;
            lifeCellsAvailable += building.Preset.LifeCells;
            if (lifeCells.Find(x => x.transform.position == building.transform.position) != null)
            {
                maxElfs += building.Preset.Elf;
                maxFairies += building.Preset.Fairy;
                curFood += building.Preset.Food;
            }
            else if (lifeCells.Find(x => x.transform.position == building.transform.position) == null)
            {
                maxElfs -= building.Preset.Elf;
                maxFairies -= building.Preset.Fairy;
                curElfs = maxElfs < curElfs ? maxElfs : curElfs;
                curFairies = maxFairies < curFairies ? maxFairies : curFairies;
            }
        }

        mana += curFairies * incomePerJob;

        if (curFood >= curElfs && curElfs < maxElfs)
        {
            curFood -= curElfs / 4;
            curElfs = Mathf.Min(curElfs + (curFood / 4), maxElfs);
        }

        else if (curFood <= curElfs)
            curElfs = curFood;

        curFairies = Mathf.Min(curElfs, maxFairies);

        UpdateStats();
    }

    public void OnPlaceBuilding(Building building)
    {
        if (mana >= building.Preset.Cost)
        {
            mana -= building.Preset.Cost;
            maxElfs += building.Preset.Elf;
            maxFairies += building.Preset.Fairy;

            buildings.Add(building);

            UpdateStats();
        }
        
    }

    public void OnPlaceLifeCell(Building building)
    {
        lifeCellsAvailable--;

        lifeCells.Add(building);

        UpdateStats();

        GameOfLifeController.Instance.AddNewLifeCell(building.gameObject);
    }

    public void OnInicializeLifeCell(Building building) => lifeCells.Add(building);
    public void OnUpdateLifeCell(Building building) => lifeCells.Remove(building);

    public void OnRemoveBuilding(Building building)
    {
        mana += building.Preset.Cost / 2;
        maxElfs -= building.Preset.Elf;
        maxFairies -= building.Preset.Fairy;

        buildings.Remove(building);

        Destroy(building.gameObject);

        UpdateStats();
    }

    private void UpdateStats() => statsText.text = string.Format("Day: {0}\t\tMana: {1}\nElfs: {2}/{3}\tFairies: {4}/{5}\nFood: {6}\t\tLife Cells: {7}", new object[] { day, mana, curElfs, maxElfs, curFairies, maxFairies, curFood, lifeCellsAvailable });

    public void ModifyDaySpeed(int factor) => speedFactor = factor;

    public void ChangeColor(GameObject button)
    {
        tempButton.GetComponent<Image>().color = Color.white;
        button.GetComponent<Image>().color = buttonColor;
        tempButton = button;
    }
}
