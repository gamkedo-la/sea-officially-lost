﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOnGround : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Terrain.activeTerrain != null)
        {
            Vector3 fixedSpot = transform.position;
            fixedSpot.y = Terrain.activeTerrain.SampleHeight(fixedSpot) + Terrain.activeTerrain.transform.position.y;
            transform.position = fixedSpot;
        }
    }
}
