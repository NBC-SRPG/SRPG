using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VersionData
{
    public string version { get; set; }
    public List<int> curGacha { get; set; }
    public int curGachaPoint { get; set; } = 0;
    public List<string> curEvents { get; set; }
}
