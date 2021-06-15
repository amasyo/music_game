using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVWriter : MonoBehaviour
{
    public string filename;

    private void Start()
    {
        WriteCSV("");
    }

    public void WriteCSV(string txt)
    {
        File.AppendAllText(filename, txt);    
    }
}
