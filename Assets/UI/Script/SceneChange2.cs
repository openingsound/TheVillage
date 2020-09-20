using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    public void Option2()
    {
        InGameManager.inGameManager.Json.OnCreateJson();

        SceneManager.LoadScene("SettingsWindow");
    }

    public void Forest2()
    {
        InGameManager.inGameManager.Json.OnCreateJson();

        SceneManager.LoadScene("forestminigame___");
    }
    public void toGame2()
    {
        SceneManager.LoadScene("InGame");
    }

    public void toMiniGame2_1()
    {
        InGameManager.inGameManager.Json.OnCreateJson();

        SceneManager.LoadScene("MiniGame2");
    }

    public void toFishGame()
    {
        InGameManager.inGameManager.Json.OnCreateJson();

        SceneManager.LoadScene("fish");
    }
}
