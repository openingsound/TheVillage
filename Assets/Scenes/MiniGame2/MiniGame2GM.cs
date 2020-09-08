using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel.Design.Serialization;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics.Contracts;
using System.Net;
using TMPro;
struct pii { int y; int x; };


public class MiniGame2GM : MonoBehaviour
{
    public GameObject linePrefab;
    public Canvas canvas;
    public int ch;
    public Sprite[] Tabimage;
    public int[,] arr = new int[9, 10];
    public bool[,] vis = new bool[9, 10];
    private int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };
    private int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
    public GameObject Tiles;
    private Dictionary<int,Food> foods;
    public Text Scoretext;
    // Start is called before the first frame update
    new bool enabled = true;
    public GameObject Successpanel;
    public GameObject rewardCanvas;
    public GameObject Failpanel;
    void Start()
    {
        foods = new Dictionary<int, Food>();
        while (true)
        {
            //print("go");
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    int ran = UnityEngine.Random.Range(0, 80);
                    if (ran >= 60)
                    {
                        ran = 0;
                    }
                    else
                    {
                        ran %= 3;
                    }
                    arr[i, j] = ran;
                    vis[i, j] = false;
                }
            }
            bool cycleche = false;
            for (int i = 0; i < 9 && cycleche == false; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (vis[i, j]) continue;
                    if (dfs(i, j, -1, -1))
                    {
                        cycleche = true;
                        break;
                    }
                }
            }
            if (cycleche) {
                for (int i = 0; i < 9 ; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        Food food = Tiles.transform.GetChild(i * 10 + j).GetComponent<Food>();
                        food.Setdepth(1);
                        int ran = arr[i, j];
                        food.SetFood(ran, i * 10 + j, Tabimage[ran]);

                        foods.Add(i * 10 + j, food);
                    }
                }
                //print("end");
                break;
            }
        }
        
    }
    bool dfs(int y,int x,int py,int px)
    {//cycle이 존재하면 true 반환
        vis[y, x] = true;
        for(int i = 0; i < 8; i++)
        {
            int ty = y + dy[i];
            int tx = x + dx[i];
            if (ty == py && tx == px) continue;
            if (ty >= 0 && ty < 9 && tx >= 0 && tx < 10 && arr[ty, tx] == arr[y, x])
            {
                if (vis[ty, tx]) return true;
                if (dfs(ty, tx, y, x)) return true;
            }
        }
        return false;
    }
   
    private List<Food> lines = new List<Food>();
    private bool unlocking;//터치 중
    private GameObject lineOnEdit;//마지막 라인
    private RectTransform lineOnEditRcts;//UI를 위해서 만들어진 transform
    private Food foodOnEdit;

    int py;
    int px;
    int ncou;
    int scou;
    public Slider slider;
    bool activated = true;//시간 체크 확인
    float timeElapse = 0;
    void Update()
    {
        if (activated)
        {
            timeElapse += UnityEngine.Time.deltaTime;
            if(timeElapse >= 3.0f)
            {
                enabled = false;
                Release();
                Failpanel.SetActive(true);
                activated = false;
            }
            slider.value = 3.0f - timeElapse;
            
        }
        if(enabled == false)
        {
            return;
        }
        if (unlocking)
        {
            Vector3 mousePos = canvas.transform.InverseTransformPoint(Input.mousePosition);
            //마우스랑 canvas의 좌표는 기준이 달라서 canvas 위치 역행렬을 곱해줌!
            lineOnEditRcts.sizeDelta = new Vector2(lineOnEditRcts.sizeDelta.x, Vector3.Distance(mousePos, foodOnEdit.transform.localPosition));
            lineOnEditRcts.rotation = Quaternion.FromToRotation(Vector3.up, (mousePos - foodOnEdit.transform.localPosition).normalized);
        }
    }
    void TimeButton()
    {
        activated = !activated;
    }
    bool TrySetLineEdit(Food food)
    {
        if (enabled == false)
        {
            return false;
        }
        foreach (var line in lines)
        {
            if (line.cou == food.cou)
            {
                //print("중복!");
                return false;
            }
        }
        //print("로컬 포지션 : " + food.transform.localPosition);
        lineOnEdit = CreatLine(food.transform.localPosition, food.cou);
        lineOnEditRcts = lineOnEdit.GetComponent<RectTransform>();
        foodOnEdit = food;
        return true;
    }
    GameObject CreatLine(Vector3 pos , int id)
    {
        var line = GameObject.Instantiate(linePrefab, canvas.transform);
        //새로운 lineprefab을 생성 위치 선언
        line.transform.localPosition = pos;
        //print(pos);
        var lineIdf = line.AddComponent<Food>();
        lineIdf.Setdepth(3);//추가함
        lineIdf.cou = id;
        lines.Add(lineIdf);
        return line;
    }
    
    public void OnMouseEnterCircle(Food food)
    {
        //Debug.Log(food.cou + "들어옴");
        if (enabled == false)
        {
            return;
        }
        ncou = food.cou;
        if (unlocking)
        {
            int y = food.cou / 10;
            int x = food.cou % 10;
            //print(py + ", " + px + ", " + y + ", " + x );
            if (Math.Abs(py - y) > 1 || Math.Abs(px - x) > 1) return;
            if (arr[py, px] != arr[y, x]) return;
            lineOnEditRcts.sizeDelta = new Vector2(lineOnEditRcts.sizeDelta.x, Vector3.Distance(foodOnEdit.transform.localPosition, food.transform.localPosition));
            lineOnEditRcts.rotation = Quaternion.FromToRotation(Vector3.up, (food.transform.localPosition - foodOnEdit.transform.localPosition).normalized );
            if (TrySetLineEdit(food))
            {
                py = y;
                px = x; 
            }
        }
    }
    public void OnMouseExitCircle(Food food)
    {
        //Debug.Log(food.cou + "나감");
    }
    public void OnMouseDownCircle(Food food)
    {
        if (enabled == false)
        {
            return;
        }
        //Debug.Log(food.cou + "다운");
        unlocking = true;
        TrySetLineEdit(food);
        py = food.cou / 10;
        px = food.cou % 10;
        scou = food.cou;
    }
    int linesiz = 0;
    public void OnMouseUpCircle(Food food)
    {
        if (enabled == false)
        {
            return;
        }
        //Debug.Log(food.cou + "업");
        unlocking = false;

        //print("손가락 땜" + food.cou);
        int y = food.cou / 10;
        int x = food.cou % 10;
        if (food.cou == ncou && (Math.Abs(py - y) > 1 || Math.Abs(px - x) > 1) == false && arr[py, px] == arr[y, x] && lines.Count >= 3)
        {
            //완성 했따리
            activated = !activated;
            enabled = false;
            lineOnEditRcts.sizeDelta = new Vector2(lineOnEditRcts.sizeDelta.x, Vector3.Distance(foodOnEdit.transform.localPosition, food.transform.localPosition));
            lineOnEditRcts.rotation = Quaternion.FromToRotation(Vector3.up, (food.transform.localPosition - foodOnEdit.transform.localPosition).normalized);
            CreatLine(food.transform.localPosition, food.cou);
            Destroy(lines[lines.Count - 1].gameObject);
            lines.RemoveAt(lines.Count - 1);
            linesiz = lines.Count;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    vis[i, j] = false;
                }
            }
            foreach (var line in lines)
            {
                int ty = line.cou / 10;
                int tx = line.cou % 10;
                vis[ty,tx] = true;
            }
            for (int i = 0; i < 9; i++){
                for (int j = 0; j < 10; j++){
                    if(i==0 || i == 8 || j == 0 || j == 9)
                    {
                        if (vis[i, j]) continue;
                        dfs2(i, j);
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (vis[i, j] == false) lines.Add(foods[i * 10 + j]);
                }
            }
            StartCoroutine(GetScore());
            
        }
        else
        {
            Release();
        }
    }
    IEnumerator GetScore()
    {
        int score = 0;
        foreach (var line in lines)
        {
            //print("들어왔다 ");
            score++;
            EnableColorFade(foods[line.cou].gameObject.GetComponent<Animator>());
            yield return new WaitForSeconds(0.2f);
            print(score);
            Scoretext.text = score.ToString();
            int rnum;
            rnum = UnityEngine.Random.Range(1,7);
            if (score % 2==1) rnum = -rnum;
            Scoretext.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rnum));
            int tem = Scoretext.fontSize;
            if (score % 2 == 0)
                Scoretext.fontSize = tem + 1;
        }
        yield return new WaitForSeconds(0.2f);
        int cou = 0;
        foreach (var line in lines)
        {
            cou++;
            if (cou > linesiz) break;
            Destroy(line.gameObject);
        }
        lines.Clear();
        lineOnEdit = null;
        lineOnEditRcts = null;
        foodOnEdit = null;
        rewardCanvas.SetActive(true);
    }
    void dfs2(int y,int x)
    {
        vis[y, x] = true;
        for(int i = 1; i < 8; i += 2)
        {
            int ty = y + dy[i];
            int tx = x + dx[i];
            if (ty >= 0 && ty < 9 && tx >= 0 && tx < 10 && vis[ty,tx]==false)
            {
                dfs2(ty, tx);
            }
        }
    }
    void EnableColorFade(Animator anim)
    {
        anim.enabled = true;
        anim.Rebind();
    }
    void Release()
    {
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        lines.Clear();
        lineOnEdit = null;
        lineOnEditRcts = null;
        foodOnEdit = null;
    }
}
