using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    public void Option2()
    {
        SceneManager.LoadScene("SettingsWindow");
    }

    public void Forest2()
    {
        SceneManager.LoadScene("forestminigame___");
    }
    public void toGame2()
    {
        SceneManager.LoadScene("HomeScreen_2");
    }

    public void toMiniGame2_1()
    {
        SceneManager.LoadScene("MiniGame2");
    }

    public void toFishGame()
    {
        SceneManager.LoadScene("fish");
    }
}
