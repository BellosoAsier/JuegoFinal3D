using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItem : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject hand;

    private GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddWeapon();
        }
    }

    private void AddWeapon()
    {
        obj = Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation);
        obj.transform.SetParent(hand.transform);
        obj.transform.localScale = new Vector3(30f, 30f, 30f);
        obj.transform.localPosition = new Vector3(0.015f, 0f, -0.125f);
        obj.transform.localRotation = Quaternion.Euler(-108f, 0f, 90f);
    }
}
