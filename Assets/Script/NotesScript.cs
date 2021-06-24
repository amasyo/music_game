using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UniRx.Triggers;

public class NotesScript : MonoBehaviour

{
    int Type;
    float Timing;

    float Distance;
    float During;

    Vector3 firstPos;
    bool isGo;
    float GoTime;

    bool Hide;

    // ノーツ移動
    void OnEnable()
    {
        isGo = false;
        firstPos = this.transform.position;

        Hide = false;

        this.UpdateAsObservable()
          .Where(_ => isGo)
          .Subscribe(_ => {
              this.gameObject.transform.position = new Vector3(firstPos.x, firstPos.y - Distance * (Time.time * 1000f - GoTime) / During, firstPos.z);
          });

        // ノーツが画面外に出たらMiss判定にする
        this.UpdateAsObservable()
          .Where(_ => this.gameObject.transform.position.y < -5.0f )
          .Subscribe(_ => {
              hideMe();
              Debug.Log("Miss");
          });

        this.UpdateAsObservable()
          .Where(_ => Hide)
          .Subscribe(_ => {
              this.gameObject.SetActive(false);
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
        GoTime = Time.time * 1000f;

        isGo = true;
    }

    public void hideMe()
    {
        Hide = true;
    }
}
