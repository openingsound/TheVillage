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
    public int num;
    public int Exp;
    public Sprite image;
    public Item(string type, string name, string tree, string land, string levelst, string isUsing, string cycle,int exp)
    {
        Type = type;
        Name = name;
        Tree = tree;
        Land = land;
        Levelst = levelst;
        IsUisng = isUsing;
        SCycle = cycle;
        Exp = exp;
    }
}
public class GameManager : MonoBehaviour
{
    public TextAsset ItemDatabase;
    public TextAsset Pre_Cost;
    public TextAsset Now_Cost;
    public TextAsset Item_Amount;
    public TextAsset MoneyAndExp;
    public List<Item> AllItemList, MyItemList, CurItemList, BuildList, SellList;
    public List<int> PrePrice, NowPrice, ItemAmount;//지금 가지고 있는 총 량
    public List<int> Explist;
    public List<int> SellAmount;//현재 팔 양
    public List<int> MoneyAndExpList;

    string filePath, filePath_MnE, filePath_pre, filePath_Amount, filePath_now;
    public GameObject Shop;
    public GameObject[] ShopSlot;
    public GameObject[] BuildSlot;
    public GameObject[] SellSlot;
    public Sprite[] ImageSlot;
    public GameObject ShopPop;
    public GameObject BuildPop;
    public GameObject BuildPopUp;
    public GameObject AutionPopup;
    public GameObject SellPop;
    public GameObject NoMoneyPop;
    public Text ExplainBox, Build_ExplainBox, Auction_ExplainBox;
    public int UserMoney, UserExp, UserLevel;
    public Text Money_t, Exp_t, Level_t;
    public Image Exp_Fill;

