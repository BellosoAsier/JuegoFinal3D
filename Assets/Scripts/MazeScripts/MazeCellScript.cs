using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellDirection { Start, Top, Down, Left, Right}
public class MazeCellScript
{
    public bool IsVisited = false;
    public bool WallRight = false;
    public bool WallTop = false;
    public bool WallLeft = false;
    public bool WallDown = false;
    public bool HasReward = false;
}
