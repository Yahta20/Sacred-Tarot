using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum CardState{
	unblock = -1,
	block = 0,
	open = 1,
	close = 2,
	death = 3
}

public class CardBihevior : MonoBehaviour
{
    private LevelGenerator LvlGen;
    private GameObject go;
	public CardState currentState{get;private set;}
    public Vector3 offset;
    private Vector3 velocity;

    public float smoothTime = .5f;
    private float posX;
    private float posY;
    private float axeY;
    private float f = 0.1f;
    private byte emountLight = 0;
    

    private float[] posit = new float[2];

    private int dirOfRotation = 0;
    private ushort TotalClicks = 0;

    public void StartPosition(float[] x)
    {
        posit[0] = x[0];
        posit[1] = x[1];
        offset.x = x[0];
        offset.y = x[1];
    }

    public void SetPosition(float[] x) {
        offset.x = posit[0] + x[0];
        offset.y = posit[1] + x[1];

        //Vector3 Newpost = offset;
    }

    public float[] getPosition()
    {
        float[] buf = new float[] { transform.position.x, transform.position.y };
        return buf;

    }

    public int GetCliks() {
        return TotalClicks;
    }

    public void SetLinkToCard(GameObject lo) {
        go = lo;
    }

    public byte getELight()
    {
        return emountLight;
    }
    
	void Awake()
    {
        LvlGen = GameObject.Find("GameManager").GetComponent(typeof(LevelGenerator)) as LevelGenerator;
        
        axeY = transform.rotation.y;
		currentState = CardState.unblock;
        //go = GetComponent<GameObject>();


    }

    void FixedUpdate()
    {
		
        if (emountLight == 0) {
            string s = go.name.ToString();
            var el = s[s.IndexOf("(")-1];
            
            try {
                int i = int.Parse(el.ToString());
                if (i == 0)
                {
                    emountLight = 10;
                }
                else { emountLight = Convert.ToByte(i); }

            }
            catch { emountLight = 11; }
            
        }
        transform.position = Vector3.SmoothDamp(transform.position, offset, ref velocity, smoothTime * Time.deltaTime);
        
		switch(currentState){
			case (CardState.open):
				axeY = Mathf.LerpAngle(axeY, 180, smoothTime);
				if (axeY>170.95f) {   
					currentState = CardState.block;
					TotalClicks++;
					axeY = 180;
				}
			break;
			case (CardState.close):
			 axeY = Mathf.LerpAngle(axeY, 0, smoothTime);
            //print(axeY+" flipBack "+go.name);
            if (axeY > 350.0f)
            {
				currentState = CardState.unblock;
                axeY = 0;
            }
			break;
			case (CardState.death):
				f += Time.deltaTime;
				axeY += 10f+(f * f+10 + Time.deltaTime);
			break;
		}
		
		transform.eulerAngles = new Vector3(0, axeY, 0);
    }
	public void SetState(CardState cs)
	{
		currentState = cs;
	}
	
    void OnMouseDown()
    {
      //  print("1");
        //if (!isopen)
        //{
        ////    print("2");
        //    if (isBlock == false)
        //    {
        //  //      print("3");
        //        if (!ischosen)
        //        {
        //    //        print("4");
        //            ischosen = true;
        //            LvlGen.setChosenCard(go);
        //        }
        //    }
        //}
		
		if (currentState == CardState.unblock){
			currentState = CardState.open;
			LvlGen.setChosenCard(go);
		}
			
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
