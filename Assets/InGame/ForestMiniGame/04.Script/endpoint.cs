using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endpoint : MonoBehaviour
{

    public GameObject Column;

    private float nowTime;
    private float makeTime = 30.0f;


    // Start is called before the first frame update
    void Start()
    {
        nowTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time - nowTime > makeTime)
        {
            nowTime = Time.time;
            GameObject temp = Instantiate(Column);
            temp.transform.parent = gameObject.transform;

            float randomY = Random.Range(-1.3f, -1.2f);

            temp.transform.localPosition = new Vector3(-gameObject.transform.localPosition.x - 5, randomY, 0);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }


}