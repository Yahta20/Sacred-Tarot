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

    private int DirOfRotation = 0;
    private int TotalClicks = 0;

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

        //go = GetComponent<GameObject>();
        //while (dirofrotation == 0)
        //{
        //    dirofrotation = random.range(-50, 50);
        //    if (dirofrotation != 0)
        //    {
        //        dirofrotation = dirofrotation / mathf.abs(dirofrotation);
        //    }
        //}

    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime * Time.deltaTime);
        
        if (deathtime) {
            transform.Rotate(0, f * DirOfRotation, 0);
            f += 0.5f+Mathf.Sin(Mathf.PI/6);
        }
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

    }

    void OnMouseDown()
    {
        if (!isopen)
        {
            if (isBlock == false)
            {
                if (!ischosen)
                {
                    ischosen = true;
                    LvlGen.setChosenCard(go);
                }
            }
        }
    }

    void Canvas()
    {
        
    }

}
