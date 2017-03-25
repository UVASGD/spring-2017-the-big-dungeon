using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private bool PlayerTouching = false;
    private float speed = 1.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || (Input.GetKey(KeyCode.W))) && (PlayerTouching))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D))) && (PlayerTouching))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A))) && (PlayerTouching))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.DownArrow) || (Input.GetKey(KeyCode.S))) && (PlayerTouching))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player is now colliding");
            PlayerTouching = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerTouching = false;
        }
    }
}




