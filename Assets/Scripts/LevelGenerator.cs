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
    [SerializeField] private GameObject   PartSys;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text triesLabel;
    [SerializeField] public float cardSizeX;//      = 1.3f;
    [SerializeField] public float cardSizeY;//      = 2f;
    [SerializeField] public float setX;//           = cardSizeX / 4 * (1 - 0.61f);
    [SerializeField] public float setY;//           = (cam.aspect/(GetMaxDistance(1)/ GetMaxDistance(2))) * setX;
    [SerializeField] public float reservSpaceX;//   =0.75 
    [SerializeField] public float reservSpaceY;//   =0.9

    private GameObject[] SetOfCard = new GameObject[25];
    private GameObject[,] CardPack = new GameObject [5,5];
    
    private GameObject    BufCard1  = null;
    private GameObject    BufCard2  = null;

    private CameraBihevior cbs = null;
    
    private const string path       = "Assets/Data/Prog.dat";
    private string Param;

    private int[] BufParam          = new int[5];

    private byte deletedCards = 0;

    private long emountcups =0;
    private long emountpent =0;
    private long emountsword=0;
    private long emountwand =0;
    private long tries = 0;
    private long Score = 0;
    private long maxScore = 0;


    private bool beatMaxScore = false;
    
    

    private float wcard = 0;
    private float hcard = 0;

    //system functions

    void Awake()
    {
        BufParam =  Progress.getCardData();

        emountcups  = BufParam[0];
        emountpent  = BufParam[1];
        emountsword = BufParam[2];
        emountwand  = BufParam[3];
        tries = BufParam[5];
        if (BufParam[4] == 0)
        {
            //Welcome canvas
            //Lor of game
            //print("Max score is 2000");
            maxScore    = 2000;
            Progress.setEmountMaxScore(BufParam[4]); 
        }
        if (BufParam[5]==0) {
            //Welcome canvas
            //Lor of game
            //print("hello");
            tries = 5;
        }


        publishCards();

        setX = cardSizeX/2*(1-0.61f);
        wcard = cardSizeX*5+6*setX;
        hcard = cardSizeY * 5 + 6 * setX;
        settingParam();
    }

    void LateUpdate()
    {
        //android ramsy
        if (Application.platform == RuntimePlatform.Android)
        {
            //if (Input.GetKey(KeyCode.Home))
            //{
            //    Application.LoadLevel("Start");
            //    return;
            //}
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.LoadLevel("Start");
                return;
            }
            /*if ( Input.GetKey(KeyCode.Menu)) {
                tries++;
            }*/
        }


        //Updating data of game

        updateSave();
        settingParam();
        print(deletedCards);
        if (deletedCards == 25) {
            publishCards();
            deletedCards = 0;
        }
        float[] a = { setX, setY };
        //Set card position whith steps cards
        deletedCards = 0;
        foreach (GameObject child in SetOfCard)
        {
            if (child != null)
            {
                CardBihevior ChildBihevior = child.GetComponent(typeof(CardBihevior)) as CardBihevior;
                float[] cardPos = ChildBihevior.getPosition();
                a[0] = (cardPos[0] / cardSizeX) * setX;
                a[1] = (cardPos[1] / cardSizeY) * setY;
                ChildBihevior.SetPosition(a);
            }
            else {
                deletedCards++;
            }
            
        }
        
        if (BufCard1 != null) {
            CardBihevior FirstChosenCard = BufCard1.GetComponent<CardBihevior>();
            CheckOfArkan(BufCard1,0);
            //print("allo0");
            if (FirstChosenCard.Openstate())
            {
                
                LockAllCards(false);
              //  print("allo");
            }
            else {
                //print("allo1");
                LockAllCards(true);
            }
        }

        if (BufCard2 != null)
        {
            CardBihevior SecondChosenCard = BufCard1.GetComponent<CardBihevior>();

            if (SecondChosenCard.Openstate())
            {
                LockAllCards(false);
                
            }
            else
            {
                
                LockAllCards(true);
            }
        }
        //sravnenie of cards 
        if (BufCard1 != null & BufCard2 != null)
        {
            LockAllCards(true);
            CardBihevior ACard = BufCard1.GetComponent<CardBihevior>();
            CardBihevior BCard = BufCard2.GetComponent<CardBihevior>();

            if (ACard.Openstate() && BCard.Openstate())
            {

                if (BufCard1.name == BufCard2.name)//Если две карты одинаковые.
                {
                    //destruction

                    int fCardClick = ACard.GetCliks();
                    int sCardClick = BCard.GetCliks();
                    int c = (fCardClick * 5) + (sCardClick * 5) - 10;
                    c = 30 - c;
                    if (c < 0)
                    {
                        c = 0;
                    }
                    Score += c;
                    // make own procedure for single card
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
                if (BufCard1.name != BufCard2.name)
                {
                    ACard.notAcouple();
                    BCard.notAcouple();
                    tries--;
                }


                BufCard1 = null;
                BufCard2 = null;
            }
        }
        else {

                LockAllCards(false);
        }

    }

    private void publishCards()
    {
        int[,] MapOfCards = Tassovaty(BufParam);//Map of cards and emount of it that will be playing
        //Set patron Arcan as 25th card
        SetOfCard[24] = HighArcCard[UnityEngine.Random.Range(0, HighArcCard.Length)];
        //CardPack[0,0] = HighArcCard[UnityEngine.Random.Range(0, HighArcCard.Length)];
        //Создание масива карт
        for (int i = 0; i < 12; i++)
        {
            switch (MapOfCards[i, 1])
            {
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
        //Перемешивание карт
        for (int i = 0; i < 25; i++)
        {
            GameObject tmp = SetOfCard[i];
            int r = UnityEngine.Random.Range(i, SetOfCard.Length);
            SetOfCard[i] = SetOfCard[r];
            SetOfCard[r] = tmp;
        }
        //Conecting drawing area
        Camera cam = GameObject.Find("Main Camera").GetComponent(typeof(Camera)) as Camera;
        cbs = cam.GetComponent(typeof(CameraBihevior)) as CameraBihevior;
        cbs.setOrtographicSet(false);

        float xpos = cardSizeX * -2;//3f;
        float ypos = cardSizeY * -2;//-4.4f;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                float[] posit = new float[2] { xpos + (cardSizeX * i), ypos + (cardSizeY * j) };
                int num = i * 5 + j;
                SetOfCard[num] = Instantiate(SetOfCard[num], new Vector3(posit[0], posit[1], 0), Quaternion.identity) as GameObject;
                CardBihevior CardBH = SetOfCard[num].GetComponent<CardBihevior>();
                CardBH.StartPosition(posit);
                CardBH.SetLinkToCard(SetOfCard[num]);
                cbs.rost(SetOfCard[num].transform);
            }
        }
    }


    //public functions
    public void setChosenCard(GameObject go) {
        if (BufCard1 == null) {
            BufCard1 = go;
            LockAllCards(true);
        } else if (BufCard2 == null) {
            BufCard2 = go;
            LockAllCards(true);
        }
    }

    public void updateSave() {

        scoreLabel.text = Score.ToString();
        triesLabel.text = tries.ToString();
        Progress.setEmountTries((byte)(tries >> 8));
        if (Score > BufParam[4] & !beatMaxScore) { Progress.setEmountMaxScore(Score); tries += 5; beatMaxScore = true; }

    }


    //private functions

    private int[,] Tassovaty(int[] inArray)//создание масива карт что будут на поле
    {
        int[,] cardEmount = new int[12, 2];

        for (int i = 0; i < 12; i++) {
            int Masty = UnityEngine.Random.Range(0, 4);//chose of cardmark 
            int nomerCarty = UnityEngine.Random.Range(0, inArray[Masty]);//Random making of new card
            cardEmount[i, 0] = nomerCarty;
            cardEmount[i, 1] = Masty;
        }

        return cardEmount;
    }

    private void settingParam() {
        var koef = cbs.getaspect() * (reservSpaceY);
        setY = setX / koef;
        cbs.setCameraAngle(Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(cbs.offset.z) , wcard));
    }

    private float roundFloat(float n) {
        return ((float)(int)(n * 100)) / 100;
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
    
    private void CheckOfArkan(GameObject go, byte numberInBuffer ) {
        int em = 0;
        for (int m = 0; m < 25; m++) // узнаем сколько карт открыто
        {
            if (go != null)
            {
                em++;
            }
        }

        CardBihevior bihevCard = go.GetComponent<CardBihevior>();
        float[] position = bihevCard.getPosition();
        int chanseOption = UnityEngine.Random.Range(0, 100);
        chanseOption = (chanseOption % 2 == 0) ? chanseOption = 0 : chanseOption = 1;
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

                bihevCard.Death();
                Score += 190;
                

                float[] fc = bihevCard.getPosition();
                
                GameObject Splash1 = Instantiate(PartSys, new Vector3(fc[0], fc[1], 0), Quaternion.identity) as GameObject;
                Destroy(Splash1, 4);
                Destroy(go, 1.0f);
                BufCard1 = null;
                LockAllCards(false);
                tries++;
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


    //Sun
    /*
    GameObject[] cardSunOpen = new GameObject[4];
    GameObject[] card2Open = new GameObject[4];
    byte num = 0;
    //print ( num);
    //make list of cards to open
    foreach (GameObject loopObj in SetOfCard)//(int i = 0; i > 25; i++)
    {
        if (loopObj != null) {
            if (numberInBuffer==0) {
                if (go.name != loopObj.name)
                {
                    if (chanseOption == 0)
                    {
                        if (loopObj.transform.position.y == position[1] )
                        {
                            print(loopObj.transform.position.x);
                            cardSunOpen[num] = loopObj;
                            num++;
                        }
                    }
                    else {
                        if (loopObj.transform.position.x == position[0])
                        {
                            print(loopObj.transform.position.x);
                            cardSunOpen[num] = loopObj;
                            num++;
                        }
                    }
                }
            }

        }
    }
    num = 0;
    foreach (GameObject lObj in cardSunOpen) {
        foreach (GameObject loopObj in SetOfCard)
        {
            if (num == card2Open.Length-1) {
                print(num);
                break;
            }
            if (lObj != loopObj) {
                if (lObj.name == loopObj.name) {
                    card2Open[num] = loopObj;
                    print(card2Open.Length);
                    num++;    
                }
            }

        }
    }

    foreach (GameObject loopObj in card2Open) {
        CardBihevior Card = loopObj.GetComponent<CardBihevior>();
        Card.Death();
    }

    if (em == 0)
    {
        Score += 190;
    }

    if (em >= 2 & em < 24)
    {
        Score += 100;
    }

    if (em == 24)
    {
        Score += 190;
    }

    */

    //Stuff
    /*
     * 
     * 
     * 
     * 
       
    StreamReader sr = new StreamReader(path);
        //Чтение достижений народного хозяйства
        if (sr != null) {
            int i = 0;
            while (!sr.EndOfStream) {
                BufParam[i] = Convert.ToInt32(sr.ReadLine());
                i++;
            }
             emountcups =BufParam[0];
             emountpent =BufParam[1];
             emountsword=BufParam[2];
             emountwand =BufParam[3];
             maxScore   =BufParam[4];
        }
    
    
    var cardasp = x / y;


       if ((cardasp/cbs.getaspect())>1.00f || (cardasp / cbs.getaspect()) < 1.00f) {

           var st = (cardasp / cbs.getaspect()).ToString();
           st = (st.Length>6) ? st.Substring(0, 6) : st;
           setY = (float.Parse(st)<1) ? Mathf.Lerp(setY, setY - 0.0025f, Time.deltaTime) : Mathf.Lerp(setY, setY + 0.0025f, Time.deltaTime);
           setX = (float.Parse(st)<1) ? Mathf.Lerp(setX, setX + 0.0025f, Time.deltaTime) : Mathf.Lerp(setX, setX - 0.0025f, Time.deltaTime);

       }



       setX =(setX > cardSizeX / 4 * (1 - 0.61f)) ? cardSizeX / 4 * (1 - 0.61f): cardSizeX / 4 * (1 - 0.61f);
       setY=(setY > cardSizeY / 4 * (1 - 0.61f)) ?cardSizeX / 4 * (1 - 0.61f): cardSizeX / 4 * (1 - 0.61f);
       */


    /*
    x = (x.Length > 3)   ? x.Substring(0, 3) : x;
    y = (y.Length > 3)   ? y.Substring(0, 3) : y;

    if ( float.Parse(x) != 6.0f) {
        setX = (cbs.GetMaxDistance(1) < 6) ? Mathf.Lerp(setX, setX + 0.02f, Time.deltaTime) : Mathf.Lerp(setX, setX - 0.02f, Time.deltaTime);

        if (x.Length > 3 ) {
            x = x.Substring(0, 3);
            setX = (float.Parse(x) == 6.0f) ? Mathf.Lerp(setX, float.Parse(x), Time.deltaTime) : Mathf.Lerp(setX, float.Parse(x), Time.deltaTime);

        }
    }
    if (float.Parse(y) != 9.0f)
    {
        setY = (cbs.GetMaxDistance(2) < 9) ? Mathf.Lerp(setY, setY + 0.02f, Time.deltaTime) : Mathf.Lerp(setY, setY - 0.02f, Time.deltaTime);

        if (y.Length > 3)
        {
            y = y.Substring(0, 3);
            setY = (float.Parse(y) == 9.0f) ? Mathf.Lerp(setY, float.Parse(y), Time.deltaTime) : Mathf.Lerp(setX, float.Parse(y), Time.deltaTime);
        }
    }
    //setY = (float.Parse(y) == 9.0f) ? setY : 0;



    //setX = (cbs.GetMaxDistance(1).ToString) ? Mathf.Lerp(setX, setX + 0.02f, Time.deltaTime) : setX;

    var condy = cbs.GetMaxDistance(2) / 4 / 6;
    */
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



