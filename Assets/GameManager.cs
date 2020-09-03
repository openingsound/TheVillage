using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

[System.Serializable]
public class Serialization<T>
{
    public Serialization(List<T> _target) => target = _target;
    public List<T> target;
}

[System.Serializable]
public class Item {

    public string Type, Name, Tree, Land,Levelst,IsUisng;
    public int level;
    public int Using;
    public int Cost;
    public Image image;
    public Item(string type, string name, string tree, string land, string levelst,string isUsing)
    {
        Type = type;
        Name = name;
        Tree = tree;
        Land = land;
        Levelst = levelst;
        IsUisng = isUsing;
    }
}
public class GameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList,CurItemList;
    string filePath;
    public GameObject Shop;
    public GameObject[] Slot;
    public GameObject ShopPop;
    public Text ExplainBox;
    void Start()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        //엑셀로 하면 매 줄마다 '\n'이 들어가 있음
        for(int i=0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4],row[5]));
        }
        foreach (Item x in AllItemList)
        {
            //print(x.Name+"  : "+ x.Levelst);
            x.level = System.Convert.ToInt32(x.Levelst);
            x.Using = System.Convert.ToInt32(x.IsUisng);
            x.Cost = x.level * 10 + x.Name.Length;
        }
        filePath = Application.persistentDataPath + "/MyItemText.txt";
        ShopUpdate();

        //Load();
    }
    public void ShopUpdate()
    {
        CurItemList = AllItemList.FindAll(x => x.level <= 3);
        CurItemList = CurItemList.FindAll(x => x.Using == 0);
        for (int i = 0; i < CurItemList.Count; i++)
        {
            Slot[i].SetActive(i < CurItemList.Count);
            Slot[i].GetComponentInChildren<Text>().text = i < CurItemList.Count ? CurItemList[i].Name : "";
            Transform sun = Slot[i].transform.GetChild(0);
            sun.GetChild(3).GetComponentInChildren<Text>().text = i < CurItemList.Count ? System.Convert.ToString(CurItemList[i].Cost) + "원" : "";
        }
    }
    public string curType2;
    public void TabClick(string tabName)
    {
        curType2 = tabName;
    }
    public int curSlotNum =0;
    public void SlotClick(int slotNum)
    {
        curSlotNum = slotNum;
        Item CurItem = CurItemList[slotNum];
        ShopPop.SetActive(true);
        ExplainBox.text = "<b>" + CurItem.Name + "을 " + CurItem.Cost + "원에 구매 하시겠습니까? </b>";

    }
    public void BuyShopPop()
    {
        string CurItemName = CurItemList[curSlotNum].Name;
        Item CurItem = AllItemList.Find(x => x.Name == CurItemName);
        CurItem.Using = 1;
        ShopPop.SetActive(false);
        ShopUpdate();
    }
    public void CloseShopPop()
    {
        ShopPop.SetActive(false);
    }
    void Load()
    {
        if (!File.Exists(filePath)) { ResetItemClick(); return; }
        string jdata = File.ReadAllText(filePath);
        MyItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;


    }
    void Save()
    {
        MyItemList.Add(AllItemList[0]);
        MyItemList.Add(AllItemList[1]);
        string jdata = JsonUtility.ToJson(new Serialization<Item>(MyItemList));
        File.WriteAllText(filePath, jdata);
        
    }
    public void ResetItemClick()
    {
        Item BasicItem = AllItemList.Find(x => x.Name == "Apple");
        MyItemList = new List<Item>() { BasicItem };
        Save();
        Load();
    }
    public void shopin()
    {
        Shop.SetActive(true);
    }
    public void shopout()
    {
        Shop.SetActive(false);
    }
}
