using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Random
{
    public static System.Random ran = new System.Random();
    public int rSpeed = ran.Next(450, 700);
    public int rTime = ran.Next(4, 7);
    public int rDraw = ran.Next(2000, 3500);
    public int rNum = ran.Next(1, 10001);
}