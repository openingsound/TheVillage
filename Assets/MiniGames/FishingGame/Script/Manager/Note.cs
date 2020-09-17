using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    int a = 0;

    public GameObject _note;

    Random rRan = new Random();

    public GameObject _target;

    public Vector3 notePos;

    TimingManager _timingManager;

    PlayerController _playerController;

    private void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _timingManager = FindObjectOfType<TimingManager>();
        notePos = _note.transform.localPosition;
    }

    private void Update()
    {
       

        if(_note.transform.localPosition.y <= -434)
        {
            a = 1;
        }


       _note.transform.localPosition += new Vector3(0,a*rRan.rSpeed,0)*Time.deltaTime;
    }

    public void aManager(bool k)
    {
        bool kk = k;

        if (kk)
        {
            a = -1;
            kk = false;
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {

            _note.transform.localPosition = notePos;
            a = 0;
            _timingManager.Restart(0);
            _playerController.NRe();
        }
    }

}
