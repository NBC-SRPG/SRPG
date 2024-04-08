using System;
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
        string path = typeof(T).ToString() + "/" + id.ToString() + ".asset";

        var op = Addressables.LoadAssetAsync<T>(path);
        op.Completed += (handler) =>
        {
            // 로딩이 완료되면 콜백함수로 로직처리
            callback(handler.Result);
        };
    }

    //taskAsync


    public static PassiveSkillBase GetAbilityBySO<T>(T so) where T : ReflectionableSO
    {
        Type passiveType = Type.GetType(so.reflection);

        if (passiveType == null)
        {
            return null;
        }

        object obj = Activator.CreateInstance(passiveType);
        PassiveSkillBase passive = obj as PassiveSkillBase;
        passive.coefficient = so.coefficients.ToList();

        return passive;
    }

}
