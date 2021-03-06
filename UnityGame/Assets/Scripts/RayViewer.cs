﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float weaponRange = 50f;
    public Camera fpsCam;

    void Update()
    {
        Vector3 lineOrigin = fpsCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
    }
}
