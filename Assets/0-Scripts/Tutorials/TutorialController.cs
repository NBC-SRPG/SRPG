using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialBase> tutorials;

    private TutorialBase currentTutorial;
    private int index = -1;

    private void Start()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if(currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    private void SetNextTutorial()
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
    }
}
