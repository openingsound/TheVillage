using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(60, 60, 60);
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Rotate(60 * Time.deltaTime, 60 * Time.deltaTime
           , 60 * Time.deltaTime);
    }
}
