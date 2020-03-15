using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Camera))]

public class CardBihevior : MonoBehaviour
{
    private LevelGenerator LvlGen;
    private GameObject go;

    public Vector3 offset;
    private Vector3 velocity;

    public bool  ischosen   {get; set;}
    private bool isopen     {get; set;}
    private bool isBlock    {get; set;}
    private bool deathtime  {get; set;}
    private bool flipBack   {get; set;}

    public float smoothTime = .5f;
    private float posX;
    private float posY;
    private float axeY;
    private float f = 0.1f;
    private float sizex = 1;//size x of card
    private float sizey = 1;//size y of card

    private float[] posit = new float[2];

    private int dirOfRotation = 0;
    private ushort TotalClicks = 0;

    public void Death() {
        deathtime = true;
    }

    public void SetBlock(bool a)
    {
        isBlock = a;
    }

    public bool GetBlock() {
        return isBlock;
    }

    public void StartPosition(float[] x)
    {
        posit[0]=x[0];
        posit[1]=x[1];
        offset.x = x[0];
        offset.y = x[1];
    }

    public void SetPosition(float [] x) {
        offset.x = posit[0]+x[0];
        offset.y = posit[1]+x[1];
        
        //Vector3 Newpost = offset;
    }

    public float[] getPosition()
    {
        float[] buf = new float[] { transform.position.x, transform.position.y };
        return buf;

    }

    public bool Openstate() {
        return isopen;
    }

    public int GetCliks() {
        return TotalClicks;
    }

    public void notAcouple() {
        flipBack = true;
    }

    public void SetLinkToCard(GameObject lo){
        go = lo;
    }
    
    void Awake()
    {
        LvlGen = GameObject.Find("GameManager").GetComponent(typeof(LevelGenerator)) as LevelGenerator;
        isopen = false;
        ischosen = false;
        deathtime = false;
        flipBack = false;
        axeY = transform.rotation.y;
        //go = GetComponent<GameObject>();
        

    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime * Time.deltaTime);
        
        if (deathtime) {
            
            axeY += 10f+Mathf.Sin(Mathf.PI/6);
        }
        if (ischosen) {
            axeY = Mathf.LerpAngle(axeY, 180, 0.05f);
            if (axeY>170.95f) {
                print("cho");
                isopen = true;
                ischosen = false;
                isBlock = true;
                TotalClicks++;
                axeY = 180;
            }
        }
        if (flipBack) {


            axeY = Mathf.LerpAngle(axeY, 0, 0.1f);
            //print(axeY+" flipBack "+go.name);
            if (axeY > 340.0f)
            {
                print("Koroche");
                axeY = 0;
                isopen = false;
                ischosen = false;
                flipBack = false;
                isBlock = false;
            }

   
        }
        transform.eulerAngles = new Vector3(0, axeY, 0);
    }


    void OnMouseDown()
    {
      //  print("1");
        if (!isopen)
        {
        //    print("2");
            if (isBlock == false)
            {
          //      print("3");
                if (!ischosen)
                {
            //        print("4");
                    ischosen = true;
                    LvlGen.setChosenCard(go);
                }
            }
        }
    }

    void Canvas()
    {
        
    }
/*
 
     
        if (ischosen) {
            //if card is chosen
            transform.Rotate(0,4.5f*DirOfRotation,0);

            if (transform.localEulerAngles.y>=180 & DirOfRotation > 0) {
                Quaternion q1 = Quaternion.Euler(new Vector3(0, 180, 0)); 
                transform.rotation = q1;
                isopen = true;
                ischosen = false;
                isBlock = true;
                TotalClicks++;
                axeY = 180;
                
            }

            if ((transform.localEulerAngles.y == 180 | transform.localEulerAngles.y <= -170) & DirOfRotation < 0)
            {
                Quaternion q1 = Quaternion.Euler(new Vector3(0, 180, 0));
                transform.rotation = q1;
                isopen = true;
                ischosen = false;
                isBlock = true;
                TotalClicks++;
                axeY = 180;
            }
            //LvlGen.
        }
        if (flipBack) {//return to normal position
            transform.Rotate(0, 3.1f * DirOfRotation, 0);
            axeY -= 3.1f;
            if ( axeY<0 )
            {
                Quaternion q1 = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.rotation = q1;
                isopen = false;
                ischosen = false;
                flipBack = false;
                isBlock = false;
                axeY = 0;
            }   
            
        }
     
     
     
     
     
     */
}
