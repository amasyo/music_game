using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour
{
    public GameObject[] notes;
    private float[] _timings;
    private int[] _lineNums;

    public string filepath;
    private int _notesCount = 0;

    private AudioSource _audioSource;
    private float _startTime = 0;

    public float timeOffset;

    private bool isPlaying = false;
    public GameObject startButton;

    public Text scoreText;
    private int _score = 0;

    void Start()
    {
        _audioSource = GetComponent <AudioSource> ();
        _timings = new float[1024];
        _lineNums = new int[1024];
        LoadCSV();
    }

    void LoadCSV()
    {

        string csv = File.ReadAllText(filepath);
        Debug.Log(csv);

        StringReader reader = new StringReader(csv);

        int i = 0;

        while (reader.Peek() >= 0)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

                for (int j =0; j < values.Length; j++)
            {
                _timings[i] = float.Parse(values[0]);
                _lineNums[i] = int.Parse(values[1]);
            }
            i++;
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            CheckNextNotes();
            scoreText.text = _score.ToString();
        }
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        _startTime = Time.time;
        _audioSource.Play();
        isPlaying = true;
    }

    void CheckNextNotes()
    {
        while (_timings[_notesCount] + timeOffset < GetGameTime() && _timings[_notesCount] != 0)
        {
            SpawnNotes(_lineNums[_notesCount]);
            _notesCount++;
        }
    }
    void SpawnNotes(int num)
    {
        Instantiate(notes[num], new Vector3(-4.0f + (2.0f * num), 10.0f, 0), Quaternion.identity);
    }

    float GetGameTime()
    {
        return Time.time - _startTime;
    }
    public void GoodTimingFunc(int num)
    {
        Debug.Log("Line" + num + "good");
        Debug.Log(GetGameTime());

        _score++;
    }
}
