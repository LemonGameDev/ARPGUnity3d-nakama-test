using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPGcamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            this.transform.position = target.position + offset;
        }
    }
}