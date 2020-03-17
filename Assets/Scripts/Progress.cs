using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public static class Progress 
{
    public static byte emountOfCups;
    public static byte emountOfPent;
    public static byte emountOfSword;
    public static byte emountOfWand;
    public static byte emountOfTries;
    public static ushort emountLightCups    ;
    public static ushort emountLightPents   ;
    public static ushort emountLightSwords  ;
    public static ushort emountLightWands   ;


    public static long maxScore   ;

    public static int[] getCardData() {return new int[6] { emountOfCups, emountOfPent, emountOfSword, emountOfWand, (int)maxScore, emountOfTries };}
    public static int[] getLightData() { return new int[6] { emountOfCups, emountOfPent, emountOfSword, emountOfWand, (int)maxScore, emountOfTries }; }

    public static void setEmountCups  (byte i) {emountOfCups =i; }
    public static void setEmountPents (byte i) {emountOfPent =i; }
    public static void setEmountSwords(byte i) {emountOfSword=i; }
    public static void setEmountWands (byte i) {emountOfWand =i; }
    public static void setEmountTries (byte i) { emountOfTries = i; }

    public static void setEmountLightCups   (ushort l){emountLightCups = l;}  
    public static void setEmountLightPents  (ushort l){emountLightPents =l;}
    public static void setEmountLightSwords (ushort l){emountLightSwords=l;}
    public static void setEmountLightWands  (ushort l){emountLightWands =l;}

    public static void setEmountmaxScore(long i) {maxScore   =i; }
}
