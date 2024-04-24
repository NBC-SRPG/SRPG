public class SpecialUI : UIBase
{
    private enum Buttons
    {
        GoldSupplyButton,
        EquipmentUpgradeMaterialButton,
        CharacterUpgradeMaterialButton,
        CharacterLimitBreakMaterialButton,
        BackButton,
        HomeButton
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        Managers.UI.SetCanvas(gameObject);

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.GoldSupplyButton).onClick.AddListener(OnClickGoldSupplyButton);
        GetButton((int)Buttons.EquipmentUpgradeMaterialButton).onClick.AddListener(OnClickEquipmentUpgradeMaterialButton);
        GetButton((int)Buttons.CharacterUpgradeMaterialButton).onClick.AddListener(OnClickCharacterUpgradeMaterialButton);
        GetButton((int)Buttons.CharacterLimitBreakMaterialButton).onClick.AddListener(OnClickCharacterLimitBreakMaterialButton);
        GetButton((int)Buttons.BackButton).onClick.AddListener(OnClickBackButton);
        GetButton((int)Buttons.HomeButton).onClick.AddListener(OnClickHomeButton);
    }

    private void OnClickGoldSupplyButton()
    {

    }

    private void OnClickEquipmentUpgradeMaterialButton()
    {

    }

    private void OnClickCharacterUpgradeMaterialButton()
    {

    }

    private void OnClickCharacterLimitBreakMaterialButton()
    {

    }

    private void OnClickBackButton()
    {
        Managers.UI.CloseUI(this);
    }

    private void OnClickHomeButton()
    {
        Managers.UI.ReturnMainUI();
    }
}
