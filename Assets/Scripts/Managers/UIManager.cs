using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Diagnostics;
public class UIManager
{
    private int order = -20;
    private Stack<UIBase> uiStack = new Stack<UIBase>();
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
            {
                root = new GameObject { name = "@UI_Root" };
            }

            return root;
        }
    }
    public void Init()
    {

    }
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = order;
            order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    // UI 동적 생성
    public T ShowUI<T>(string name = null, Transform parent = null) where T : UIBase
    {
        // 이름이 없다면 타입을 이름으로 사용
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }
        // UI 생성
        GameObject go = Managers.Resource.Instantiate($"UI/{name}");
        // 컴포넌트를
        T ui = go.GetComponent<T>();
        // UI 스택에 넣기
        uiStack.Push(ui);
        // 부모 설정이 되어있다면
        if (parent != null)
        {
            // 해당 부모로 설정
            go.transform.SetParent(parent);
        }
        else
        {
            // 없다면 @UI_Root로 설정
            go.transform.SetParent(Root.transform);
        }
        return ui;
    }
    // UI 스택에서 T타입의 UI를 return
    public T FindUI<T>() where T : UIBase
    {
        foreach (var item in uiStack)
        {
            if (item is T)
            {
                return item as T;
            }
        }
        return null;
    }
    // UI스택의 가장 위에 있는 UI return
    public T PeekUI<T>() where T : UIBase
    {
        // 스택이 비어있으면 null return
        if (uiStack.Count == 0)
        {
            return null;
        }
        // 스택의 가장 위 UI와 T의 타입이 맞지 않으면 null return
        return uiStack.Peek() as T;
    }
    // UI 스택의 가장 위에 있는 UI 닫기
    public void CloseUI(UIBase closeUi)
    {
        // 스택이 비어있으면 return
        if (uiStack.Count == 0)
        {
            return;
        }
        // 가장 위에 있는 UI가 닫으려는 UI와 다르다면 Fail로그 출력 후 return
        if (uiStack.Peek() != closeUi)
        {
            Debug.Log("Close ui Failed!");
            return;
        }

        // UI 스택에서 Pop & Destroy
        UIBase destroyUi = uiStack.Pop();
        Object.Destroy(destroyUi.gameObject);
    }
}
