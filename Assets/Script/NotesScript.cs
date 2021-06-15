using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesScript : MonoBehaviour
{
    public int lineNum;
    private GameController _gameController;
    private bool isInLine = false;
    private KeyCode _lineKey;


    private void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
    void Update()
    {
       transform.position += Vector3.down * 10.0f * Time.deltaTime;

        if (transform.position.y < -3.0f)
        {
            Debug.Log("false");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInLine = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInLine = false;
    }

    void CheckInput(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            _gameController.GoodTimingFunc(lineNum);
            Destroy(gameObject);
        }
    }
}
