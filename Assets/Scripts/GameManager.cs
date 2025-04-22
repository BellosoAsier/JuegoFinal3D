using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Minigame Flags")]
    public bool Minigame1_Flag;
    public bool Minigame2_Flag;
    public bool Minigame3_Flag;
    //public bool Minigame4_Successful;

    [Header("Puzzle")]
    [SerializeField] private AudioSource audioMinigame1;
    [SerializeField] private AudioSource audioMinigame2;
    [SerializeField] private AudioSource audioMinigame3;

    [Header("Computers")]
    [SerializeField] private GameObject computerMinigame1;
    [SerializeField] private GameObject computerMinigame2;
    [SerializeField] private GameObject computerMinigame3;
    //[SerializeField] private GameObject computerMinigame4;

    [Header("Door Block")]
    [SerializeField] private GameObject block1;
    [SerializeField] private GameObject block2;
    [SerializeField] private GameObject block3;

    [Header("Material")]
    [SerializeField] private Material materialIncorrect;
    [SerializeField] private Material materialCorrect;

    [SerializeField] private InputManagerSO inputManager;

    private void OnEnable()
    {
        inputManager.EnableInputs();
    }

    private void OnDisable()
    {
        inputManager.DisableInputs();
    }

    private void Update()
    {
        ChangePuzzleState(computerMinigame1, block1, materialIncorrect, materialCorrect, Minigame1_Flag);
        ChangePuzzleState(computerMinigame2, block2, materialIncorrect, materialCorrect, Minigame2_Flag);
        ChangePuzzleState(computerMinigame3, block3, materialIncorrect, materialCorrect, Minigame3_Flag);
    }

    private void ChangePuzzleState(GameObject computer, GameObject block, Material incorrect, Material correct, bool flag)
    {
        MeshRenderer renderer = computer.GetComponent<MeshRenderer>();
        Material[] compMaterials = renderer.materials;
        if (!flag)
        {
            block.SetActive(true);
            
            compMaterials[3] = incorrect;
            renderer.materials = compMaterials;
        }
        else
        {

            block.SetActive(false);
            compMaterials[3] = correct;
            renderer.materials = compMaterials;
        }
    }
}
