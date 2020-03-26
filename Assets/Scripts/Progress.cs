using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

[System.Serializable]
public class Progress 
{
    private byte emountOfCups     ;
    private byte emountOfPent     ;
    private byte emountOfSword    ;
    private byte emountOfWand     ;

    private byte opensOfCups      ;
    private byte opensOfPent      ;
    private byte opensOfSword     ;
    private byte opensOfWand      ;

    private byte emountOfTries    ;

    private byte emountLightCups  ;
    private byte emountLightPents ;
    private byte emountLightSwords;
    private byte emountLightWands ;
    
    public long maxScore   ;

    public byte[]   getCardData()   { return new byte[4] { emountOfCups, emountOfPent, emountOfSword, emountOfWand};}
    public byte[]   getLightData()  { return new byte[4] { emountLightCups, emountLightPents, emountLightSwords, emountLightWands }; }
                                                                   
    public byte getEmountCups  ()   {return emountOfCups  ; }        
    public byte getEmountPents ()   {return emountOfPent  ; }
    public byte getEmountSwords()   {return emountOfSword ; }
    public byte getEmountWands ()   {return emountOfWand  ; }
    public byte getEmountTries ()   {return emountOfTries ; }

    public byte getEmountLightCups  () {return emountLightCups  ; }  
    public byte getEmountLightPents () {return emountLightPents ; }
    public byte getEmountLightSwords() {return emountLightSwords; }
    public byte getEmountLightWands () {return emountLightWands ; }

    public long getMaxScore()       { return maxScore; }           
    public void setMaxScore(long i) { maxScore = i; }

    public void setEmountTries (byte i)      {emountOfTries  = i; }

    public void setEmountLightCups  (byte l) { emountLightCups += l  ; setEmountCups   (l); }
    public void setEmountLightPents (byte l) { emountLightPents  += l ; setEmountPents (l); }
    public void setEmountLightSwords(byte l) { emountLightSwords += l; setEmountSwords (l); }
    public void setEmountLightWands (byte l) { emountLightWands  += l ; setEmountWands (l); }

    private void setEmountCups  (byte l) {
        if (l> emountOfCups & l != emountOfCups) {
            opensOfCups += 1;
        }
        if (opensOfCups> emountOfCups) {

            emountOfCups = opensOfCups;
            opensOfCups = 0;
        }
        if (emountOfCups>9) { emountOfCups = 9; }
        //Debug.Log("Cups:"+opensOfCups+"|"+ emountOfCups+"|"+l);
        //emountOfCups   = i;
    }

    private void setEmountPents (byte l) {//emountOfPent   = i;
        if (l > emountOfPent & l != opensOfPent)
        {
            opensOfPent += 1;   
        }
        
        if (opensOfPent  > emountOfPent)
        {
            emountOfPent = opensOfPent ;
            opensOfPent  = 0;
        }
        if (emountOfPent > 9) { emountOfPent = 9; }
        //Debug.Log("Pent:" + opensOfCups + "|" + emountOfCups + "|" + l);
    }

    private void setEmountSwords(byte l) {//emountOfSword  = i;
        if (l > emountOfSword & l != opensOfSword)
        {
            opensOfSword += 1;
            
        }
        if (opensOfSword > emountOfSword)
        {
            emountOfSword = opensOfSword;
            opensOfSword = 0;
        }
        if (emountOfSword > 9) { emountOfSword = 9; }
        //Debug.Log("Sword:" + opensOfCups + "|" + emountOfCups + "|" + l);
    }

    private void setEmountWands (byte l) {//emountOfWand   = i; 
        if (l > emountOfWand & l != opensOfWand)
        {
            opensOfWand += 1;

        }
        if (opensOfWand > emountOfWand)
        {
            emountOfWand = opensOfWand;
            opensOfWand = 0;
        }
        if (emountOfWand > 9) { emountOfWand = 9; }
        //Debug.Log("Wand:" + opensOfCups + "|" + emountOfCups + "|" + l);
    }


    

    

}
