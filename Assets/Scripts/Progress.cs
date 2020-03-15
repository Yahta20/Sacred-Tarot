using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public static class Progress 
{
    public static byte emountcups;
    public static byte emountpent;
    public static byte emountsword;
    public static byte emountwand;
    public static ushort emountLightCups    ;
    public static ushort emountLightPents   ;
    public static ushort emountLightSwords  ;
    public static ushort emountLightWands   ;


    public static int maxScore   ;

    public static int[] getCardData() {return new int[5] { emountcups, emountpent, emountsword, emountwand, maxScore};}
    public static int[] getLightData() { return new int[5] { emountcups, emountpent, emountsword, emountwand, maxScore }; }

    public static void setEmountCups  (byte i) {emountcups =i; }
    public static void setEmountPents (byte i) {emountpent =i; }
    public static void setEmountSwords(byte i) {emountsword=i; }
    public static void setEmountWands (byte i) {emountwand =i; }

    public static void setEmountLightCups   (ushort l){emountLightCups = l;}  
    public static void setEmountLightPents  (ushort l){emountLightPents =l;}
    public static void setEmountLightSwords (ushort l){emountLightSwords=l;}
    public static void setEmountLightWands  (ushort l){emountLightWands =l;}

    public static void setEmountScore (int i) {maxScore   =i; }
}
