using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{/*
    public GameObject buttMenu;
    public GameObject buttExit;
    

    // Start is called before the first frame update
    void Start()
    {
        buttMenu.SetActive(true);
        buttExit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowExitMenu()
    {
        buttMenu.SetActive(false);
        buttExit.SetActive(true);
    }
    public void ShowButtMenu()
    {
        buttMenu.SetActive(true);
        buttExit.SetActive(false);
    }*/
    public void StartGame()
    {
        Application.LoadLevel("SampleScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
