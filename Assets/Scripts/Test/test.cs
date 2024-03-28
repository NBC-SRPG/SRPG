using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

[Serializable]
public class test : MonoBehaviour
{
    public Text text;
    private List<int> list;

    private string str = "miku";

    void Start()
    {
        list = new List<int>();
        list.Add(1);
        list.Add(3);
    }
    
    public void OnClick()
    {
        string json = JsonConvert.SerializeObject(str);
        
        foreach(int i in list)
        {
            Debug.Log(i);
        }

        Debug.Log(json);
        text.text = json;
    }

}

