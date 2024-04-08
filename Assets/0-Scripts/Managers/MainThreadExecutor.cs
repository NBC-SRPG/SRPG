using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadExecutor : MonoBehaviour
{
    private static readonly Queue<Action> actions = new Queue<Action>();
    private void Update()
    {
        while (actions.Count > 0)
        {
            Action action = null;

            lock (actions)
            {
                if (actions.Count > 0)
                {
                    action = actions.Dequeue();
                }
            }

            action?.Invoke();
        }
    }

    public static void ExecuteInMainThread(Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }
}
