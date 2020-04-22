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
    [SerializeField] private Text SemLightLabel;
    [SerializeField] private Text CemLightLabel;
    [SerializeField] private Text PemLightLabel;
    [SerializeField] private Text WemLightLabel;
    [SerializeField] public float cardSizeX;//      = 1.3f;
    [SerializeField] public float cardSizeY;//      = 2f;
    [SerializeField] public float setX;//           = cardSizeX / 4 * (1 - 0.61f);
    [SerializeField] public float setY;//           = (cam.aspect/(GetMaxDistance(1)/ GetMaxDistance(2))) * setX;
    [SerializeField] public float reservSpaceX;//   = 0.75 
    [SerializeField] public float reservSpaceY;//   = 0.9

    private GameObject[] SetOfCard = new GameObject[25];
    private GameObject[,] CardPack = new GameObject[5, 5];

    private GameObject BufCard1 = null;
    private GameObject BufCard2 = null;

    private CameraBihevior cbs = null;

    private Canvas mainCanvas = null;

    private Progress progress = new Progress();

    private byte[] BufParam = new byte[5];

    private byte deletedCards = 0;

    private byte emountcups = 0;
    private byte emountpent = 0;
    private byte emountsword = 0;
    private byte emountwand = 0;
    private sbyte tries = 0;
    private long Score = 0;
    private long maxScore = 0;

    private bool beatMaxScore = false;

    private float wcard = 0;
    private float hcard = 0;

    //system functions

    void Awake()
    {
        //SaveLoad.fileDelete();
        //Conecting drawing area
        mainCanvas = GameObject.Find("Main_Canvas").GetComponent(typeof(Canvas)) as Canvas;
        Camera cam = GameObject.Find("Main_Camera").GetComponent(typeof(Camera)) as Camera;
        cbs = cam.GetComponent(typeof(CameraBihevior)) as CameraBihevior;
        cbs.setOrtographicSet(false);
        SaveLoad.fileDelete();
        progress = SaveLoad.loadData();

        //BufParam =  progress.getCardData();

        tries = progress.getEmountTries();
        maxScore = progress.getMaxScore();


        if (maxScore < 2000)
        {
            //Welcome canvas
            //Lor of game
            maxScore = 2000;
            progress.setMaxScore(maxScore);
        }

        if (tries == 0)
        {
            //Welcome canvas
            //Lor of game
            //print("hello");
            tries = 5;
        }


        publishCards();


        setDisplayParam();
    }
    private void FixedUpdate()
    {
        if (tries == 0)
        {
            //Canvas of deth
            LockAllCards(true);

        }
    }
    void LateUpdate()
    {
        //android ramsy
        if (Application.platform == RuntimePlatform.Android)
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                //pause
                Application.LoadLevel("Start");
                return;
            }

        }

        


        //Updating data of game

        updateSave();
        setDisplayParam();

        if (deletedCards == 25)
        {
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
            else
            {
                deletedCards++;
            }

        }

        if (BufCard1 != null)
        {
            CardBihevior FirstChosenCard = BufCard1.GetComponent<CardBihevior>();
            CheckOfArkan(BufCard1);
            //print("allo0");
            if (FirstChosenCard.Openstate())
            {

                LockAllCards(false);
                //  print("allo");
            }
            else
            {
                //print("allo1");
                LockAllCards(true);
            }
        }

        if (BufCard2 != null)
        {
            CardBihevior SecondChosenCard = BufCard2.GetComponent<CardBihevior>();
            CheckOfArkan(BufCard2);
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


                    score4cards(BufCard1, BufCard2);

                    // make own procedure for single card
                    //get light only once


                    destroingCard(BufCard1);
                    destroingCard(BufCard2);

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

        else
        {

            LockAllCards(false);
        }

    }

    private void score4cards(GameObject bufCard1, GameObject bufCard2)
    {
        CardBihevior ACard = BufCard1.GetComponent<CardBihevior>();
        CardBihevior BCard = BufCard2.GetComponent<CardBihevior>();
        int fCardClick = ACard.GetCliks();
        int sCardClick = BCard.GetCliks();
        int c = (fCardClick * 5) + (sCardClick * 5) - 10;
        c = 30 - c;
        if (c <= 0)
        {
            c = 0;
        }


        lightAdd(BufCard1.name, ACard.getELight());
        Score += c;

    }
    private void scoreArcan4cards(int i)
    {

        Score += i;

    }



    //public functions

    public void setChosenCard(GameObject go)
    {
        if (BufCard1 == null)
        {
            BufCard1 = go;
            LockAllCards(true);
        }
        else if (BufCard2 == null)
        {
            BufCard2 = go;
            LockAllCards(true);
        }
    }

    public void updateSave()
    {

        SemLightLabel.text = progress.getEmountLightSwords().ToString();
        CemLightLabel.text = progress.getEmountLightCups().ToString();
        PemLightLabel.text = progress.getEmountLightPents().ToString();
        WemLightLabel.text = progress.getEmountLightWands().ToString();
        scoreLabel.text = Score.ToString();
        triesLabel.text = tries.ToString();
        progress.setEmountTries(tries);
        if ((Score > maxScore) & !beatMaxScore) { progress.setMaxScore(Score); tries += 5; beatMaxScore = true; }
        SaveLoad.saveData(progress);
    }


    //private functions

    private void publishCards()
    {
        byte[,] MapOfCards = makeMapOfCards(progress.getCardData());//Map of cards and emount of it that will be playing
        //Set patron Arcan as 25th card
        SetOfCard[24] = HighArcCard[UnityEngine.Random.Range(0, HighArcCard.Length)];

        //Kreation map array
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
        setX = cardSizeX / 2 * (1 - 0.61f);
        wcard = cardSizeX * 5 + 6 * setX;
        hcard = cardSizeY * 5 + 6 * setX;
    }

    private byte[,] makeMapOfCards(byte[] inArray)//создание масива карт что будут на поле
    {
        byte[,] cardEmount = new byte[12, 2];

        /*for (int i = 0; i < 12; i++) {
            byte Masty = Convert.ToByte(UnityEngine.Random.Range(0, 4));                  //chose of cardmark 
            byte nomerCarty = Convert.ToByte(UnityEngine.Random.Range(0, inArray[Masty]));//Random making of new card
            cardEmount[i, 0] = nomerCarty;
            cardEmount[i, 1] = Masty;
        }*/
        for (int i = 0; i < 12; i++)
        {
            byte Masty = Convert.ToByte(i / 3);
            byte nomerCarty = Convert.ToByte(UnityEngine.Random.Range(0, inArray[Masty] + 1));
            cardEmount[i, 0] = nomerCarty;
            cardEmount[i, 1] = Masty;
        }

        return cardEmount;
    }

    private void lightAdd(string name, byte i)
    {
        string typeOfLight = name.Substring(0, 4);
        switch (typeOfLight)
        {
            case "pent":
                progress.setEmountLightPents(Convert.ToByte(i));
                break;
            case "wand":
                progress.setEmountLightWands(Convert.ToByte(i));
                break;
            case "swor":
                progress.setEmountLightSwords(Convert.ToByte(i));
                break;
            case "cups":
                progress.setEmountLightCups(Convert.ToByte(i));
                break;
        }
    }

    private void setDisplayParam()
    {
        var koef = cbs.getaspect() * (reservSpaceY);
        setY = setX / koef;
        cbs.setCameraAngle(Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(cbs.offset.z), wcard));
    }

    private float roundFloat(float n)
    {
        return ((float)(int)(n * 100)) / 100;
    }

    private void LockAllCards(bool b)
    {
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

    private void CheckOfArkan(GameObject go)
    {



        CardBihevior bihevCard = go.GetComponent<CardBihevior>();
        float[] position = bihevCard.getPosition();
        int chanseOption = UnityEngine.Random.Range(0, 100);
        chanseOption = (chanseOption % 2 == 0) ? chanseOption = 0 : chanseOption = 1;
        byte number = 0;
        int addScore = 0;
        byte arcan = 0;
        switch (go.name)
        {
            case "moon(Clone)":
                //количество открытых карт
                break;
            case "sun(Clone)":
                GameObject[] cardSunOpen = new GameObject[5];
                foreach (GameObject cO in SetOfCard)
                {
                    if (cO != null)//find cards in the line
                    {
                        if (chanseOption == 0)
                        {
                            if (go.transform.position.x == cO.transform.position.x & cO.GetInstanceID() != go.GetInstanceID() & cO != null)
                            {

                                cardSunOpen[number] = cO;
                                number++;
                                
                                //& cO.GetInstanceID() != go.GetInstanceID() & number >= cardSunOpen.Length
                            }
                        }
                        else
                        {
                            if (go.transform.position.y == cO.transform.position.y & cO.GetInstanceID() != go.GetInstanceID() & cO != null)
                            {
                                //& cO.GetInstanceID() != go.GetInstanceID() & number >= cardSunOpen.Length
                                cardSunOpen[number] = cO;
                                number++;
                                
                            }
                        }
                    }
                }
                if (BufCard1 != null)
                {
                    if (BufCard1.GetInstanceID() != go.GetInstanceID())
                    {
                        int co = 0;
                        foreach (GameObject soc in cardSunOpen) {
                            if (soc == null)
                            {
                                break;
                            }
                            else {
                                co++;
                            }
                            if (soc.GetInstanceID() == BufCard1.GetInstanceID())
                            {
                                co = 0;
                                break;
                            }
                            
                        }
                        if (co>0) {
                            cardSunOpen[co] = BufCard1;
                        }
                        foreach (GameObject cso in cardSunOpen)
                        {
                            if (cso != null)
                            {
                                
                               //if (cso.GetInstanceID() != BufCard1.GetInstanceID() & cso.name == BufCard1.name)
                               //{
                               //    destroingCard(BufCard1);
                               //    BufCard1 = null;
                               //    destroingCard(cso);
                               //    scoreArcan4cards(20);
                               //    break;
                               //}
                            }
                        }
                    }
                }
                //samecards in line destroy
                for (int i = 0; i < cardSunOpen.Length; i++)
                {
                    for (int j = 0; j < cardSunOpen.Length; j++)
                    {
                        if (cardSunOpen[i] != null & cardSunOpen[j] != null)
                        {
                            if (cardSunOpen[i].GetInstanceID() != cardSunOpen[j].GetInstanceID() & cardSunOpen[i].name == cardSunOpen[j].name)
                            {
                                destroingCard(cardSunOpen[i]);
                                destroingCard(cardSunOpen[j]);
                                cardSunOpen[j] = null;
                                cardSunOpen[i] = null;

                                scoreArcan4cards(20);
                            }
                        }
                    }
                }
                //othercard for para
                foreach (GameObject cso in cardSunOpen)
                {
                    foreach (GameObject co in SetOfCard)
                    {
                        if (co != null & cso != null)
                        {
                            if (co.name == cso.name & co.GetInstanceID() != cso.GetInstanceID())
                            {
                                destroingCard(cso);
                                destroingCard(co);
                                scoreArcan4cards(20);
                                break;
                            }
                        }
                    }
                }
                destroingCard(go);

                BufCard1 = null;
                BufCard2 = null;
                LockAllCards(false);
                break;
            case "star(Clone)":
                //количество открытых карт


                break;
            case "tower(Clone)":
                //количество открытых карт



                break;
        }

        switch (deletedCards)
        {
            case 0:
                addScore = arcan * 10;
                break;
            case 24:
                addScore = arcan * 10;
                break;
            default:
                if (arcan > 0)
                {
                    addScore = deletedCards * 10;
                }
                break;
        }

        Score += addScore;

    }

    private void destroingCard(GameObject go)
    {

        CardBihevior ACard = go.GetComponent<CardBihevior>();

        ACard.Death();

        float[] fc = ACard.getPosition();
        GameObject Splash1 = Instantiate(PartSys, new Vector3(fc[0], fc[1], 0), Quaternion.identity) as GameObject;
        Destroy(Splash1, 3.5f);
        Destroy(go, 1.6f);

    }
}
   