using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DetectorType { Minigame1_Start, Minigame1_End, Minigame2_Start, Minigame2_End }
public class GlobalTriggerDetector : MonoBehaviour
{
    [SerializeField] private DetectorType detectorType;

    private GoblinSpawner minigame1;
    private void Start()
    {
        minigame1 = FindAnyObjectByType<GoblinSpawner>();
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
                    //Implementar
                    break;
                case DetectorType.Minigame2_End:
                    //Implementar
                    break;
                default:
                    break;
            }
        }
    }
}
