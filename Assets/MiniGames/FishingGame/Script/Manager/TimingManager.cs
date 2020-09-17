using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class TimingManager : MonoBehaviour
{
    [SerializeField] Transform Center;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    TimeManager _timeManager;
    public List<int> MoneyAndExpList;
    public Text rewardMoeny;
    int winT = 0; //성공 횟수 정수

   public GameObject _timing;

   ppo _ppo;
    
    public int drawNum = 0;
   public int num = 0;
   
    bool reStart = false;
  
    PlayerController _playerController;

    Note _note;
    
    Character _character;
    bool boly = true;

    UIManager uIManager;
    void Start()
    {
        _timing = GameObject.FindWithTag("Timing");

        Center = _timing.transform;

        _note = FindObjectOfType<Note>();
      
        _playerController = FindObjectOfType<PlayerController>();

        _ppo = FindObjectOfType<ppo>();

        uIManager = FindObjectOfType<UIManager>();

        _character = FindObjectOfType<Character>();

        _timeManager = FindObjectOfType<TimeManager>();

        timingBoxs = new Vector2[timingRect.Length];
        
        for(int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(Center.localPosition.y - timingRect[i].rect.height / 2
                , Center.localPosition.y + timingRect[i].rect.height / 2);
        }
        loadMnE();
    }

    void loadMnE()
    {
        string filePath_MnE = Application.persistentDataPath + "/MoneyAndExp.txt";
        string jdata = File.ReadAllText(filePath_MnE);
        MoneyAndExpList = JsonUtility.FromJson<Serialization<int>>(jdata).target;
    }
    void writeMnE(int good)
    {
        string filePath_MnE = Application.persistentDataPath + "/MoneyAndExp.txt";
        print(filePath_MnE);
        int userlevel = MoneyAndExpList[2];//레벨 저장
        int UPMoney = 10 + userlevel * good;
        MoneyAndExpList[0] += UPMoney;
        rewardMoeny.text = System.Convert.ToString(UPMoney);
        string jdata = JsonUtility.ToJson(new Serialization<int>(MoneyAndExpList));
        File.WriteAllText(filePath_MnE, jdata);
    }



    public void CheckTiming()
    {
        

        for(int i = 0; i< timingBoxs.Length; i++)
        {
            float _posY = transform.localPosition.y;

            if(_posY <= timingBoxs[i].y && _posY >= timingBoxs[i].x ){
                num++;
                Debug.Log("HIt");

                _ppo.ppo0();

                Random rD = new Random();

               

                if(num == 1) { 
                
                    drawNum += rD.rDraw;
                    OnDraw();
                }
                if(num > 1)
                {
                    writeMnE(3);
                    drawNum += rD.rDraw;
                   
                    OnDraw();
                    
                    reStart = true;

                    boly = false;
                   
                    _character.boly(boly);
                }
                return;
        }

        }

       _ppo.dro0();

        num++;
        Debug.Log("MISS");
        if(num > 1)
                {
                     boly = false;
                    _character.boly(boly);
                    
                    OnDraw();
             reStart = true;
           
                }   
        
        }



    public void OnDraw()
    {

        _timeManager.GameOver(winT);

        if (num > 1)
        {

            Random rD = new Random();

            int dD = rD.rNum;

            if (drawNum != 0)
            {

                Debug.Log(dD);

                if (drawNum >= dD)
                {
                    uIManager.Success();
                    winT++; //성공 횟수 증가

                }
                else
                {

                    uIManager.Fail();


                }


            }
            else
            {
                uIManager.Fail();
                Debug.Log("FAil");

            }
        }
    }

    private void Update()
    {
        

        if (reStart)
        {
            Debug.Log(drawNum);

            

            drawNum = 0;

            num = 0;

            reStart = false;
          
        }

         
     
        
    }

    public void Restart(int a)
    {
        drawNum = a;
        num = a;
        
    }
}