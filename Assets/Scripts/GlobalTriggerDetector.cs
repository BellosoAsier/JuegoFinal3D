using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectorType { Minigame1_Start, Minigame1_End, Minigame2_Start, Minigame2_End, Minigame3_Restart, Minigame3_CloseDoor, Minigame3_End }
public class GlobalTriggerDetector : MonoBehaviour
{
    [SerializeField] private DetectorType detectorType;

    private GoblinSpawner minigame1;
    private MazeSpawner minigame2;
    private PuzzleRun minigame3;
    private void Start()
    {
        minigame1 = FindAnyObjectByType<GoblinSpawner>();
        minigame2 = FindAnyObjectByType<MazeSpawner>();
        minigame3 = FindAnyObjectByType<PuzzleRun>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (detectorType)
            {
                case DetectorType.Minigame1_Start:
                    minigame1.enabled = true;
                    break;
                case DetectorType.Minigame1_End:
                    minigame1.enabled = false;
                    break;
                case DetectorType.Minigame2_Start:
                    minigame2.StartMaze();
                    break;
                case DetectorType.Minigame2_End:
                    minigame2.EndMaze(true);
                    break;
                case DetectorType.Minigame3_Restart:
                    minigame3.ChooseCombination();
                    minigame3.OpenDoor();
                    break;
                case DetectorType.Minigame3_CloseDoor:
                    minigame3.CloseDoor();
                    break;
                case DetectorType.Minigame3_End:
                    FindAnyObjectByType<GameManager>().Minigame3_Flag = true;
                    minigame3.OpenDoor();
                    break;
                default:
                    break;
            }
        }
    }
}
