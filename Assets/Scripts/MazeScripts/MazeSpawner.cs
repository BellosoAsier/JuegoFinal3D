using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;
using UnityEngine.UIElements;

public class MazeSpawner : MonoBehaviour
{
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
    [SerializeField] private GameObject rewardPrefab;

    private MazeGenerator mazeGenerator = null;

    // Start is called before the first frame update
    void Start()
    {
        mazeGenerator = gameObject.AddComponent<MazeGenerator>();
        mazeGenerator.MazeGeneratorInitialize(rows, columns);
        mazeGenerator.SetNumberOfTries(numberOfTries);
        mazeGenerator.GenerateMaze(IsWallThick, FixRewards, numberRewards);

        if (mazeGenerator.MazeCells == null)
        {
            return;
        }
        
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                float x = column * (cellWidth);
                float z = row * (cellHeight);
                MazeCellScript cell = mazeGenerator.GetMazeCell(row, column);
                GameObject tmp = Instantiate(floorPrefab, new Vector3(x, 0, z) + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                
                if (row == 0 && column == 0)
                {
                    cell.WallDown = false;
                }

                if (cell.WallRight)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x + cellWidth / 2, 0, z) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallTop)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 0, z + cellHeight / 2) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x - cellWidth / 2, 0, z) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallDown)
                {
                    tmp = Instantiate(wallPrefab, new Vector3(x, 0, z - cellHeight / 2) + wallPrefab.transform.position + transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                }
                if (cell.HasReward && rewardPrefab != null)
                {
                    tmp = Instantiate(rewardPrefab, new Vector3(x, 1, z) + transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }

}