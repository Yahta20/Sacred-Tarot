using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void StartGame()
    {
        Application.LoadLevel("Level");
    }
    public void ExitGame()
    {
        Application.Quit();
        print("vse norm");
    }
}
