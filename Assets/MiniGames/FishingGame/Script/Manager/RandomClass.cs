using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Random
    {
        public static System.Random ran = new System.Random();
        public int rSpeed = ran.Next(630,830);
    public int rTime = ran.Next(4,7);
    public int rDraw = ran.Next(2800,3800);
    public int rNum = ran.Next(1,10001);
        }

