using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class position : MonoBehaviour
{
    public Text elementalText;
    public Camera camera;
    public Transform target;
    
    void Start()
    {
        target = GetComponent<Transform>();

    }
    void Update()
    {
        Vector3 screenPos = camera.WorldToScreenPoint(target.position);
        float x = screenPos.x+80;

        elementalText.transform.position = new Vector3(x+30,screenPos.y+50,elementalText.transform.position.z);
    }
}