    void Start()
    {
        string[] line = ItemDatabase.text.Substring(0, ItemDatabase.text.Length - 1).Split('\n');
        //엑셀로 하면 매 줄마다 '\n'이 들어가 있음
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            AllItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5], row[6], System.Convert.ToInt32(row[7])));
        }
        int n = 0;
        foreach (Item x in AllItemList)
        {
            //print(x.Name+"  : "+ x.Levelst);
            x.level = System.Convert.ToInt32(x.Levelst);
            x.Using = System.Convert.ToInt32(x.IsUisng);
            x.Cycle = System.Convert.ToInt32(x.SCycle);
            x.Cost = x.level * 10 + x.Cycle / 10;
            x.image = ImageSlot[n];
            x.num = n++;
        }
        filePath = Application.persistentDataPath + "/MyItemText.txt";
        filePath_MnE = Application.persistentDataPath + "/MoneyAndExp.txt";
        filePath_pre = Application.persistentDataPath + "/PreCost.txt";
        filePath_now = Application.persistentDataPath + "/NowCost.txt";
        filePath_Amount = Application.persistentDataPath + "/Amount.txt";
        print(filePath);
        ShopUpdate();

        line = Pre_Cost.text.Substring(0, Pre_Cost.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            PrePrice.Add(System.Convert.ToInt32(row[0]));
        }
        line = Now_Cost.text.Substring(0, Now_Cost.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            NowPrice.Add(System.Convert.ToInt32(row[0]));
        }
        //Load();
        line = Item_Amount.text.Substring(0, Item_Amount.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            ItemAmount.Add(System.Convert.ToInt32(row[0]));
            SellAmount.Add(0);
        }
        line = MoneyAndExp.text.Substring(0, MoneyAndExp.text.Length - 1).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            //tab으로 나눔
            MoneyAndExpList.Add(System.Convert.ToInt32(row[0]));
        }
        Load();
        Read_sta();
        ShopUpdate();
        SellUpdate();
       
        //Save();
        //print(filePath);

    }

    void Read_sta()
    {
        //정보들 파일에서 읽기
        UserMoney = MoneyAndExpList[0];
        UserExp = MoneyAndExpList[1];
        setLevel(MoneyAndExpList[1]);
        Money_t.text = System.Convert.ToString(UserMoney);
    }

 

    void Write_sta(int money, int exp)
    {
        UserMoney += money;
        UserExp += exp;
        //정보들 파일에 쓰기
        MoneyAndExpList[0] = UserMoney;
        MoneyAndExpList[1] = UserExp;
        Save();
        Read_sta();
    }

    public void ChangePrice()
    {
        //날이 바뀌거나 날이 바뀌고 입장하면 업데이트
        for (int i = 0; i < PrePrice.Count; i++)
        {
            PrePrice[i] = NowPrice[i];
            NowPrice[i] = AllItemList[i].Cost + UnityEngine.Random.Range(-(AllItemList[i].Cost / 3), (AllItemList[i].Cost / 3));
        }
        Save();
        Load();
    }
    public void ShopUpdate()
    {
        CurItemList = AllItemList.FindAll(x => x.level <= UserLevel);
        BuildList = AllItemList.FindAll(x => x.Using == 1);
        CurItemList = CurItemList.FindAll(x => x.Using == 0);

        for (int i = 0; i < ShopSlot.Length; i++)
        {
            ShopSlot[i].SetActive(i < CurItemList.Count);
            if (i >= CurItemList.Count) continue;
            ShopSlot[i].GetComponentInChildren<Text>().text = i < CurItemList.Count ? CurItemList[i].Name : "";
            Transform sun = ShopSlot[i].transform.GetChild(0);
            sun.GetChild(0).GetComponentInChildren<Image>().sprite = ImageSlot[CurItemList[i].num];
            sun.GetChild(3).GetComponentInChildren<Text>().text = i < CurItemList.Count ? System.Convert.ToString(CurItemList[i].Cost * 10) + "원" : "";
        }
        for (int i = 0; i < BuildList.Count; i++)
        {
            BuildSlot[i].SetActive(true);
            BuildSlot[i].transform.GetChild(0).GetComponentInChildren<Text>().text = BuildList[i].Name;//이름
            BuildSlot[i].transform.GetChild(1).GetComponentInChildren<Text>().text = System.Convert.ToString(BuildList[i].Cost*3) + "원";
            BuildSlot[i].transform.GetChild(3).GetComponentInChildren<Text>().text = "  " + System.Convert.ToString(BuildList[i].Cycle / 60) + "시간 " + (BuildList[i].Cycle % 60 == 0 ? "" : System.Convert.ToString(BuildList[i].Cycle % 60) + "분");
            BuildSlot[i].transform.GetChild(2).GetComponentInChildren<Text>().text = "  " + System.Convert.ToString((BuildList[i].Cycle * 3) / 60) + "시간 " + ((BuildList[i].Cycle * 3) % 60 == 0 ? "" : System.Convert.ToString(BuildList[i].Cycle * 3 % 60) + "분");
            BuildSlot[i].transform.GetChild(5).GetComponentInChildren<Image>().sprite = ImageSlot[BuildList[i].num];
        }
    }

    public void SellUpdate()
    {
        BuildList = AllItemList.FindAll(x => x.Using == 1);
        CurItemList = CurItemList.FindAll(x => x.Using == 0);
        for (int i = 0; i < BuildList.Count; i++)
        {
            SellSlot[i].SetActive(true);
            int PPrice = PrePrice[BuildList[i].num];
            int NPrice = NowPrice[BuildList[i].num];
            SellSlot[i].transform.GetChild(0).GetComponentInChildren<Text>().text = BuildList[i].Name;//이름
            SellSlot[i].transform.GetChild(1).GetComponentInChildren<Text>().text = System.Convert.ToString(PPrice);
            SellSlot[i].transform.GetChild(2).GetComponentInChildren<Text>().text = System.Convert.ToString(NPrice);
            SellSlot[i].transform.GetChild(12).GetComponentInChildren<Image>().sprite = ImageSlot[BuildList[i].num];//이름
            if (PPrice == NPrice)
                SellSlot[i].transform.GetChild(3).gameObject.SetActive(false);
            else
            {
                SellSlot[i].transform.GetChild(3).gameObject.SetActive(true);
                if (PPrice > NPrice)
                {
                    SellSlot[i].transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                    SellSlot[i].transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                    SellSlot[i].transform.GetChild(3).GetComponent<Text>().text = "(         " + System.Convert.ToString(PPrice - NPrice) + " )";
                }
                else
                {
                    SellSlot[i].transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
                    SellSlot[i].transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                    SellSlot[i].transform.GetChild(3).GetComponent<Text>().text = "(         " + System.Convert.ToString(NPrice - PPrice) + " )";
                }
            }
            SellSlot[i].transform.GetChild(4).GetComponentInChildren<Text>().text = System.Convert.ToString(SellAmount[BuildList[i].num]) + " / " + System.Convert.ToString(ItemAmount[BuildList[i].num]) + " 개";
        }
    }



    public void fun0(int num)
    {
        num = BuildList[num].num;
        SellAmount[num] -= 10;
        if (SellAmount[num] < 0) SellAmount[num] = 0;

        SellUpdate();
    }
    public void fun1(int num)
    {
        num = BuildList[num].num;
        SellAmount[num] -= 1;
        if (SellAmount[num] < 0) SellAmount[num] = 0;

        SellUpdate();
    }
    public void fun2(int num)
    {
        num = BuildList[num].num;
        SellAmount[num] += 1;
        if (SellAmount[num] > ItemAmount[num]) SellAmount[num] = ItemAmount[num];

        SellUpdate();
    }

    public void fun3(int num)
    {

        num = BuildList[num].num;
        SellAmount[num] += 10;
        if (SellAmount[num] > ItemAmount[num]) SellAmount[num] = ItemAmount[num];
        //print(num +" "+ SellAmount[num]);

        SellUpdate();
    }
    public void Aution_Sell_Btn(int num)
    {
        SellUpdate();
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
        ExplainBox.text = "<b>" + CurItem.Name + "을(를) " + CurItem.Cost*10 + "원에 구매 하시겠습니까? </b>";

    }
    public int curBuildSlotNum = 0;
    public void Build_SlotClick(int BuildSlotNum)//건설 구매창 띄우기
    {
        curBuildSlotNum = BuildSlotNum;
        Item CurItem = BuildList[BuildSlotNum];
        BuildPopUp.SetActive(true);
        Build_ExplainBox.text = "<b>" + CurItem.Name + "을(를) " + CurItem.Cost*3 + "원에 건설 하시겠습니까? </b>";
    }
    int NowSellNum = 0;
    public void Aution_SlotClick(int AuctionSlotNum)
    {
        Item CurItem = BuildList[AuctionSlotNum];
        NowSellNum = AuctionSlotNum;
        int itemp = NowPrice[CurItem.num];
        AutionPopup.SetActive(true);
        Auction_ExplainBox.text = CurItem.Name + "을(를) " + "개당 " + itemp + "원에 판매 하시겠습니까?(총 " + itemp * SellAmount[CurItem.num] + "원)";
    }
    public void AutionPopup_Sell()
    {
        NowSellNum = BuildList[NowSellNum].num;
        Write_sta(NowPrice[NowSellNum] * SellAmount[NowSellNum], 0);

        //SellAmount[CurItem.num] 만큼 ItemAmount[NowSellNum]에서 아이템 감소
        ItemAmount[NowSellNum] -= SellAmount[NowSellNum];

        for (int i = 0; i < SellAmount.Count; i++)
        {
            SellAmount[i] = 0;
        }
        Save();
        Load();
        SellUpdate();
       
        AutionPopup.SetActive(false);
    }
    public void AutionPopup_Close()
    {
        AutionPopup.SetActive(false);

    }
    public void BuildPopup_Build()//딱 건설을 하는 버튼
    {
        string CurItemName = BuildList[curBuildSlotNum].Name;
        Item CurItem = AllItemList.Find(x => x.Name == CurItemName);
        if (UserMoney >= CurItem.Cost*3)
        {
            Write_sta(-CurItem.Cost*3, 0);
            //건설 하기
        }
        else
        {
            NomoneyOpen();
        }
        BuildPopUp.SetActive(false);

    }
    public void BuildPopup_Close()
    {
        BuildPopUp.SetActive(false);

    }
    public void BuildClick(int BuildSlotNum)
    {
        Item CurItem = BuildList[BuildSlotNum];
    }
    public void BuyShopPop()//구매하는 버튼
    {
        string CurItemName = CurItemList[curShopSlotNum].Name;
        Item CurItem = AllItemList.Find(x => x.Name == CurItemName);
        
        ShopPop.SetActive(false);
        if (UserMoney >= CurItem.Cost*10)
        {
            Write_sta(-CurItem.Cost*10, 0);
            CurItem.Using = 1;
            //구매 하기
        }
        else
        {
            NomoneyOpen();
        }
        ShopUpdate();
        Save();
    }
    public void CloseShopPop()
    {
        ShopPop.SetActive(false);
    }
    public void OpenBuildPop()
    {
        ShopUpdate();
        BuildPop.SetActive(true);
    }
    public void CloseBuildPop()
    {
        BuildPop.SetActive(false);
    }
    public void OpenSelldPop()
    {
        for (int i = 0; i < SellAmount.Count; i++)
        {
            SellAmount[i] = 0;
        }
        SellUpdate();
        SellPop.SetActive(true);
    }
    public void CloseSellPop()
    {
        SellUpdate();
        SellPop.SetActive(false);
    }
    void NomoneyOpen() {
        NoMoneyPop.SetActive(true);
    }
    public void NoMoneyClose()
    {
        print("끄기");
        NoMoneyPop.SetActive(false);
    }



    void Load()
    {
        if (!File.Exists(filePath)) { ResetItemClick(); ChangePrice(); ChangePrice(); }
        string jdata = File.ReadAllText(filePath);
        AllItemList = JsonUtility.FromJson<Serialization<Item>>(jdata).target;
        jdata = File.ReadAllText(filePath_MnE);
        MoneyAndExpList = JsonUtility.FromJson<Serialization<int>>(jdata).target;
        jdata = File.ReadAllText(filePath_pre);
        PrePrice = JsonUtility.FromJson<Serialization<int>>(jdata).target;
        jdata = File.ReadAllText(filePath_now);
        NowPrice = JsonUtility.FromJson<Serialization<int>>(jdata).target;
        jdata = File.ReadAllText(filePath_Amount);
        ItemAmount = JsonUtility.FromJson<Serialization<int>>(jdata).target;
        print(filePath);
    }
    void Save()
    {
        print("저장");
        string jdata = JsonUtility.ToJson(new Serialization<Item>(AllItemList));
        File.WriteAllText(filePath, jdata);
        jdata = JsonUtility.ToJson(new Serialization<int>(MoneyAndExpList));
        File.WriteAllText(filePath_MnE, jdata);
        jdata = JsonUtility.ToJson(new Serialization<int>(PrePrice));
        File.WriteAllText(filePath_pre, jdata);
        jdata = JsonUtility.ToJson(new Serialization<int>(NowPrice));
        File.WriteAllText(filePath_now, jdata);
        jdata = JsonUtility.ToJson(new Serialization<int>(ItemAmount));
        File.WriteAllText(filePath_Amount, jdata);
    }
    public void ResetItemClick()
    {
        Save();
    }
    public void shopin()
    {
        Shop.SetActive(true);
    }
    public void shopout()
    {
        Shop.SetActive(false);
    }

    void setLevel(int exp)
    {
        for(int i = 1; i <= 10; i++)
        {
            if (exp < Explist[i])
            {
                Level_t.text = System.Convert.ToString(i);
                Exp_t.text = System.Convert.ToString(exp);
                Exp_Fill.fillAmount = (float)exp / (float)Explist[i];
                UserLevel = i;
                print("Level : " + UserLevel);
                break;
            }
            else
            {
                exp -= Explist[i];
            }
        }

        if(UserLevel == 10)
        {
            Exp_Fill.fillAmount = 1;
        }
    }
}
