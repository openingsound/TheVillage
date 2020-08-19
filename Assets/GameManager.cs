using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.InteropServices;

[System.Serializable]
public class Item {

    public string Type, Name, Tree, Land,Levelst;
    public int level;
    public Image image;
    public Item(string type, string name, string tree, string land, string levelst)
    {
        Type = type;
        Name = name;
        Tree = tree;
        Land = land;
        Levelst = levelst;
    }
}
public class GameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList,CurItemList;
    string filePath;
    public GameObject Shop;
    public GameObject[] Slot;

    void Start()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        //엑셀로 하면 매 줄마다 '\n'이 들어가 있음
        for(int i=0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4]));
        }
        print("good");
        int temcou = 0;
        foreach (Item x in AllItemList)
        {
            temcou++;
            if (temcou == 1) continue;
            print(x.Name+"  : "+ x.Levelst);
            x.level = System.Convert.ToInt32(x.Levelst);
            print(x.level);
        }
        filePath = Application.persistentDataPath + "/MyItemText.txt";
        CurItemList = AllItemList.FindAll(x => x.level <= 3);
        print("Good");
        for(int i = 1; i < CurItemList.Count; i++)
        {
            Slot[i].SetActive(i < CurItemList.Count);
            Slot[i].GetComponentInChildren<Text>().text = i < CurItemList.Count ? CurItemList[i].Name : "";
            Transform sun = Slot[i].transform.GetChild(0);
            int cost = CurItemList[i].level * 10 + CurItemList[i].Name.Length;
            sun.GetChild(3).GetComponentInChildren<Text>().text = i < CurItemList.Count ? System.Convert.ToString(cost) + "원" : "";
        }
        //Load();
    }
    //void Load()
    //{
    //    if (!File.Exists(filePath)) { ResetItemClick(); return; }
    //    string jdata = File.ReadAllText(filePath);
    //}
    //void Save()
    //{
    //    File.WriteAllText(filePath,"하이");
    //}
    //public void ResetItemClick()
    //{
    //    Item BasicItem = AllItemList.Find(x => x.Name == "Apple");
    //    MyItemList = new List<Item>() { BasicItem };
    //    Save();
    //    Load();
    //}
    public void shopin()
    {
        Shop.SetActive(true);
    }
    public void shopout()
    {
        Shop.SetActive(false);
    }
}
