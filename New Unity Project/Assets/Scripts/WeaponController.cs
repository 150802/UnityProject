using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int gunDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 50f;
    public Transform cameraLeader;
    public Transform gunEnd;
    public Camera fpsCam;
    public GameObject explotionPrefab;

    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private LineRenderer gunEffect;
    private float nextFire;

    private void Start()
    {
        gunEffect = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown ("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;

            StartCoroutine(ShotEffect());

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            gunEffect.SetPosition(0, gunEnd.position);

            if (Physics.Raycast(rayOrigin,fpsCam.transform.forward, out hit, weaponRange))
            {
                gunEffect.SetPosition(1, hit.point);
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(hit.normal * hitForce);
                }

                

                if (hit.collider.GetComponent<HealthScript>() != null) {
                    hit.collider.GetComponent<HealthScript>().hit = true;
                }
            }
            else
            {
                gunEffect.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }

        if (Input.GetKeyDown("1") && Time.time > nextFire)
        {
            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            nextFire = Time.time + fireRate;

            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
            {
                gunEffect.SetPosition(1, hit.point);
                Instantiate(explotionPrefab, new Vector3(hit.point.x, hit.point.y, hit.point.z), Quaternion.identity);
            }
            else
            {
                gunEffect.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunEffect.enabled = true;
        yield return shotDuration;
        gunEffect.enabled = false;
    }
}
