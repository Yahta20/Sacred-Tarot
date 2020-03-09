﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraBihevior : MonoBehaviour
{
    public List<Transform> targets;
    private float aspect { get; set; }
    private float aprof { get; set; }
    private float OffsetY { get; set; }
    private float MashtabCofficient { get; set; }
    public float smoothTime = .5f;
    public Vector3 offset;
    private Vector3 velocity;
    private Camera cam;
    private float camangle;
    public float minZoom = 71f;

    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        camangle = cam.fieldOfView;
        OffsetY = 0;
        aprof = 80f;
        offset.y = 0.3f;
        offset.z = -10;
        startBoundsSpace(cam.aspect);
        settingUpdater(aprof);
        cam.fieldOfView = aprof;
        MashtabCofficient = 36.0f;
    }

    private void startBoundsSpace(float aspect)
    {
        aspect = this.aspect;
    }

    private void setBoundsSpace(float aspect)
    {
        if (this.aspect != aspect)
        {
            aspect = this.aspect;
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //float[] posit = GetMaxDistance();
        //        print("|x=" + posit[0] + "|y=" + posit[1]);
        transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime*Time.deltaTime);
        settingUpdater(cam.fieldOfView);
        
    }

 

   

    private void aperture(float anglew)
    {
        
        if (cam.orthographic != false)
        {
            cam.orthographic = false;
        }

        //var coranglew = Mathf.Lerp();
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, anglew, Time.deltaTime);

    }

    private void move(float ofsy)
    {
        // offset = ofsy;
        //Vector3 CentPoint = getCentr();
        //+CentPoint;
    }

    private float SetHorizontalAngle(float horizontalFOV, float aspect)
    {
        //float f = 
        return Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((horizontalFOV * Mathf.Deg2Rad) / 2f) / aspect);
        //return ((float)(int)(f * 100)) / 100;
    }

    public float getaspect() {
        return aspect;
    }

    public float getkoef()
    {

        float koef = cam.aspect/(GetMaxDistance(1)/ GetMaxDistance(2));
        return koef;
    }

    private void settingUpdaterInfo() {
        aspect = cam.aspect;
        //camangle = aspect / 0.75f;
        //print("|-Aspect Camera="+aspect+"-|-Clear Aspect=" + camangle +"" +
        //    "-|-Camera W="+cam.pixelWidth + "-|-Camera H=" + cam.pixelHeight);
        var wh = GetMaxDistance();
        //print("|-bounds W=" + wh[0] + "-|-bounds H=" + wh[1]+"-|");
        //print("|-bounds W=" + GetMaxDistance(6) + "-|-bounds H=" + GetMaxDistance(5) + "-|");
        


    }

    private void settingUpdater(float a) {
        settingUpdaterInfo();

        
        aperture(roundFloat(a));


        
    }

    private float roundFloat(float n)
    {
        return ((float)(int)(n * 100)) / 100;
    }

    public void rost(Transform t)
    {
                targets.Add(t);
    }

    public float GetMaxDistance(int numb)
    {

        float posit=0;
        if (targets.Count > 0)
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            switch (numb)
            {
                case 1:
                    return posit = bounds.size.x;
                case 2:
                    return posit = bounds.size.y;
                default:
                    if (numb%2==0)
                        return posit = bounds.size.x/0.8f;
                    if (numb%2==1)
                        return posit = bounds.size.y / 0.8f;
                    break;
            }
            
        }
        return posit;
    }

    public float[] GetMaxDistance()
    {
        
        float[] posit = new float[2] {0,0};
        if (targets.Count > 0)
        {
           
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            posit[0] = bounds.size.x / 0.8f;
            posit[1] = bounds.size.y / 0.8f;
        }
        
        return posit;
    }
}
