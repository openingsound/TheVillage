using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class endpoint : MonoBehaviour
{

    public GameObject Column;

    private float nowTime;
    private float makeTime = 25.0f;
    public List<int> MoneyAndExpList;
    void loadMnE()
    {
        string filePath_MnE = Application.persistentDataPath + "/MoneyAndExp.txt";
        string jdata = File.ReadAllText(filePath_MnE);
        MoneyAndExpList = JsonUtility.FromJson<Serialization<int>>(jdata).target;
    }

    // Start is called before the first frame update
    void Start()
    {
        nowTime = Time.time;
        loadMnE();
        makeTime = (float)MoneyAndExpList[2] * 2.0f + 10.0f;
    }

    // Update is called once per frame
    void Update()
    {


        if (Time.time - nowTime > makeTime)
        {
            nowTime = Time.time;
            GameObject temp = Instantiate(Column);
            temp.transform.parent = gameObject.transform;

            float randomY = UnityEngine.Random.Range(-1.3f, -1.2f);

            temp.transform.localPosition = new Vector3(-gameObject.transform.localPosition.x - 5, randomY, 0);
            temp.transform.localScale = new Vector3(1, 1, 1);
        }
    }


}