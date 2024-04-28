using UnityEngine;

public class EnemyInfoUI : UIBase
{
    private enum Texts
    {
        StageText
    }

    private enum Buttons
    {
        BackImage,
        CloseButton
    }

    private enum GameObjects
    {
        EnemyInfo
    }

    public void Init(StageSO stage)
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetText((int)Texts.StageText).text = stage.stageName;

        foreach (var enemy in stage.enemiesInfo)
        {
            GameObject go = Managers.Resource.Instantiate(
                Managers.Resource.Load<GameObject>("Prefabs/UI/RewardIconUI"),
                GetObject((int)GameObjects.EnemyInfo).transform);

            go.GetComponent<RewardIconUI>().Init(enemy.characterName, enemy.icon);
            go.GetComponent<RewardIconUI>().SetSpriteSize(new Vector2(100f, 100f));
        }

        GetButton((int)Buttons.BackImage).onClick.AddListener(OnClickCloseButton);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(OnClickCloseButton);
    }

    private void OnClickCloseButton()
    {
        Managers.UI.CloseUI(this);
    }
}
