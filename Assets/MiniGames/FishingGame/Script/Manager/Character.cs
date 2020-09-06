using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Animator ani;
    PlayerController _playerController;
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        ani = GetComponent<Animator>();
    }

    public void boly(bool k) { 
           bool n = ani.GetBool("1");
        n= k;
            ani.SetBool("1",n);
    }

}
