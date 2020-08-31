using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    int a = 0;

    public GameObject _note;

    Random rRan = new Random();

    public GameObject _target;

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

    public void Spawn()
    {
       GameObject n_note = Instantiate(_note,new Vector3(-820,320,0),Quaternion.identity);

        n_note.transform.SetParent(_note.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Note"))
        {

           _note.transform.position = _target.transform.position;
            a = 0;
            
        }
    }

}
