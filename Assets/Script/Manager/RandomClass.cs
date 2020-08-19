using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Random
{
    // 랜덤함수 객체 생성
    public static System.Random ran = new System.Random();

    // 노트 스피드를 400 ~ 500 랜덤으로 결정
    public int rSpeed = ran.Next(400, 500);

    // 노트 타임을 3 ~ 6 랜덤으로 결정
    public int rTime = ran.Next(3, 6);


    public int rDraw = ran.Next(20, 50);
}