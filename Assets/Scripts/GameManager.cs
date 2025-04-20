using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Minigame Flags")]
    [SerializeField] public bool Minigame1_Flag;
    [SerializeField] public bool Minigame2_Flag;
    [SerializeField] public bool Minigame3_Flag;
    //[SerializeField] private bool Minigame4_Successful;

    [Header("Computers")]
    [SerializeField] private GameObject computerMinigame1;
    [SerializeField] private GameObject computerMinigame2;
    [SerializeField] private GameObject computerMinigame3;
    //[SerializeField] private GameObject computerMinigame4;

    [Header("Material")]
    [SerializeField] private Material materialIncorrect;
    [SerializeField] private Material materialCorrect;

    private void Awake()
    {
        
    }

    private void Update()
    {
        ChangeVisibilityOfComputer(computerMinigame1, materialIncorrect, materialCorrect, Minigame1_Flag);
        ChangeVisibilityOfComputer(computerMinigame2, materialIncorrect, materialCorrect, Minigame2_Flag);
        ChangeVisibilityOfComputer(computerMinigame3, materialIncorrect, materialCorrect, Minigame3_Flag);
    }

    private void ChangeVisibilityOfComputer(GameObject computer, Material incorrect, Material correct, bool flag)
    {
        MeshRenderer renderer = computer.GetComponent<MeshRenderer>();
        Material[] compMaterials = renderer.materials;
        if (!flag)
        {
            compMaterials[3] = incorrect;
            renderer.materials = compMaterials;
        }
        else
        {
            compMaterials[3] = correct;
            renderer.materials = compMaterials;
        }
    }

}
