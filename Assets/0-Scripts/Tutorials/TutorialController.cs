using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialBase> tutorials;

    private TutorialBase currentTutorial;
    private int index = -1;

    private BattleUI Ui;

    private void Start()
    {
        SetNextTutorial();

        Ui = Managers.UI.FindUI<BattleUI>();

        Ui.HideGoalText();
    }

    private void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if(index >= tutorials.Count - 1)
        {
            CompletedAllTutorial();
            return;
        }

        index++;
        currentTutorial = tutorials[index];

        currentTutorial.Enter();
    }

    private void CompletedAllTutorial()
    {
        currentTutorial = null;

        Ui.ShowGoalText();
    }
}
