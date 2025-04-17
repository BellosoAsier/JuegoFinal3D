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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChooseCombination()
    {
        PuzzleRunCombination prc = listCombinations[UnityEngine.Random.Range(0,listCombinations.Count)];

        frame.GetComponent<MeshRenderer>().material = prc.material;

        foreach (GameObject go in listGameObjectsComb)
        {
            foreach(Transform t in go.transform)
            {
            }
        }
    }
}
