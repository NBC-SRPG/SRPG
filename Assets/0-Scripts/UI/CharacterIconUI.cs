using UnityEngine.UI;

public class CharacterIconUI : UIBase
{
    private Outline outline;
    private CharacterListPopUpUI ui;
    public Character character;

    private enum Images
    {
        IconImage
    }

    private enum Buttons
    {
        IconButton
    }

    public void Init(Character character)
    {
        outline = GetComponent<Outline>();
        ui = Managers.UI.PeekUI<CharacterListPopUpUI>();
        this.character = character;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetImage((int)Images.IconImage).sprite = character.SO.icon;

        GetButton((int)Buttons.IconButton).onClick.AddListener(OnClickIconButton);
    }

    private void OnClickIconButton()
    {
        ui.SelectCharacter(this);
    }

    public void SetOutline(bool status)
    {
        outline.enabled = status;
    }
}
