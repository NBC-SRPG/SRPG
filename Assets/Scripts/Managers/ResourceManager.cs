using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    // Sprite 캐싱 딕셔너리
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

    public void Init()
    {
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(Sprite))
        {
            // 캐싱 값 존재 시 return
            if (_sprites.TryGetValue(path, out Sprite sprite))
            {
                return sprite as T;
            }
            
            // 없으면 Load 후 return
            Sprite sp = Resources.Load<Sprite>($"Sprites/{path}");
            // 딕셔너리 추가
            _sprites.Add(path, sp);
            return sp as T;
        }

        // 이외 타입들은 바로 return
        return Resources.Load<T>(path);
    }

    // Load & Prefabs 경로 생략 함수
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        return Instantiate(prefab, parent);
    }

    // 프리팹 생성 시 이름 뒤 (Clone) 삭제
    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }
}