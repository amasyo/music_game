using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class NotesScript : MonoBehaviour

{
    private GameController _gameController;
    private KeyCode _lineKey;

    int Type;
    float Timing;

    float Distance;
    float During;

    Vector3 firstPos;
    bool isGo;
    float GoTime;

    // ノーツ移動
    void OnEnable()
    {
        isGo = false;
        firstPos = this.transform.position;

        this.UpdateAsObservable()
          .Where(_ => isGo)
          .Subscribe(_ => {
              this.gameObject.transform.position = new Vector3(firstPos.x, firstPos.y - Distance * (Time.time * 1000 - GoTime) / During, firstPos.z);
          });
    }

    public void setParameter(int type, float timing)
    {
        Type = type;
        Timing = timing;
    }

    public int getType()
    {
        return Type;
    }

    public float getTiming()
    {
        return Timing;
    }

    public void go(float distance, float during)
    {
        Distance = distance;
        During = during;
        GoTime = Time.time * 1000;

        isGo = true;
    }

    private void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _lineKey = Gameutil.GetKeyCodeByLineNum(Type);
    }
    void Update()
    {
        // ここが判定機構
        if (transform.position.y > -4.0f && transform.position.y < -2.0f)
        {
            CheckInput(_lineKey);
        }
    }

    void CheckInput(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            _gameController.GoodTimingFunc(Type);
            Destroy(gameObject);
        }
    }
}
