using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    public Test_Json saveManager;

    public void Option2()
    {
        Save();

        SceneManager.LoadScene("SettingsWindow");
    }

    public void Forest2()
    {
        Save();

        SceneManager.LoadScene("forestminigame___");
    }
    public void toGame2()
    {
        SceneManager.LoadScene("InGame");
    }

    public void toMiniGame2_1()
    {
        Save();

        SceneManager.LoadScene("MiniGame2");
    }

    public void toFishGame()
    {
        Save();

        SceneManager.LoadScene("fish");
    }

    private void Save()
    {
        // 플레이어의 마지막 접속 시간을 현재로 변경
        if (saveManager.player != null)
        {
            saveManager.player.lastConnectTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Json 파일을 생성 및 쓰기
        saveManager.OnCreateJson();
    }
}
