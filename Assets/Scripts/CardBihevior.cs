using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBihevior : MonoBehaviour
{
    private LevelGenerator LvlGen;


    private bool ischosen = false;
    private bool isopen = false;
    private bool isBlock = false;
    private bool deathtime = false;
    private bool flipBack = false;
    private float posX;
    private float posY;
    private float axeY;
    private int DirOfRotation=0;
    private int TotalClicks = 0;
    private float f = 0.1f;

    public bool getState() {
        return ischosen;
    }

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

    public void SetPosition(float x, float y) {
        posX = x;
        posY = y;
    }

    public float [] getPosition( )
    {
        float[] buf = new float[2];
        buf[0] = posX;
        buf[1] = posY;
        return buf;
    }

    public bool Openstate() {
        return isopen;
    }

    public int GetCliks() {
        return TotalClicks;
    }

    public void notAcouple() {
        flipBack= true;
    }

    public void toChose(bool b) {
         ischosen=b;
    }

    void Start()
    {
    LvlGen = GameObject.Find("GameManager").GetComponent(typeof(LevelGenerator)) as LevelGenerator;
        while (DirOfRotation == 0)
            {
                DirOfRotation = Random.Range(-50, 50);
                if (DirOfRotation != 0)
                {
                    DirOfRotation = DirOfRotation / Mathf.Abs(DirOfRotation);
                }
            }
        
    }

    void FixedUpdate()
    {
        if (deathtime) {
            transform.Rotate(0, f * DirOfRotation, 0);
            f += 0.5f;
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
                    
                }
            }
        }
    }


}
