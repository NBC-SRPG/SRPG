using System;
using System.Linq;
using UnityEngine;
using static Constants;

public class GachaEntryUI : UIBase
{

    // 캐릭터 id
    public int characterId;

    
    private enum Images
    {
        CharacterImage,
        CharacterOutline,
        CharacterAttributeImage,
    }
    
    private enum GameObjects
    {
        Star
    }

    private void Start()
    {
        Init();

    }

    private void Init()
    {
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        Utility.Id2SO<CharacterSO>(characterId, (result) =>
        {
            GetImage((int)Images.CharacterImage).sprite = (result as CharacterSO).icon;
            GetImage((int)Images.CharacterAttributeImage).sprite = (result as CharacterSO).GetElementSprite();
            SetOutline((result as CharacterSO).elementType);
            SetStar((result as CharacterSO).basicStar);
        });

    }



    private void SetOutline(Constants.ElementType elementType)
    {
        switch (elementType)
        {
            case Constants.ElementType.Fire:
                GetImage((int)Images.CharacterOutline).color = Color.red;
                break;
            case Constants.ElementType.Water:
                GetImage((int)Images.CharacterOutline).color = Color.blue;
                break;
            case Constants.ElementType.Grass:
                GetImage((int)Images.CharacterOutline).color = Color.green;
                break;
            case Constants.ElementType.Bolt:
                GetImage((int)Images.CharacterOutline).color = Color.yellow;
                break;
            case Constants.ElementType.Dark:
                GetImage((int)Images.CharacterOutline).color = Color.black;
                break;
            case Constants.ElementType.Light:
                GetImage((int)Images.CharacterOutline).color = Color.white;
                break;
        }
    }

    private void SetStar(int numberOfStars)
    {
        float starWidth = 25f; // 별 이미지의 너비

        // 기존에 생성된 별들 제거
        foreach (Transform child in GetObject((int)GameObjects.Star).transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfStars; i++)
        {
            GameObject star = Managers.Resource.Instantiate("Star", GetObject((int)GameObjects.Star).transform);
            star.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            RectTransform rt = star.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(0f, 0f);
            rt.anchorMin = new Vector2(0f, 0f);
            rt.pivot = new Vector2(0f, 0f);
            rt.anchoredPosition = new Vector2(i * starWidth, 0);
        }
    }

}
