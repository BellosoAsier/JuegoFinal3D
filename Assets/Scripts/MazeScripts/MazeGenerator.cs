using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class MazeGenerator : MonoBehaviour
{
    private int mazeRowCount;
    private int mazeColumnCount;
    public int MazeRowCount { get { return mazeRowCount; } }
    public int MazeColumnCount { get { return mazeColumnCount; } }

    public MazeCellScript[,] MazeCells { get { return mazeCells; } }

    private MazeCellScript[,] mazeCells;

    private int numberOfTries;
    private int initialNumberOfTries;

    public struct CellToMove
    {
        public int _row;
        public int _column;
        public CellDirection _cellDirection;

        public CellToMove(int row, int column, CellDirection cellDirection)
        {
            _row = row;
            _column = column;
            _cellDirection = cellDirection;
        }
    }

    public void SetNumberOfTries(int x)
    {
        numberOfTries = x;
        initialNumberOfTries = x;
    }

    private int GetCellInRange(int max) 
    {
        return max;
    }

    private List<CellToMove> mazeCellsToMove = new List<CellToMove>();

    public void MazeGeneratorInitialize(int rows, int columns)
    {
        mazeRowCount = Mathf.Abs(rows);
        mazeColumnCount = Mathf.Abs(columns);

        if (mazeRowCount == 0) mazeRowCount = 1;
        if (mazeColumnCount == 0) mazeColumnCount = 1;

        mazeCells = new MazeCellScript[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                mazeCells[row, column] = new MazeCellScript();
            }
        }
    }

    public MazeCellScript GetMazeCell(int row, int column)
    {
        if (row >= 0 && column >= 0 && row < mazeRowCount && column < mazeColumnCount)
        {
            return mazeCells[row, column];
        }
        throw new System.ArgumentOutOfRangeException();
    }

    private void SetMazeCell(int row, int column, MazeCellScript cell)
    {
        if (row >= 0 && column >= 0 && row < mazeRowCount && column < mazeColumnCount)
        {
            mazeCells[row, column] = cell;
        }
        else
        {
            throw new System.ArgumentOutOfRangeException();
        }
    }

    public void GenerateMaze(bool wall, bool fixRewards, int numberRewards)
    {
        CellDirection[] movesAvailable = new CellDirection[4];
        int movesAvailableCount = 0;
        mazeCellsToMove.Clear();
        mazeCellsToMove.Add(new CellToMove(0, 0, CellDirection.Start));

        while (mazeCellsToMove.Count > 0)
        {
            movesAvailableCount = 0;
            CellToMove ctm = mazeCellsToMove[mazeCellsToMove.Count - 1];

            // Right
            if (ctm._column + 1 < MazeColumnCount && !GetMazeCell(ctm._row, ctm._column + 1).IsVisited && !IsCellInList(ctm._row, ctm._column + 1))
            {
                movesAvailable[movesAvailableCount++] = CellDirection.Right;
            }
            else if (!GetMazeCell(ctm._row, ctm._column).IsVisited && ctm._cellDirection != CellDirection.Left)
            {
                GetMazeCell(ctm._row, ctm._column).WallRight = true;
                if (ctm._column + 1 < MazeColumnCount) GetMazeCell(ctm._row, ctm._column + 1).WallLeft = true;
            }

            // Front
            if (ctm._row + 1 < MazeRowCount && !GetMazeCell(ctm._row + 1, ctm._column).IsVisited && !IsCellInList(ctm._row + 1, ctm._column))
            {
                movesAvailable[movesAvailableCount++] = CellDirection.Top;
            }
            else if (!GetMazeCell(ctm._row, ctm._column).IsVisited && ctm._cellDirection != CellDirection.Down)
            {
                GetMazeCell(ctm._row, ctm._column).WallTop = true;
                if (ctm._row + 1 < MazeRowCount) GetMazeCell(ctm._row + 1, ctm._column).WallDown = true;
            }

            // Left
            if (ctm._column - 1 >= 0 && !GetMazeCell(ctm._row, ctm._column - 1).IsVisited && !IsCellInList(ctm._row, ctm._column - 1))
            {
                movesAvailable[movesAvailableCount++] = CellDirection.Left;
            }
            else if (!GetMazeCell(ctm._row, ctm._column).IsVisited && ctm._cellDirection != CellDirection.Right)
            {
                GetMazeCell(ctm._row, ctm._column).WallLeft = true;
                if (ctm._column - 1 >= 0) GetMazeCell(ctm._row, ctm._column - 1).WallRight = true;
            }

            // Back
            if (ctm._row - 1 >= 0 && !GetMazeCell(ctm._row - 1, ctm._column).IsVisited && !IsCellInList(ctm._row - 1, ctm._column))
            {
                movesAvailable[movesAvailableCount++] = CellDirection.Down;
            }
            else if (!GetMazeCell(ctm._row, ctm._column).IsVisited && ctm._cellDirection != CellDirection.Top)
            {
                GetMazeCell(ctm._row, ctm._column).WallDown = true;
                if (ctm._row - 1 >= 0) GetMazeCell(ctm._row - 1, ctm._column).WallTop = true;
            }

            if (!GetMazeCell(ctm._row, ctm._column).IsVisited && movesAvailableCount == 0)
            {
                GetMazeCell(ctm._row, ctm._column).HasReward = true;
            }

            GetMazeCell(ctm._row, ctm._column).IsVisited = true;

            if (movesAvailableCount > 0)
            {
                switch (movesAvailable[Random.Range(0, movesAvailableCount)])
                {
                    case CellDirection.Right:
                        mazeCellsToMove.Add(new CellToMove(ctm._row, ctm._column + 1, CellDirection.Right));
                        break;
                    case CellDirection.Top:
                        mazeCellsToMove.Add(new CellToMove(ctm._row + 1, ctm._column, CellDirection.Top));
                        break;
                    case CellDirection.Left:
                        mazeCellsToMove.Add(new CellToMove(ctm._row, ctm._column - 1, CellDirection.Left));
                        break;
                    case CellDirection.Down:
                        mazeCellsToMove.Add(new CellToMove(ctm._row - 1, ctm._column, CellDirection.Down));
                        break;
                }
            }
            else
            {
                mazeCellsToMove.Remove(ctm);
            }
        }
        if (wall)
        {
            RemoveDuplicateWalls();
        }
        if (fixRewards)
        {
            FixNumberOfRewards(numberRewards, wall, fixRewards);
        }
    }
    private void RemoveDuplicateWalls()
    {
        for (int row = 0; row < MazeRowCount; row++)
        {
            for (int col = 0; col < MazeColumnCount; col++)
            {
                MazeCellScript current = GetMazeCell(row, col);

                // Comparar con la celda de la derecha
                if (col + 1 < MazeColumnCount)
                {
                    MazeCellScript right = GetMazeCell(row, col + 1);
                    if (current.WallRight && right.WallLeft)
                    {
                        // Eliminar la duplicada
                        right.WallLeft = false;
                        SetMazeCell(row, col + 1, right);
                    }
                }

                // Comparar con la celda de arriba
                if (row + 1 < MazeRowCount)
                {
                    MazeCellScript top = GetMazeCell(row + 1, col);
                    if (current.WallTop && top.WallDown)
                    {
                        // Eliminar la duplicada
                        top.WallDown = false;
                        SetMazeCell(row + 1, col, top);
                    }
                }
            }
        }
    }

    private void FixNumberOfRewards(int maxRewards, bool wall, bool fixRewards)
    {
        List<(int lRow, int lCols, MazeCellScript lCell)> newMazeCell = new List<(int, int, MazeCellScript)>();

        for (int row = 0; row < MazeRowCount; row++)
        {
            for (int col = 0; col < MazeColumnCount; col++)
            {
                MazeCellScript current = GetMazeCell(row, col);
                if (current.HasReward)
                {
                    newMazeCell.Add((row, col, current));
                }
            }
        }

        if (newMazeCell.Count <= maxRewards)
        {
            numberOfTries--;
            if (numberOfTries >= 0)
            {
                GenerateMaze(wall, fixRewards, maxRewards);
            }
            else
            {
                mazeCells = null;
                Debug.LogError($"After {initialNumberOfTries} tries, there are no possible maze distributions for {maxRewards} rewards. Try to change the number or dimensions of the maze.");
            }
            
        } 

        for (int i = 0; i < newMazeCell.Count; i++)
        {
            int randomIndex = Random.Range(i,newMazeCell.Count);
            (newMazeCell[i], newMazeCell[randomIndex]) = (newMazeCell[randomIndex], newMazeCell[i]);
        }

        for (int i = maxRewards; i < newMazeCell.Count; i++)
        {
            var (row, column, cell) = newMazeCell[i];
            cell.HasReward = false;
            SetMazeCell(row, column, cell);
        }
    }
    private bool IsCellInList(int row, int column)
    {
        return mazeCellsToMove.FindIndex((other) => other._row == row && other._column == column) >= 0;
    }
}
