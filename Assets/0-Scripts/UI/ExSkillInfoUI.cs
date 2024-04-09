using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExSkillInfoUI : UIBase
{
    public int characterId;
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
        ExSkillLevelUpButton
    }

    private void OnDestroy()
    {
        //Managers.AccountData.characterData[characterId].characterGrowth.OnExSkillLevelChanged -= UpdateDescription;
    }

    public void Init(int characterId)
    {
        this.characterId = characterId;
        string exSkillDescription = Managers.AccountData.characterData[characterId].characterData.skill.description;

        //Managers.AccountData.characterData[characterId].characterGrowth.OnExSkillLevelChanged += UpdateDescription;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetText((int)Texts.ExSkillNameText).text = $"Ex 스킬 / {Managers.AccountData.characterData[characterId].characterData.skill.skillName}";
        GetText((int)Texts.ExSkillLevelText).text = $"Lv. {Managers.AccountData.characterData[characterId].characterGrowth.ExSkillLevel}";
        GetText((int)Texts.ExSkilCostText).text = $"코스트: {Managers.AccountData.characterData[characterId].characterData.skill.cost}";

        GetButton((int)Buttons.BackImage).onClick.AddListener(CloseUI);
        GetButton((int)Buttons.ExSkillLevelUpButton).onClick.AddListener(OnClickExSkillLevelUpButton);

        UpdateDescription(Managers.AccountData.characterData[characterId].characterData.skill.description);
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

        Managers.AccountData.characterData[characterId].characterGrowth.ExSkillLevelUp(1);
    }
    private void CloseUI()
    {
        Debug.Log("CloseUI");

        Managers.UI.CloseUI(this);
    }
}
