using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static Constants;


public static class Utility
{
    // 비동기 처리 후 호출할 콜백함수
    public delegate void Callback<T>(T result);


    // 타입, id, 콜백함수를 param으로 받아 사용
    public static void Id2SO<T>(int id, Callback<ScriptableObject> callback) where T : ScriptableObject
    {
        // 어드레서블 경로
        string path = typeof(T).ToString() + "/" + typeof(T).ToString() + "_" + id.ToString() + ".asset";

        var op = Addressables.LoadAssetAsync<T>(path);
        op.Completed += (handler) =>
        {
            // 로딩이 완료되면 콜백함수로 로직처리
            callback(handler.Result);
        };
    }

    public static T Id2SOWait<T>(int id) where T : ScriptableObject
    {
        // 어드레서블 경로
        string path = typeof(T).ToString() + "/" + typeof(T).ToString() + "_" + id.ToString() + ".asset";

        T result = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
        return result; 
    }

    public static AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = Addressables.LoadAssetAsync<AudioClip>(path).WaitForCompletion();

        return audioClip;
    }


    //taskAsync


    public static PassiveLogic GetAbilityBySO<T>(T so) where T : PassiveSO
    {
        Type passiveType;

        if(so.reflection != null)
        {
            passiveType = Type.GetType(so.reflection);
        }
        else
        {
            passiveType = null;
        }

        if (passiveType == null)
        {
            return null;
        }

        object obj = Activator.CreateInstance(passiveType);
        PassiveLogic passive = obj as PassiveLogic;

        if (so.coefficients != null)
        {
            passive.coefficient = new Dictionary<string, int>(so.coefficients);
        }

        return passive;
    }

    public static void Stage2SO<T>(string id, Callback<ScriptableObject> callback) where T : ScriptableObject
    {
        // 어드레서블 경로
        string path = "StageSO/Stage" + "_" + id + ".asset";

        var op = Addressables.LoadAssetAsync<T>(path);
        op.Completed += (handler) =>
        {
            // 로딩이 완료되면 콜백함수로 로직처리
            callback(handler.Result);
        };
    }
}
