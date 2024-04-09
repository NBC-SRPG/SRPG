using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingUI : UIBase
{
    private bool isDataLoaded = false;

    private enum Texts
    {
        StatusText
    }

    private enum Images
    {
        ProgressBar
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (isDataLoaded && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        StartCoroutine(LoadAllData());
    }

    private IEnumerator LoadAllData()
    {
        string[] dataPaths = { "stageClearData", "characterData", "playerData", "friendData", "formationData", "versionData", "gachaPoint", "mailBox", "missionData" };
        int totalDataCount = dataPaths.Length;
        int loadedDataCount = 0;

        foreach (var path in dataPaths)
        {
            yield return StartCoroutine(ReadDataFromFirebase(path, () =>
            {
                loadedDataCount++;
                UpdateProgress(loadedDataCount, totalDataCount);
                if (loadedDataCount == totalDataCount)
                {
                    isDataLoaded = true;

                    GetText((int)Texts.StatusText).text = "로딩 완료! 화면을 클릭하여 시작하세요.";
                }
            }));
        }
    }

    private void UpdateProgress(int loaded, int total)
    {
        float progress = (float)loaded / total;
        GetImage((int)Images.ProgressBar).fillAmount = progress;
        GetText((int)Texts.StatusText).text = $"로딩 중... ({progress * 100:F0}%)";
    }

    private IEnumerator ReadDataFromFirebase(string path, Action onComplete)
    {
        var task = Managers.DB.userDB.Child(path).GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}
