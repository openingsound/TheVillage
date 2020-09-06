using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange1 : MonoBehaviour
{
    public void Option1()
    {
        SceneManager.LoadScene("SettingsWindow");
    }

    public void Forest1()
    {
        SceneManager.LoadScene("forestminigame___");
    }

    public void toMiniGame2_2()
    {
        SceneManager.LoadScene("MiniGame2");
    }

    public void toGame1()
    {
        SceneManager.LoadScene("HomeScreen_2");
    }
}
