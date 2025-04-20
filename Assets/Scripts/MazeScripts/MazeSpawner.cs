using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;
using TMPro;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject mazeContainer;

    [Header("PREFABS")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject wallPrefab;

    [Header("DIMENSIONS")]
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 5;
    [SerializeField] private float cellWidth = 5;
    [SerializeField] private float cellHeight = 5;

    [Header("PROPERTIES")]
    [SerializeField] private int numberOfTries;
    [SerializeField] private bool IsWallThick;

    [Header("REWARDS")]
    [SerializeField] private bool FixRewards;
    [SerializeField] private int numberRewards;
    private int collectedRewards;
    [SerializeField] private GameObject rewardPrefab;

    [Header("TIMER")]
    [SerializeField] private GameObject mazeCanvas;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float timerMaze;
    private float timer;
    [SerializeField] private bool MazeRunning;

    private MazeGenerator mazeGenerator = null;
    private Vector3 destination;

    public int CollectedRewards { get => collectedRewards; set => collectedRewards = value; }

    // Start is called before the first frame update
    void Start()
    {
        timer = timerMaze;
        mazeGenerator = gameObject.AddComponent<MazeGenerator>();
        destination = gameObject.transform.GetChild(1).position;

        ChooseMazeDistribution();

        BuildMaze();
    }

    private void Update()
    {
        if (MazeRunning)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay(timer);
            if (timer <= 0)
            {
                RunOutOfTime();
            }
        }
    }

    private void UpdateTimerDisplay(float time)
    { 
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        // Formato mm:ss
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartMaze()
    {
        if(!MazeRunning) collectedRewards = 0;
        mazeCanvas.SetActive(true);
        MazeRunning = true;
    }

    public void EndMaze(bool activatedByPlayer)
    {
        if (MazeRunning)
        {
            if (activatedByPlayer && collectedRewards.Equals(MazeGenerator.rewardsAmount))
            {
                FindAnyObjectByType<GameManager>().Minigame2_Flag = true;
            }
            ChooseMazeDistribution();
            BuildMaze();
        }
        mazeCanvas.SetActive(false);
        MazeRunning = false;
        timer = timerMaze;
    }

    private void RunOutOfTime()
    {
        CharacterController controller = FindFirstObjectByType<PlayerBehaviour>().GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false; // Desactivar para evitar conflictos
            FindFirstObjectByType<PlayerBehaviour>().transform.position = destination;
            controller.enabled = true; // Volver a activar
        }

        EndMaze(false);

        if (FindFirstObjectByType<PlayerBehaviour>().TryGetComponent(out Damageable damageSystem))
        {
            damageSystem.DamageTarget(20f);
        }
    }

    private void ChooseMazeDistribution()
    {
        mazeGenerator.MazeGeneratorInitialize(rows, columns);
        mazeGenerator.SetNumberOfTries(numberOfTries);
        mazeGenerator.GenerateMaze(IsWallThick, FixRewards, numberRewards);

        if (mazeGenerator.MazeCells == null)
        {
            return;
        }
    }

    private void BuildMaze()
    {
        ClearMaze(mazeContainer);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                float x = column * (cellWidth);
                float z = row * (cellHeight);
                MazeCellScript cell = mazeGenerator.GetMazeCell(row, column);
                GameObject tmp = Instantiate(floorPrefab, new Vector3(x, 0, z) + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = mazeContainer.transform;

                if (row == 0 && column == 0)
                {
                    cell.WallDown = false;
                }

                if (cell.WallRight)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x + cellWidth / 2, 0, z) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = mazeContainer.transform;
                }
                if (cell.WallTop)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 0, z + cellHeight / 2) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = mazeContainer.transform;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x - cellWidth / 2, 0, z) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = mazeContainer.transform;
                }
                if (cell.WallDown)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 0, z - cellHeight / 2) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = mazeContainer.transform;
                }
                if (cell.HasReward && rewardPrefab != null)
                {
                    tmp = Instantiate(rewardPrefab, new Vector3(x, 1, z) + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = mazeContainer.transform;
                }
            }
        }
    }

    private void ClearMaze(GameObject container)
    {
        foreach(Transform t in container.transform)
        {
            Destroy(t.gameObject);
        }
    }
}