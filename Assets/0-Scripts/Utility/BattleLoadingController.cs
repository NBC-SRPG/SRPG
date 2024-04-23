using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleLoadingController : MonoBehaviour
{
    static string nextScene;

    public static void LoadBattle(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("BattleLoadingScene");
    }

    private void Start()
    {
        StartCoroutine(LoadBattleSceneProcess());
    }

    private IEnumerator LoadBattleSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float time = 0;
        bool waitSO = false;

        Utility.Stage2SO<StageSO>(Managers.GameManager.stageName, (result) =>
        {
            Managers.GameManager.thisStage = (StageSO)result;
            waitSO = true;
        });

        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9)
            {

            }
            else
            {
                time += Time.unscaledDeltaTime;
                if(time >= 1 && waitSO)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
