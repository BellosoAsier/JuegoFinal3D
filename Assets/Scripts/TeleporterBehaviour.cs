using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TeleporterBehaviour : MonoBehaviour
{
    private bool ItHurts;
    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        destination = gameObject.transform.GetChild(0).position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.enabled = false; // Desactivar para evitar conflictos
                other.transform.position = destination;
                controller.enabled = true; // Volver a activar
            }
        }
    }
}
