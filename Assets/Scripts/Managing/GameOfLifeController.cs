using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLifeController : MonoBehaviour
{
    [SerializeField]
    private int width = 10;
    [SerializeField]
    private int height = 10;
    [SerializeField]
    private float cellSize = 1f;
    [SerializeField]
    private float cellOffset = 0.5f;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private float updateDelay = 1f;

    private float time;

    private Dictionary<Vector3, bool> cells = new();
    private readonly Dictionary<Vector3, GameObject> activeCells = new();

    public GameOfLifeController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitializeCells();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= updateDelay) 
        {
            UpdateCells();
            time = 0f;
        }
    }

    private void InitializeCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 position = new((x * cellSize) + cellOffset, 0.001f, (z * cellSize) + cellOffset);
                bool isAlive = Random.Range(0, 2) == 0;
                cells.Add(position, isAlive);

                GameObject cell = isAlive ? Instantiate(cellPrefab, position, Quaternion.identity) : null;
                if (cell != null)
                    activeCells.Add(cell.transform.position, cell);
            }
        }
    }

    private void UpdateCells()
    {
        Dictionary<Vector3, bool> newCells = new();

        foreach (KeyValuePair<Vector3, bool> cell in cells)
        {
            Vector3 position = cell.Key;
            bool isAlive = cell.Value;

            int aliveNeighbors = CountAliveNeighbors(position);

            if (isAlive && (aliveNeighbors < 2 || aliveNeighbors > 3))
            {
                isAlive = false;
            }
            else if (!isAlive && aliveNeighbors == 3)
            {
                isAlive = true;
            }

            newCells.Add(position, isAlive);

            if (!activeCells.ContainsKey(position) && isAlive)
            {
                GameObject cellInstance = Instantiate(cellPrefab, position, Quaternion.identity);
                activeCells.Add(position, cellInstance);
            } 
            else if (activeCells.ContainsKey(position) && !isAlive) 
            {
                Destroy(activeCells[position]);
                activeCells.Remove(position);
            }
        }

        cells = newCells;
    }

    private int CountAliveNeighbors(Vector3 position)
    {
        int count = 0;

        for (float x = position.x - 1; x <= position.x + 1; x += 1)
        {
            for (float z = position.z - 1; z <= position.z + 1; z += 1)
            {
                if (x == position.x && z == position.z)
                {
                    continue;
                }

                Vector3 neighborPosition = new(x, 0.001f, z);

                if (cells.ContainsKey(neighborPosition) && cells[neighborPosition])
                {
                    count++;
                }
            }
        }

        return count;
    }
}
