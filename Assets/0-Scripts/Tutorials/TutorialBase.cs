using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialBase : MonoBehaviour
{
    //튜툐리얼 진입 시
    public abstract void Enter();

    // 튜툐리얼 진행 중
    public abstract void Execute(TutorialController controller);

    //튜툐리얼 종료 시
    public abstract void Exit();
}
