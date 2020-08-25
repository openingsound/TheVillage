using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Food : MonoBehaviour
{
    public int num;
    public int cou;
    public Sprite sprite;
    public UnityEngine.UI.Image image;
    public UnityEngine.UI.Button Btn;
    public void Setdepth(int d)
    {
        GUI.depth = d;
    }
    public void SetFood(int num,int cou,Sprite sprite)
    {
        this.num = num;
        this.cou = cou;
        this.image = GetComponent<Image>();
        image.sprite = sprite;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
