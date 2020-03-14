using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public static class Progress 
{
    public static int emountcups;
    public static int emountpent ;
    public static int emountsword;
    public static int emountwand ;
    public static int maxScore   ;

    public static int[] getData() {return new int[5] { emountcups, emountpent, emountsword, emountwand, maxScore}; }
    public static void setEmountCups  (int i) {emountcups =i; }
    public static void setEmountPents (int i) {emountpent =i; }
    public static void setEmountSwords(int i) {emountsword=i; }
    public static void setEmountWands (int i) {emountwand =i; }
    public static void setEmountScore (int i) {maxScore   =i; }
}
