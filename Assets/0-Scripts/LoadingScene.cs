using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.ShowUI<DownloadUI>();
        Debug.Log("LoadingSceneInit");
    }
}
