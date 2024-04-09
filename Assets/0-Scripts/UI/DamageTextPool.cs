using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextPool : MonoBehaviour
{
    [System.Serializable]
    public struct pool
    {
        public string tag;
        public GameObject prefabs;
        public int size;
    }

    public List<pool> textPool = new List<pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in textPool)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject text = Instantiate(pool.prefabs, transform);
                text.gameObject.SetActive(false);
                objectPool.Enqueue(text);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetText(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject text = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(text);

        return text;
    }
}
