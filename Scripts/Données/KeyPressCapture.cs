using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class KeyPressCapture : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/keyPressData.txt";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    void Update()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                string keyPress = "Key pressed: " + keyCode.ToString();
                Debug.Log(keyPress);
                AppendToFile(keyPress);
            }
        }
    }

    void AppendToFile(string text)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(text);
        }
    }
}