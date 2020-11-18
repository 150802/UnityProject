using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int gunDamage = 1;
    public int specialDamage = 1;
    public float fireRate = .25f;
    public float weaponRange = 50f;
    public float hitForce = 50f;
    public float attackRadius = 5;
    public LayerMask enemyLayers;
    public Transform gunEnd;
    public Camera fpsCam;
    public Animator characterAnimator;
    private LineRenderer gunEffect;
    public GameObject attackPoint;

    private void Start()
    {
        gunEffect = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetButtonDown ("Fire1"))
        {
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
                    hit.collider.GetComponent<HealthScript>().Hit(gunDamage);
                }
            }
            else
            {
                gunEffect.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
            }
        }

        //Special weapon controller
        if (Input.GetKeyDown("1"))
        {
            //Start attack animation
            characterAnimator.SetTrigger("isAttacking");

            //Get all close enemies
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.transform.position, attackRadius, enemyLayers);

            //Hit all close enemies
            foreach(Collider enemy in hitEnemies)
            {
                print("hit!!!!!");
                enemy.GetComponent<HealthScript>().Hit(specialDamage);
            }
        }
    }

    private IEnumerator ShotEffect()
    {
        gunEffect.enabled = true;
        yield return new WaitForSeconds(.07f);
        gunEffect.enabled = false;
    }
}
