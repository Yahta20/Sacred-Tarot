using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] HighArcCard;
    [SerializeField] private GameObject[] CupArcCard;
    [SerializeField] private GameObject[] PentArcCard;
    [SerializeField] private GameObject[] SwordArcCard;
    [SerializeField] private GameObject[] WandArcCard;
    [SerializeField] private GameObject PartSys;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text triesLabel;

    private GameObject[] SetOfCard = new GameObject[25];
    private GameObject BufCard1 = null;
    private GameObject BufCard2 = null;
    private const float OffsetX = 1.5f;
    private const float OffsetY = 2.2f;

    private const string path = "Assets/Data/Prog.dat";
    private string Param;

    private int[] BufParam = new int[5];

    int emountcups;
    int emountpent;
    int emountsword;
    int emountwand;
    int number = -1;
    int EmountOfOpencard = 0;
    long Score=0;
    int tries = 10;

    void Awake()
    {
        StreamReader sr = new StreamReader(path);
        if (sr != null) {//Чтение достижений народного хозяйства
            int i = 0;
            while (!sr.EndOfStream) {
                BufParam[i] = Convert.ToInt32(sr.ReadLine());
                i++;
            }
        }

        int[,] MapOfCards = Tassovaty(BufParam);

        SetOfCard[24] = HighArcCard[UnityEngine.Random.Range(0, HighArcCard.Length)];
        
        for (int i = 0; i < 12; i++) {//Создание масива карт


            switch (MapOfCards[i, 1]) {
                case 0:
                    SetOfCard[i] = CupArcCard[MapOfCards[i, 0]];
                    SetOfCard[i + 12] = CupArcCard[MapOfCards[i, 0]];
                    break;
                case 1:
                    SetOfCard[i] = PentArcCard[MapOfCards[i, 0]];
                    SetOfCard[i + 12] = PentArcCard[MapOfCards[i, 0]];
                    break;
                case 2:
                    SetOfCard[i] = SwordArcCard[MapOfCards[i, 0]];
                    SetOfCard[i + 12] = SwordArcCard[MapOfCards[i, 0]];
                    break;
                case 3:
                    SetOfCard[i] = WandArcCard[MapOfCards[i, 0]];
                    SetOfCard[i + 12] = WandArcCard[MapOfCards[i, 0]];
                    break;
            }
        }

        for (int i = 0; i < 25; i++) {//Перемешивание карт
            GameObject tmp = SetOfCard[i];
            int r = UnityEngine.Random.Range(i, SetOfCard.Length);
            SetOfCard[i] = SetOfCard[r];
            SetOfCard[r] = tmp;
        }

        Camera cam = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
        cameraBS cbs = cam.GetComponent(typeof(cameraBS)) as cameraBS;
        
        float xpos = -3f;
        float ypos = -4.4f;
        for (int i = 0; i < 5; i++)
        {

            for (int j = 0; j < 5; j++)
            {
                float PosX = xpos + (OffsetX * i);
                float PosY = ypos + (OffsetY * j);

                //card = Instantiate
                int num = i * 5 + j;

                SetOfCard[num] = Instantiate(SetOfCard[num], new Vector3(PosX, PosY, 0), Quaternion.identity) as GameObject;
                Transform t = SetOfCard[num].transform;
                cbs.rost(t);
                CardBihevior CardBH = SetOfCard[num].GetComponent<CardBihevior>();
                CardBH.SetPosition(PosX, PosY);
            }
        }
    }

    

    private int[,] Tassovaty(int[] inArray)//создание масива карт что будут на поле
    {
        int[,] cardEmount = new int[12, 2];

        for (int i = 0; i < 12; i++) {
            int Masty = UnityEngine.Random.Range(0, 4);
            int nomerCarty = UnityEngine.Random.Range(0, inArray[Masty]);
            cardEmount[i, 0] = nomerCarty;
            cardEmount[i, 1] = Masty;
        }

        return cardEmount;
    }

    void FixedUpdate()
    {
        scoreLabel.text = Score.ToString();
        triesLabel.text = tries.ToString();
        CardBihevior CardBH;
        bool stat;

    }

    private void LockAllCards(bool b) {
        ///Locking or Unlocking all cards by putting bool charakter
        for (int j = 0; j < 25; j++)
        {
            if (SetOfCard[j] != null)
            {
                CardBihevior Card = SetOfCard[j].GetComponent<CardBihevior>();
                Card.SetBlock(b);
            }
        }
    }

    private void CheckOfArkan(GameObject go) {
        int em = 0;
        for (int m = 0; m < 25; m++) // узнаем сколько карт открыто
        {
            if (go != null)
            {
                em++;
            }
        }
        float pX= go.transform.position.x;
        float pY = go.transform.position.y;
        CardBihevior bihevCard = go.GetComponent<CardBihevior>();
        Debug.Log( go.name);
        switch (go.name)
        {
            case "moon(Clone)":
                //количество открытых карт
                
                if (em == 0)
                {
                    Score += 180;
                }

                if (em >= 2 & em <= 23)
                {
                    Score += 100;
                }

                if (em == 24)
                {
                    Score += 180;
                }

                break;
            case "sun(Clone)":
                //количество открытых карт
                GameObject[] cardtoopen = new GameObject[8];
                if (em == 0)
                {
                    Score += 190;
                    int v = 0;
                    for (int i = 0; i > 25; i++) {//Создание списка карт
                        
                            if (go.name != SetOfCard[i].name) {
                                if (SetOfCard[i].transform.position.y == pY | SetOfCard[i].transform.position.x == pX)
                                {
                                    cardtoopen[v] = SetOfCard[i];
                                Debug.Log(v + cardtoopen[v].name);
                                }
                            }
                        
                    }
                    for (int i=0;i> cardtoopen.Length;i++) {
                        CardBihevior cb = cardtoopen[i].GetComponent<CardBihevior>();
                        cb.toChose(true);
                    }

                }

                if (em >= 2 & em <= 23)
                {
                    Score += 100;
                }

                if (em == 24)
                {
                    Score += 190;
                }

                break;
            case "star(Clone)":
                //количество открытых карт

                if (em == 0)
                {
                    Score += 170;
                }

                if (em >= 2 & em <= 23)
                {
                    Score += 100;
                }

                if (em == 24)
                {
                    Score += 170;
                }

                break;
            case "tower(Clone)":
                //количество открытых карт

                if (em == 0)
                {
                    Score += 160;
                }

                if (em >= 2 & em <= 23)
                {
                    Score += 100;
                }

                if (em == 24)
                {
                    Score += 160;
                }

                break;
        }
    }

    private void setChosencard(CardBihevior cb) {

    }
    /*
      for (int i = 0; i < 25; i++)
        {
            if (SetOfCard[i] != null)
            {
                CardBH = SetOfCard[i].GetComponent<CardBihevior>();
                stat = CardBH.getState();


                if (stat)
                {//Опредиление открытой карты
                    number = i;
                }

                if (number != -1)
                {// Block all card when one is chozen
                    if (number != i)
                    {
                        CardBH.SetBlock(true);
                    }
                }

                

                if (number == i & CardBH.Openstate())
                {
                    
                    if (SetOfCard[i].name == "sun(Clone)"   | SetOfCard[i].name == "star(Clone)" |
                        SetOfCard[i].name == "tower(Clone)" | SetOfCard[i].name == "moon(Clone)")
                    {
                        
                        CheckOfArkan(SetOfCard[i]);
                    }
                    else
                    {
                        number = -1;
                        EmountOfOpencard++;
                        if (EmountOfOpencard == 1)
                        {
                            BufCard1 = SetOfCard[i];
                        }
                        if (EmountOfOpencard == 2)
                        {
                            BufCard2 = SetOfCard[i];
                        }
                        LockAllCards(false);
                        CardBH.SetBlock(true);
                    }
                }

                if (EmountOfOpencard == 2)
                {
                    //Все карты блокируются
                    LockAllCards(true);

                    if (BufCard1.name == BufCard2.name)//Если две карты одинаковые.
                    {
                        //destruction
                        CardBihevior ACard = BufCard1.GetComponent<CardBihevior>();
                        CardBihevior BCard = BufCard2.GetComponent<CardBihevior>();
                        
                        int a = ACard.GetCliks();
                        int b = BCard.GetCliks();
                        int c = (a*5)+(b*5)-10;
                        c = 30 - c;
                        if (c < 0) {
                            c = 0;
                        }
                        Score += c;
                        

                        ACard.Death();
                        BCard.Death();

                        float[] fc = ACard.getPosition();
                        float[] sc = BCard.getPosition();
                        GameObject Splash1 = Instantiate(PartSys, new Vector3(fc[0], fc[1], 0), Quaternion.identity) as GameObject;
                        GameObject Splash2 = Instantiate(PartSys, new Vector3(sc[0], sc[1], 0), Quaternion.identity) as GameObject;
                        Destroy(Splash1, 4);
                        Destroy(Splash2, 4);
                        Destroy(BufCard1, 1.5f);
                        Destroy(BufCard2, 1.5f);
                        tries++;

                    }

                    if (BufCard1.name != BufCard2.name) {
                        CardBihevior ACard = BufCard1.GetComponent<CardBihevior>();
                        CardBihevior BCard = BufCard2.GetComponent<CardBihevior>();
                        ACard.notAcouple();
                        BCard.notAcouple();
                        tries--;
                    }
                    //Unlock cards
                    LockAllCards(false);

                    BufCard1 = null;
                    BufCard2 = null;
                    EmountOfOpencard = 0;

                }
                
               
            }
        }
     
     
     */
}



