using UnityEngine;
using UnityEngine.UI;
using static Constants;

public class PassiveSkillInfoUI : UIBase
{
    private enum Texts
    {
        PassiveSkillDescriptionText,
        PassiveSkillNameText
    }

    private enum Images
    {
        PassiveSkillIconImage
    }

    private enum Buttons
    {
        ConfirmButton,
        CloseButton,
        BackImage
    }

    public void Init(int characterId)
    {
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        string exSkillDescription = Managers.AccountData.characterData[characterId].exSkill.description;

        GetText((int)Texts.PassiveSkillNameText).text = Managers.AccountData.characterData[characterId].passiveSkill.passiveName;
        GetText((int)Texts.PassiveSkillDescriptionText).text = Managers.AccountData.characterData[characterId].passiveSkill.description;
        GetImage((int)Images.PassiveSkillIconImage).sprite = Managers.AccountData.characterData[characterId].passiveSkill.icon;


        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.ConfirmButton).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);

    }

    private void CloseUI()
    {
        Managers.UI.CloseUI(this);
    }
}
