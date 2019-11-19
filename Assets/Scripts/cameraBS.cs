using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]


public class cameraBS : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;

    private Vector3 velocity;
    public float smoothTime = .5f;

    private Camera cam;
    private Canvas can;
    private CanvasScaler cs;
    private int height;

    // Start is called before the first frame update
    void Start()
    {
        
        can = GameObject.Find("Canvas").GetComponent(typeof(Canvas)) as Canvas;
        cam = GetComponent<Camera>();
        cs = can.GetComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        //   offset.x = 1.55f;
        //   offset.y = 3.28f;
        offset.z = -10;

        
        transform.position = Vector3.SmoothDamp(transform.position, offset,ref velocity , smoothTime);
        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (targets.Count == 0) {
            transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime);
            
            return;
        }
        move();
        zome();


    }

    void move() {

        Vector3 CentPoint = getCentr();
        Vector3 Newpost = CentPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, Newpost, ref velocity, smoothTime);
    }

    private float getHorizontalAngle(Camera camera)
    {
        float vFOVrad = camera.fieldOfView * Mathf.Deg2Rad;
        float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
        return Mathf.Atan(cameraHeightAt1 * camera.aspect) * 2f * Mathf.Rad2Deg;
    }
    private float SetHorizontalAngle(float horizontalFOV, float aspect)
    {

        return Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((horizontalFOV * Mathf.Deg2Rad) / 2f) / aspect);
        
    }




    void zome() {
        if (cam.orthographic != false) {
            cam.orthographic = false;

        }
        //zoom of objects by changin the 
        if (Mathf.Abs(cam.fieldOfView - SetHorizontalAngle(45f, cam.aspect))>.5f)
        {
            if (SetHorizontalAngle(45, cam.aspect)+0.5f> cam.fieldOfView | SetHorizontalAngle(45, cam.aspect) - 0.5f < cam.fieldOfView)
            {
                if (cam.fieldOfView < SetHorizontalAngle(45, cam.aspect))
                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, SetHorizontalAngle(45, cam.aspect), Time.deltaTime);
                else
                    cam.fieldOfView = Mathf.Lerp(SetHorizontalAngle(45, cam.aspect), cam.fieldOfView, Time.deltaTime);
                
                //cam.fieldOfView = SetHorizontalAngle(45f, cam.aspect);
            }   
        }
        // zoom of ui
        int mph = cam.pixelHeight;
        int mpw = cam.pixelWidth;
        var asp = cam.aspect;
        
        print("|H:"+mph+ "|W:" + mpw + "|asp:" + asp+ "|\n");
    }
    float[] GetMaxDistance() {

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        float[] posit = new float[2]{ bounds.size.x, bounds.size.y };

        return posit;
    }
   
    Vector3 getCentr() {
        if (targets[0].position!=null) {
            return Vector3.zero;
        }
        if (targets.Count == 1)
            return targets[0].position;
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
    

    public void rost(Transform t) {
        targets.Add(t);

    }
}
