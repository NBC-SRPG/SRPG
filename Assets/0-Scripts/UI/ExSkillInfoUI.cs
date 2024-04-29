using UnityEngine;
public class ExSkillInfoUI : UIBase
{
    public Character character;
    private enum Texts
    {
        ExSkillNameText,
        ExSkillLevelText,
        ExSkillDescriptionText,
        ExSkilCostText
    }
    private enum Buttons
    {
        BackImage,
        ExSkillLevelUpButton,
        CloseButton
    }

    private enum Images
    {
        ExSkillRange
    }

    private void OnDestroy()
    {
        //Managers.AccountData.characterData[characterId].characterGrowth.OnExSkillLevelChanged -= UpdateDescription;
    }

    public void Init(Character character)
    {
        this.character = character;

        //Managers.AccountData.characterData[characterId].characterGrowth.OnExSkillLevelChanged += UpdateDescription;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetText((int)Texts.ExSkillNameText).text = $"{character.exSkill.skillName} / Lv.{character.Growth.exSkillLevel}";
        GetText((int)Texts.ExSkillDescriptionText).text = character.exSkill.description;
        GetText((int)Texts.ExSkilCostText).text = $"코스트: {character.exSkill.cost}";
        GetImage((int)Images.ExSkillRange).sprite = character.exSkill.icon;

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.CloseButton).onClick.AddListener(CloseUI);
        //GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);

        //UpdateDescription(Managers.AccountData.characterData[characterId].exSkill.description);
    }
    private void UpdateDescription(string newDescription)
    {
        // TODO
        // 각종 변경해야 할 부분 구하는 함수 만들기
        // 이 부분은 스킬이 가지고 있는게 더 좋을듯??

        string exSkillDescription = newDescription;
        exSkillDescription = exSkillDescription.Replace("{Damage}", "200%");
        exSkillDescription = exSkillDescription.Replace("{Attribute}", "N");
        exSkillDescription = exSkillDescription.Replace("{Count}", "3");
        exSkillDescription = exSkillDescription.Replace("{Turn}", "1");
        
        GetText((int)Texts.ExSkillDescriptionText).text = exSkillDescription;
    }
    private void OnClickExSkillLevelUpButton()
    {
        Debug.Log("OnClickExSkillLevelUpButton");

        // TODO
        // 스킬 레벨이 변경 되었을 때 이벤트를 걸기
        // Ap, 골드, 다이아에 했던 것 처럼 이벤트 걸기
        // 스킬 레벨이 변경 되면 스킬 설명 업데이트 하기

        character.Growth.exSkillLevel++;
    }
    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }
}
