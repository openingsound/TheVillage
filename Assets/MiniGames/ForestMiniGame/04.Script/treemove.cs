using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treemove : MonoBehaviour
{
    float makeTime;
    void Start()
    {
        makeTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - makeTime > 5.0f)
            Destroy(gameObject);
    }
}
