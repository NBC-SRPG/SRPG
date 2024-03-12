// UI 관련 스크립트에서 상속하기 위한 Base 클래스
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    // UI 캐싱용 딕셔너리
    private Dictionary<Type, UnityEngine.Object[]> objectsDic = new Dictionary<Type, UnityEngine.Object[]>();

    // 바인딩 할 오브젝트의 이름을 Enum타입으로 받아와 딕셔너리에 저장
    private void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        objectsDic.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = FindChild(gameObject, names[i], true);
            }
            else
            {
                objects[i] = FindChild<T>(gameObject, names[i], true);
            }

            if (objects[i] == null)
            {
                Debug.Log($"Failed to bind({names[i]})");
            }
        }
    }
    // UIBase를 상속 받는 UI 스크립트에서 사용 할 Bind 함수들
    protected void BindObject(Type type) 
    {
        Bind<GameObject>(type); 
    }

    protected void BindImage(Type type)
    {
        Bind<Image>(type); 
    }

    protected void BindText(Type type) 
    {
        Bind<TMP_Text>(type); 
    }

    protected void BindButton(Type type) 
    {
        Bind<Button>(type); 
    }
    // 캐싱 딕셔너리에서 T타입의 인덱스에 해당하는 값을 반환
    // Enum타입으로 저장하였으므로 Enum타입의 값을 int로 변환하여 사용 -> 가독성
    private T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (objectsDic.TryGetValue(typeof(T), out objects) == false)
        {
            return null;
        }

        return objects[idx] as T;
    }
    // UIBase를 상속 받는 UI 스크립트에서 사용 할 Get 함수들
    protected GameObject GetObject(int idx) 
    {
        return Get<GameObject>(idx); 
    }

    protected TMP_Text GetText(int idx) 
    {
        return Get<TMP_Text>(idx); 
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx); 
    }

    protected Image GetImage(int idx)
    {
        return Get<Image>(idx); 
    }

    // Find 함수들은 타 스크립트에서 필요 시 Utils로 이동
    // go 오브젝트의 자식들 중 이름이 name인 오브젝트의 T 컴포넌트 반환
    private T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
        {
            return null;
        }
        // recursive == false인 경우 자식 오브젝트만 검사
        if (recursive == false)
        {
            // GameObject.Find -> 씬 모두 검사, transform.Find -> 자식들만 검사
            Transform transform = go.transform.Find(name);
            if (transform != null)
            {
                return transform.GetComponent<T>();
            }
        }
        // recursive == true인 경우 모든 하위 오브젝트 검사
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }
    // GameObject Find의 경우 Transform으로 바꾸어 Find 후 GameObject로 반환
    private GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform != null)
        {
            return transform.gameObject;
        }
        return null;
    }
}
