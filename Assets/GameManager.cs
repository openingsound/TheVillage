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
public class Item
{

    public string Type, Name, Tree, Land, Levelst, IsUisng, SCycle;
    public int level;
    public int Using;
    public int Cost;
    public int Cycle;
    public Image image;
    public Item(string type, string name, string tree, string land, string levelst, string isUsing, string cycle)
    {
        Type = type;
        Name = name;
        Tree = tree;
        Land = land;
        Levelst = levelst;
        IsUisng = isUsing;
        SCycle = cycle;
    }
}
public class GameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public List<Item> AllItemList, MyItemList, CurItemList, BuildList;
    string filePath;
    public GameObject Shop;
    public GameObject[] ShopSlot;
    public GameObject[] BuildSlot;
    public GameObject ShopPop;
    public GameObject BuildPop;
    public Text ExplainBox;
    void Start()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        //엑셀로 하면 매 줄마다 '\n'이 들어가 있음
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6]));
        }
        foreach (Item x in AllItemList)
        {
            //print(x.Name+"  : "+ x.Levelst);
            x.level = System.Convert.ToInt32(x.Levelst);
            x.Using = System.Convert.ToInt32(x.IsUisng);
            x.Cycle = System.Convert.ToInt32(x.SCycle);
            x.Cost = x.level * 10 + x.Cycle / 10;
        }
        filePath = Application.persistentDataPath + "/MyItemText.txt";
        ShopUpdate();

        //Load();
    }
    public void ShopUpdate()
    {
        CurItemList = AllItemList.FindAll(x => x.level <= 3);
        BuildList = AllItemList.FindAll(x => x.Using == 1);
        CurItemList = CurItemList.FindAll(x => x.Using == 0);
        for (int i = 0; i < ShopSlot.Length; i++)
        {
            ShopSlot[i].SetActive(i < CurItemList.Count);
            if (i > CurItemList.Count) continue;
            ShopSlot[i].GetComponentInChildren<Text>().text = i < CurItemList.Count ? CurItemList[i].Name : "";
            Transform sun = ShopSlot[i].transform.GetChild(0);
            sun.GetChild(3).GetComponentInChildren<Text>().text = i < CurItemList.Count ? System.Convert.ToString(CurItemList[i].Cost) + "원" : "";
        }
        for (int i = 0; i < BuildList.Count; i++)
        {
            BuildSlot[i].SetActive(true);
            BuildSlot[i].transform.GetChild(0).GetComponentInChildren<Text>().text = BuildList[i].Name;//이름
            BuildSlot[i].transform.GetChild(1).GetComponentInChildren<Text>().text = System.Convert.ToString(BuildList[i].Cost) + "원";
            BuildSlot[i].transform.GetChild(3).GetComponentInChildren<Text>().text = System.Convert.ToString(BuildList[i].Cycle / 60) + "시간 " + (BuildList[i].Cycle % 60 == 0 ? "" : System.Convert.ToString(BuildList[i].Cycle % 60) + "분");
            BuildSlot[i].transform.GetChild(2).GetComponentInChildren<Text>().text = System.Convert.ToString((BuildList[i].Cycle * 3) / 60) + "시간 " + ((BuildList[i].Cycle * 3) % 60 == 0 ? "" : System.Convert.ToString(BuildList[i].Cycle * 3 % 60) + "분");
        }
    }
    public string curType2;
    public void TabClick(string tabName)
    {
        curType2 = tabName;
    }
    public int curShopSlotNum = 0;
    public void SlotClick(int ShopSlotNum)
    {
        curShopSlotNum = ShopSlotNum;
        Item CurItem = CurItemList[ShopSlotNum];
        ShopPop.SetActive(true);
        ExplainBox.text = "<b>" + CurItem.Name + "을 " + CurItem.Cost + "원에 구매 하시겠습니까? </b>";

    }
    public void BuildClick(int BuildSlotNum)
    {
        Item CurItem = BuildList[BuildSlotNum];
    }
    public void BuyShopPop()
    {
        string CurItemName = CurItemList[curShopSlotNum].Name;
        Item CurItem = AllItemList.Find(x => x.Name == CurItemName);
        CurItem.Using = 1;
        ShopPop.SetActive(false);
        ShopUpdate();
    }
    public void OpenShopPop()
    {
        if (BuildPop.activeInHierarchy)
        {
            BuildPop.SetActive(false);
        }

        Shop.SetActive(true);

        //UIManager.UImanager.isShop = true;
    }
    public void CloseShopPop()
    {
        ShopPop.SetActive(false);

        UIManager.UImanager.isShop = false;
    }
    public void OpenBuildPop()
    {
        if (UIManager.UImanager.isShop)
        {
            return;
        }

        BuildPop.SetActive(true);
    }
    public void CloseBuildPop()
    {
        BuildPop.SetActive(false);
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

        UIManager.UImanager.isShop = true;
    }
    public void shopout()
    {
        Shop.SetActive(false);

        UIManager.UImanager.isShop = false;
    }
}
