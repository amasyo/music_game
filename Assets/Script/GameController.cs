using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UniRx;
using UniRx.Triggers;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject Notes0;
    [SerializeField] GameObject Notes1;
    [SerializeField] GameObject Notes2;
    [SerializeField] GameObject Notes3;
    [SerializeField] GameObject Notes4;
    [SerializeField] GameObject Notes5;

    private float Timing;
    private int Type;

    public string filepath;

    private AudioSource _audioSource;

    public float timeOffset = 0f;

    public GameObject startButton;

    public Text scoreText;
    private int _score = 0;     // スコア

    public static float notesSpeed = 1.0f;    // ノーツスピード

    float PlayTime;
    float Distance;
    float During;
    bool isPlaying;
    int GoIndex;

    // 判定の範囲
    float PerfectRange;
    float GreatRange;
    float GoodRange;
    float BadRange;

    List<float> NoteTimings;

    List<GameObject> Notes;     // ノーツの配列

    // notesSpeed の受け渡し関数
    public static float get_notesSpeed()
    {
        return notesSpeed;
    }

    void Start()
    {
        _audioSource = GetComponent <AudioSource> ();
        LoadCSV();
    }

    // イベントを通知するサブジェクトを追加
    Subject<string> MessageEffectSubject = new Subject<string>();

    // イベントを検知するオブザーバーを追加
    public IObservable<string> OnMessageEffect
    {
        get { return MessageEffectSubject; }
    }

    void OnEnable()
    {
        Distance = 10.0f - (-3.0f);     // ノーツ生成位置から判定ラインまでの距離
        During = 2000f;       // デフォルトの値
        isPlaying = false;
        GoIndex = 0;

        // 判定範囲の設定
        PerfectRange = 60f;     // Perfectの範囲（+-10msで設定）
        GreatRange = 80f;       // Greatの範囲
        GoodRange = 100f;       // Goodの範囲
        BadRange = 130f;        // Badの範囲

        // ノーツを発射するタイミングかチェック
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > GoIndex)
          .Where(_ => Notes[GoIndex].GetComponent<NotesScript>().getTiming() <= ((Time.time * 1000f - PlayTime) + During + timeOffset))
          .Subscribe(_ => {
              Notes[GoIndex].GetComponent<NotesScript>().go(Distance, During);
              GoIndex++;
          });

        // Sキーが押されたらNotes0の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => {
                beat(0, Time.time * 1000f - PlayTime);
            });

        // Dキーが押されたらNotes1の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ => {
                beat(1, Time.time * 1000f - PlayTime);
            });

        // Fキーが押されたらNotes2の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.F))
            .Subscribe(_ => {
                beat(2, Time.time * 1000f - PlayTime);
            });

        // Jキーが押されたらNotes3の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.J))
            .Subscribe(_ => {
                beat(3, Time.time * 1000f - PlayTime);
            });

        // Kキーが押されたらNotes4の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.K))
            .Subscribe(_ => {
                beat(4, Time.time * 1000f - PlayTime);
            });

        // Lキーが押されたらNotes5の判定
        this.UpdateAsObservable()
            .Where(_ => isPlaying)
            .Where(_ => Input.GetKeyDown(KeyCode.L))
            .Subscribe(_ => {
                beat(5, Time.time * 1000f - PlayTime);
            });
    }

    // 譜面の読み込み・ノーツ生成
    void LoadCSV()
    {
        Notes = new List<GameObject>();
        NoteTimings = new List<float>();

        // 譜面の読み込み
        string csv = File.ReadAllText(filepath);

        StringReader reader = new StringReader(csv);

        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            Timing = float.Parse(values[0]);
            Type = int.Parse(values[1]);

            // ノーツの生成
            GameObject Note;
            if (Type == 0)
            {
                Note = Instantiate(Notes0, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else if (Type == 1)
            {
                Note = Instantiate(Notes1, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else if (Type == 2)
            {
                Note = Instantiate(Notes2, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else if (Type == 3)
            {
                Note = Instantiate(Notes3, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else if (Type == 4)
            {
                Note = Instantiate(Notes4, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else if (Type == 5)
            {
                Note = Instantiate(Notes5, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);
            }
            else
            {
                Note = Instantiate(Notes0, new Vector3(-4.0f + (2.0f * Type), 10.0f, 0f), Quaternion.identity);  // デフォルトで0レーンに生成
                    Type = 0;
            }

            Note.GetComponent<NotesScript>().setParameter(Type, Timing);

            Notes.Add(Note);
            NoteTimings.Add(Timing);
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            scoreText.text = _score.ToString();
        }
    }

    public void StartGame()
    {
        startButton.SetActive(false);

        PlayTime = Time.time * 1000f;
        isPlaying = true;

        notesSpeed = 2.0f;  // ノーツスピードを格納
        if (notesSpeed >= 1.0f && notesSpeed <= 12.0f)      // ノーツスピードが適切な範囲かチェック
        {
            During = -4000f / 9f * notesSpeed + 49000f / 9f;    // ノーツ生成位置から判定ラインまで移動するのにかかる時間（線形的に計算）
        }

        _audioSource.Play();
        isPlaying = true;
    }


    // ノーツの判定機構
    void beat(int type, float timing)
    {
        float minDiff = -1f;
        int minDiffIndex = -1;

        for (int i = 0; i < NoteTimings.Count; i++)
        {
            if (NoteTimings[i] > 0f)
            {
                float diff = Math.Abs(NoteTimings[i] - timing);
                if (minDiff == -1f || minDiff > diff)
                {
                    minDiff = diff;
                    minDiffIndex = i;
                }
            }
        }

        if (minDiff != -1f && Notes[minDiffIndex].GetComponent<NotesScript>().getType() == type)
        {
            if (minDiff <= PerfectRange)
            {
                NoteTimings[minDiffIndex] = -1f;
                Notes[minDiffIndex].GetComponent<NotesScript>().hideMe();

                MessageEffectSubject.OnNext("Perfect");
                _score += 100;
                Debug.Log("Perfect");
            }

            else if (minDiff <= GreatRange)
            {
                NoteTimings[minDiffIndex] = -1f;
                Notes[minDiffIndex].GetComponent<NotesScript>().hideMe();

                MessageEffectSubject.OnNext("Great");
                _score += 70;
                Debug.Log("Great");
            }

            else if (minDiff <= GoodRange)
            {
                NoteTimings[minDiffIndex] = -1f;
                Notes[minDiffIndex].GetComponent<NotesScript>().hideMe();

                MessageEffectSubject.OnNext("Good");
                _score += 40;
                Debug.Log("Good");
            }

            else if (minDiff < BadRange)
            {
                NoteTimings[minDiffIndex] = -1f;
                Notes[minDiffIndex].GetComponent<NotesScript>().hideMe();

                MessageEffectSubject.OnNext("Bad");
                _score += 10;
                Debug.Log("Bad");
            }

            else
            {
                // 仮置き
            }
        }

        else
        {
            // 仮置き
        }
    }
}
