using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialShowImage : TutorialBase
{
    [SerializeField] private List<Image> images;
    private int index;

    public override void Enter()
    {
        index = 0;

        images[index].gameObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        if(Input.GetMouseButtonDown(0))
        {
            images[index].gameObject.SetActive(false);
            index++;

            if(index == images.Count)
            {
                controller.SetNextTutorial();
                return;
            }

            images[index].gameObject.SetActive(true);
        }
    }

    public override void Exit()
    {
        foreach (var image in images)
        {
            image.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        foreach(var image in images)
        {
            image.gameObject.SetActive(false);
        }
    }

}
