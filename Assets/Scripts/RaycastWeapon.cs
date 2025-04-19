using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public Camera playerCamera;
    private Transform laserOrigin;
    public float weaponRange = 50f;
    public float laserDuration = 0.05f;

    private LineRenderer laserLine;

    public void ShootLaser()
    {
        laserOrigin = GameObject.Find("WeaponPlayer").transform.GetChild(0);
        laserLine = laserOrigin.parent.gameObject.GetComponent<LineRenderer>();
        laserLine.SetPosition(0, laserOrigin.position);
        Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * weaponRange));
        }
        StartCoroutine(ShootLaserEnumerator());
    }

    IEnumerator ShootLaserEnumerator()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
