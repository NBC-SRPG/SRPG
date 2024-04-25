using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private enum Buttons
    {
        BackImage
    }
    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Database.OnLoadingProgressChanged -= UpdateProgress;
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
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        Database.OnLoadingProgressChanged += UpdateProgress;

        GetButton((int)Buttons.BackImage).onClick.AddListener(OnClickStart);

        //StartCoroutine(Managers.DB.DataLoad());
        //StartCoroutine(LoadAllData());
    }

    private void OnClickStart()
    {
        StartCoroutine(Managers.DB.DataLoad());
    }

    private void UpdateProgress(float progress)
    {
        GetImage((int)Images.ProgressBar).fillAmount += progress;
        int progressPercentage = Mathf.FloorToInt(GetImage((int)Images.ProgressBar).fillAmount * 100);
        GetText((int)Texts.StatusText).text = $"로딩 중... ({progressPercentage}%)";

        if (progressPercentage >= 100)
        {
            isDataLoaded = true;
            GetText((int)Texts.StatusText).text = "로딩 완료! 화면을 클릭하여 시작하세요.";
        }
    }

    /*
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
    */
}
