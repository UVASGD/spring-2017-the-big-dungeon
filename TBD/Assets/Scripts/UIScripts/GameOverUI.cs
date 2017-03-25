using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    public bool isActive = false;
    private PlayerController player;
    private SaveController save;
    ScreenFader sf;
    private GameObject arrow;
    public Vector2 offset = new Vector2(-400f, 130f);
    private int index = 0;

    // Use this for initialization
    void Start () {
        Debug.Log("Start");
        player = FindObjectOfType<PlayerController>();
        save = FindObjectOfType<SaveController>();
        gameObject.SetActive(isActive);
        arrow = gameObject.GetComponentInChildren<Animator>().gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!player.alive && !isActive)
        {
            isActive = true;
            gameObject.SetActive(isActive);
            player.frozen = true;
            Debug.Log("Death Screen");
            if (sf != null)
            {
                sf.BlackOut();
            }
        }

        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if(index == 1)
                {
                    index--;
                    arrow.GetComponent<RectTransform>().anchoredPosition += offset;
                }
            }

            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (index == 0)
                {
                    index++;
                    arrow.GetComponent<RectTransform>().anchoredPosition -= offset;
                }
            }

            else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (index)
                {
				case 0:
						save = FindObjectOfType<SaveController> ();
                        save.LoadFromSlot(save.getCurrentSlot());
                        gameObject.SetActive(false);
                        player.alive = true;
                        player.frozen = false;
                        break;
                    case 1:
                        exitConfirm();
                        break;
                }
            }
        }
	}

    public void exitConfirm()
    {
        // Could break a lot of stuff switching between scenes
        SceneManager.LoadScene(0);
        gameObject.SetActive(false);
    }

    public void setActive()
    {
        if (!player.alive && !isActive)
        {
            isActive = true;
            gameObject.SetActive(isActive);
            player.frozen = true;
            Debug.Log("Death Screen");
            if (sf != null)
            {
                sf.BlackOut();
            }
        }
    }
}
