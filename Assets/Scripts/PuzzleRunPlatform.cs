using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRunPlatform : MonoBehaviour
{
    [SerializeField] private bool IsPlatform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (IsPlatform)
            {
               gameObject.SetActive(false);
            }
        }
    }
}
