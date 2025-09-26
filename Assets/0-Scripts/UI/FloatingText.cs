using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private BattleUI Ui;

    public void Start()
    {
        Ui = Managers.UI.FindUI<BattleUI>();
    }

    public void Deactivate()
    {
        Ui.ResetTextTransform(this.transform.parent);
        transform.parent.gameObject.SetActive(false);
    }
}
