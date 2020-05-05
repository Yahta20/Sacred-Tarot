using System.Collections;
using System.Collections.Generic;
using UnityEngine;



	public enum CanvasState {
	clear=0,
	info=1,
	pause=2,
	over =3
}
//[RequireComponent(typeof(Canvas))]
public class CanvasBihevior : MonoBehaviour
{
	
	
	public GameObject gameInfo;
	public GameObject gamePause;
	public GameObject gameOver;
	
	public Canvas parentCanvas;
	
	public CanvasState currentState {get; private set;} 
    // Start is called before the first frame update
	


    void Awake()
    {
		
		
		currentState = CanvasState.info;
    }

	public void SetCanvasState(CanvasState cs){
		currentState = cs;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState){
			case (CanvasState.clear):
				gameInfo.SetActive(false);
				gamePause.SetActive(false);
				gameOver.SetActive(false);
			break;
			case (CanvasState.info):
				gameInfo.SetActive(true);
				gamePause.SetActive(false);
				gameOver.SetActive(false);
			break;
			case (CanvasState.pause):
				gameInfo.SetActive(false);
				gamePause.SetActive(true);
				gameOver.SetActive(false);
			break;
			case (CanvasState.over):
				gameInfo.SetActive(false);
				gamePause.SetActive(false);
				gameOver.SetActive(true);
			break;
			
		}
    }
}
