//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(Camera))]


//public class CameraBihevior : MonoBehaviour
//{
//    public List<Transform> targets;
//    public Vector3 offset;
//    private Vector3 velocity;
//    public float smoothTime = .5f;
//    private Camera cam;
//    private Canvas can;
//    private CanvasScaler cs;
//    private float screenWight;
//    private float osHight;

//    // Start is called before the first frame update
//    void Start()
//    {  
//        can = GameObject.Find("MainCanvas").GetComponent(typeof(Canvas)) as Canvas;
//        //cam = GetComponent<Camera>();
//        cs = can.GetComponent<CanvasScaler>();
//        //cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
//        //   offset.x = 1.55f;
//        //   offset.y = 3.28f;
//        offset.z = -10;
//        transform.position = Vector3.SmoothDamp(transform.position, offset,ref velocity , smoothTime);
//        screenWight = GetMaxDistance()[0] + 2;
//        osHight = GetMaxDistance()[1];
//    }
//    // Update is called once per frame
//    void LateUpdate()
//    { 
//       transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime);    
//       mashtab();
//    }
//    private float getHorizontalAngle(Camera camera)
//    {
//        float vFOVrad = camera.fieldOfView * Mathf.Deg2Rad;
//        float cameraHeightAt1 = Mathf.Tan(vFOVrad * .5f);
//        return Mathf.Atan(cameraHeightAt1 * camera.aspect) * 2f * Mathf.Rad2Deg;
//    }
//    private float SetHorizontalAngle(float horizontalFOV, float aspect)
//    {
//        return Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan((horizontalFOV * Mathf.Deg2Rad) / 2f) / aspect); 
//    }
//    private void move(float ofsy) {
//        offset.y = ofsy;
//        //Vector3 CentPoint = getCentr();
//        Vector3 Newpost = offset;//+CentPoint;
//        transform.position = Vector3.SmoothDamp(transform.position, Newpost, ref velocity, smoothTime);
//    }
//    private void zome(float anglew) {

//        if (cam.orthographic != false) {
//            cam.orthographic = false;

//        }
//        //FIND ANGLE for camera
//        var cuality = 0.001f;
//        //

//        //zoom of objects by changin the 
//        if (Mathf.Abs(cam.fieldOfView - SetHorizontalAngle(anglew, cam.aspect))> cuality)
//        {
//            if (SetHorizontalAngle(anglew, cam.aspect)+ cuality > cam.fieldOfView | SetHorizontalAngle(anglew, cam.aspect) - cuality < cam.fieldOfView)
//            {
//                if (cam.fieldOfView < SetHorizontalAngle(45, cam.aspect))
//                    cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, SetHorizontalAngle(anglew, cam.aspect), Time.deltaTime);
//                else
//                    cam.fieldOfView = Mathf.Lerp(SetHorizontalAngle(anglew, cam.aspect), cam.fieldOfView, Time.deltaTime);
                
//                //cam.fieldOfView = SetHorizontalAngle(45f, cam.aspect);
//            }   
//        }
//        // zoom of ui
        
//    }
//    private void mashtab() {
//        var asp = cam.aspect;
//        var screenHight = screenWight / asp;
//        var anglew = Mathf.Abs(Mathf.Rad2Deg * 2 * Mathf.Atan((screenWight / 2) / offset.z));
//        var ofy = (screenHight / 2) - ((osHight / 2) + 1);//
//        var offy = Mathf.Abs(ofy / 0.75f);

//        move(offy);
//        //zome(anglew);

//        //print("( " + screenHight + " / " + osHight + " / " + ofy + " / " + offy + ")");
//        //tang = Mathf.Rad2Deg*2*Mathf.Atan(offset.z/(screenWight/2));
//    }
//    float[] GetMaxDistance()
//    {
//        float[] posit = new float[2] { 0, 0 };
//        if (targets.Count > 0)
//        {
//            var bounds = new Bounds(targets[0].position, Vector3.zero);
//            for (int i = 0; i < targets.Count; i++)
//            {
//                bounds.Encapsulate(targets[i].position);
//            }
//            posit[0] = bounds.size.x;
//            posit[1] = bounds.size.y;   
//        }
//        return posit;
//    }
//    Vector3 getCentr() {
//        if (targets[0].position!=null) {
//            return Vector3.zero;
//        }
//        if (targets.Count == 1)
//            return targets[0].position;
//        var bounds = new Bounds(targets[0].position, Vector3.zero);
//        for (int i = 0; i < targets.Count; i++)
//        {
//            bounds.Encapsulate(targets[i].position);
//        }
//        return bounds.center;
//    }
//    public void rost(Transform t) {
//        targets.Add(t);
//    }

    
//}
