using System.Collections.Generic;
using UnityEngine;
using static Constants;


[CreateAssetMenu(menuName = "ChapterData", fileName = "ChapterSO_")]
public class ChapterSO : ScriptableObject
{
    [Header("Develope")]
    public int id;

    [Header("Description")]
    public string chapterName;
    public string description;
    public Sprite chapterImage;

    public List<int> stageList;
}
