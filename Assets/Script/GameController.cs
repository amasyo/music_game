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

    public GameObject[] notes;
    private float[] _timings;
    private int[] _type;

    public string filepath;

    private AudioSource _audioSource;

    public float timeOffset = 0f;

    public GameObject startButton;

    public Text scoreText;
    private int _score = 0;

    public static float notesSpeed = 1.0f;    // ノーツスピード

    //　ノーツを移動のための変数
    float PlayTime;
    float Distance;
    float During;
    bool isPlaying;
    int GoIndex;

    List<GameObject> Notes;     // ノーツの配列

    // notesSpeed の受け渡し関数
    public static float get_notesSpeed()
    {
        return notesSpeed;
    }

    void Start()
    {
        _audioSource = GetComponent <AudioSource> ();
        _timings = new float[2048];
        _type = new int[2048];
        LoadCSV();
    }

    void OnEnable()
    {
        Distance = 10.0f - (-3.0f);     // ノーツ生成位置から判定ラインまでの距離
        During = 2000.0f;       // デフォルトの値
        isPlaying = false;
        GoIndex = 0;

        // ノーツを発射するタイミングかチェック
        this.UpdateAsObservable()
          .Where(_ => isPlaying)
          .Where(_ => Notes.Count > GoIndex)
          .Where(_ => Notes[GoIndex].GetComponent<NotesScript>().getTiming() <= ((Time.time * 1000 - PlayTime) + During + timeOffset))
          .Subscribe(_ => {
              Notes[GoIndex].GetComponent<NotesScript>().go(Distance, During);
              GoIndex++;
          });
    }

    // 譜面の読み込み・ノーツ生成
    void LoadCSV()
    {
        Notes = new List<GameObject>();

        // 譜面の読み込み
        string csv = File.ReadAllText(filepath);

        StringReader reader = new StringReader(csv);

        int i = 0;

        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

                for (int j =0; j < values.Length; j++)
            {
                _timings[i] = float.Parse(values[0]);
                _type[i] = int.Parse(values[1]);

                // ノーツの生成
                GameObject Note;
                if (_type[i] == 0)
                {
                    Note = Instantiate(Notes0, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else if (_type[i] == 1)
                {
                    Note = Instantiate(Notes1, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else if (_type[i] == 2)
                {
                    Note = Instantiate(Notes2, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else if (_type[i] == 3)
                {
                    Note = Instantiate(Notes3, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else if (_type[i] == 4)
                {
                    Note = Instantiate(Notes4, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else if (_type[i] == 5)
                {
                    Note = Instantiate(Notes5, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);
                }
                else
                {
                    Note = Instantiate(Notes0, new Vector3(-4.0f + (2.0f * _type[i]), 10.0f, 0), Quaternion.identity);  // デフォルトで0レーンに生成
                }

                Note.GetComponent<NotesScript>().setParameter(_type[i], _timings[i]);

                Notes.Add(Note);
            }
            i++;
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

        PlayTime = Time.time * 1000;
        isPlaying = true;

        notesSpeed = 9.0f;  // ノーツスピードを格納
        if (notesSpeed >= 1.0f && notesSpeed <= 12.0f)      // ノーツスピードが適切な範囲かチェック
        {
            During = -4000.0f / 9.0f * notesSpeed + 49000.0f / 9.0f;    // ノーツ生成位置から判定ラインまで移動するのにかかる時間（線形的に計算）
        }

        _audioSource.Play();
        isPlaying = true;
        Debug.Log(Time.time * 1000);
    }

    public void GoodTimingFunc(int num)
    {
        Debug.Log("Line" + num + "good");

        _score++;
    }
}
