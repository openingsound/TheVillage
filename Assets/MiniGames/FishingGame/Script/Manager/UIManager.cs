using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
  
    TimingManager _timingManager;
    
     public Text _gameOverText;

    public GameObject coin;

    public GameObject fail;

    public int n;

    private void Start()
    {
        coin.SetActive(false);

        fail.SetActive(false);
    }


    public void Success()
    {
        coin.SetActive(true);
        Invoke("Off",2f);
    }

    public void Fail()
    {
        fail.SetActive(true);
        Invoke("Off",1.5f);
    }
    
    void Off()
    {
        coin.SetActive(false);
        fail.SetActive(false);
    }

    private void Update()
    {
        _gameOverText.text = "Game Over\n획득 골드: ";
    }
}
