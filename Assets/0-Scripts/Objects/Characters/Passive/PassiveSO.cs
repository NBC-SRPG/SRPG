using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PassiveSO : SerializedScriptableObject
{
    public Dictionary<string, int> coefficients;
    public string reflection;
}
