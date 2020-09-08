using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tree : MonoBehaviour
{

    public GameObject Column;

    private float nowTime;
    private float makeTime = 1.5f;
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

            float randomY = UnityEngine.Random.Range(-3.7f, -2.6f);

            temp.transform.localPosition = new Vector3(-gameObject.transform.localPosition.x + 8, randomY, 0);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }

}