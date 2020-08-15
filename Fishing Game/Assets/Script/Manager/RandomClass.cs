using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class Random
    {
        public static System.Random ran = new System.Random();
        public int rSpeed = ran.Next(400,500);
    public int rTime = ran.Next(3,6);
    public int rDraw = ran.Next(20,50);
        }

