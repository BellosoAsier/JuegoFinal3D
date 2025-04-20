using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRun : MonoBehaviour
{
    [System.Serializable]
    public class PuzzleRunCombination
    {
        public Material material;
        public List<string> combination;
    }

    [SerializeField] private List<PuzzleRunCombination> listCombinations;
    [SerializeField] private List<GameObject> listGameObjectsComb;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject mainDoor;

    // Start is called before the first frame update
    void Start()
    {
        ChooseCombination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCombination()
    {
        ResetLevel();

        PuzzleRunCombination prc = listCombinations[UnityEngine.Random.Range(0,listCombinations.Count)];

        frame.GetComponent<MeshRenderer>().material = prc.material;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (prc.combination[i][j].Equals('O'))
                {
                    listGameObjectsComb[i].transform.GetChild(j).GetComponent<BoxCollider>().isTrigger = true;
                }

                else if (prc.combination[i][j].Equals('X'))
                {
                    listGameObjectsComb[i].transform.GetChild(j).GetComponent<BoxCollider>().isTrigger = false;
                }
            }
        }
    }

    private void ResetLevel()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if(!listGameObjectsComb[i].transform.GetChild(j).gameObject.activeSelf)
                {
                    listGameObjectsComb[i].transform.GetChild(j).gameObject.SetActive(true);
                }
            }
        }
    }

    public void OpenDoor()
    {
        mainDoor.transform.GetChild(0).gameObject.SetActive(false);
        mainDoor.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void CloseDoor()
    {
        mainDoor.transform.GetChild(0).gameObject.SetActive(true);
        mainDoor.transform.GetChild(1).gameObject.SetActive(true);
    }
}
