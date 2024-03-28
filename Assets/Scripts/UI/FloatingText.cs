using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public void Deactivate()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
