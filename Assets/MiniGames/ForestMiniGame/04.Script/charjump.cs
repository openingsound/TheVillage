using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class charjump : MonoBehaviour
{
    public GameObject rewardCanvas;
    public AudioSource tapAudio;
    public AudioSource winAudio;
    public AudioSource collideAudio;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(16, 9, false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    //버튼 누르면 점프
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<Rigidbody>().AddForce(0, 300, 0);

            tapAudio.Play();

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("forestminigame___");
        }
    }
    //캐릭터 충돌했을 때 게임오버
    void OnCollisionEnter(Collision coll)
    {
     
            collideAudio.Play();
            rewardCanvas.SetActive(true);
            Debug.Log("GameOver");
            Time.timeScale = 0;
            gameObject.GetComponent<Animator>().Play("die");
        
    }

    public void Replay()
    {
        SceneManager.LoadScene("forestminigame___");
    }

    //동전을 먹을경우 보상
    private void OnTriggerEnter (Collider collision)
    {
        if(collision.CompareTag("ScorePlus"))
        {
            winAudio.Play();
            rewardCanvas.SetActive(true);
            Debug.Log("Player wins");
            Time.timeScale = 0;
            gameObject.GetComponent<Animator>().Play("win");
        }
    }

}
