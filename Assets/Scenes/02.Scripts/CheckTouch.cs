using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTouch : MonoBehaviour
{
    public static CheckTouch ClickedBlock = null;

    private Renderer myRenderer;
    public Color touchColor;
    public Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();

        NonClickBlock();
    }

    public void OnClickBlock()
    {
        myRenderer.material.color = touchColor;

        if(ClickedBlock != null)
        {
            ClickedBlock.NonClickBlock();
            
        }

        ClickedBlock = this.gameObject.GetComponent<CheckTouch>();
    }

    public void NonClickBlock()
    {
        myRenderer.material.color = originalColor;
    }
}
