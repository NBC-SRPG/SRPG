using UnityEngine;

public class MainScene : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.ShowUI<MainUI>();
        Managers.UI.ShowUI<CommonUI>();
        Debug.Log("MainSceneInit");
    }
}
