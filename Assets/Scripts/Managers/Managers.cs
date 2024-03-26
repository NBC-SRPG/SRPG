using System;
using System.Collections.Generic;
using UnityEngine;
public class Managers : MonoBehaviour
{
    private static Managers instance = null;
    public static Managers Instance { get { return instance; } }
    
    private static ResourceManager resourceManager = new ResourceManager();
    private static SoundManager soundManager = new SoundManager();
    private static UIManager uiManager = new UIManager();
    public static ResourceManager Resource { get { Init(); return resourceManager; } }
    public static SoundManager Sound { get { Init(); return soundManager; } }
    public static UIManager UI { get {  Init(); return uiManager; } }

    private static MapManager s_mapManager = new MapManager();
    public static MapManager MapManager { get { Init(); return s_mapManager; } }

    private static BattleManager s_battleManager = new BattleManager();
    public static BattleManager BattleManager { get { Init(); return s_battleManager; } }

    private static AccountData s_accountData = new AccountData();
    public static AccountData AccountData { get { Init(); return s_accountData; } }

    private static GachaManager s_gachamanager = new GachaManager();
    public static GachaManager GachaManager { get { Init(); return s_gachamanager; } }
    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (instance == null)
        {
            // @Managers 오브젝트 찾기
            GameObject go = GameObject.Find("@Managers");

            // 없으면 생성
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
            }

            // Managers 컴포넌트 Get 하여 s_instance에 적용
            if (go.TryGetComponent(out Managers managers))
            {
                instance = managers;
            }
            // 없으면 Add
            else
            {
                instance = go.AddComponent<Managers>();
            }

            // 씬 넘어가도 유지 되도록 DontDestroy
            DontDestroyOnLoad(go);

            // 들고있는 매니저들 Init
            resourceManager.Init();
            soundManager.Init();
            uiManager.Init();
            s_mapManager.Init();
            s_battleManager.Init();
            s_gachamanager.Init();

            // 테스트용 데이터
            s_accountData.Init(new Dictionary<string, int>(), 
                new Dictionary<int, Character>(), 
                new PlayerData(), 
                new Dictionary<int, bool>(), 
                new Dictionary<int, string[]>(), 
                new Dictionary<int, FormationData>(),
                new List<MailSO>());

            Character testChatacter1 = Resource.Load<GameObject>("Prefabs/TestCharacter1").GetComponent<Character>();
            Character testChatacter2 = Resource.Load<GameObject>("Prefabs/TestCharacter2").GetComponent<Character>();
            Character testChatacter3 = Resource.Load<GameObject>("Prefabs/TestCharacter3").GetComponent<Character>();
            s_accountData.characterData.Add((testChatacter1.characterData.character_Id), testChatacter1);
            s_accountData.characterData.Add((testChatacter2.characterData.character_Id), testChatacter2);
            s_accountData.characterData.Add((testChatacter3.characterData.character_Id), testChatacter3);

            testChatacter1.CharacterInit();
            testChatacter2.CharacterInit();
            testChatacter3.CharacterInit();
            testChatacter1.characterGrowth.InitialInit();
            testChatacter2.characterGrowth.InitialInit();
            testChatacter3.characterGrowth.InitialInit();

            MailSO mailSO = ScriptableObject.CreateInstance<MailSO>();
            mailSO.id = "1";
            mailSO.title = "테스트 메일입니다.";
            mailSO.sender = "테스트운영자";
            mailSO.dateSent = new DateTime(2024, 03, 15);
            DateTime expireDate = mailSO.dateSent.AddDays(14);
            mailSO.remainingTime = expireDate - DateTime.Now;
            if (expireDate <= DateTime.Now)
            {
                mailSO.remainingTime = TimeSpan.Zero;
            }
            else
            {
                mailSO.remainingTime = expireDate - DateTime.Now;
            }
            s_accountData.mailBox.Add(mailSO);
            
            mailSO = ScriptableObject.CreateInstance<MailSO>();
            mailSO.id = "2";
            mailSO.title = "테스트 메일입니다.";
            mailSO.sender = "테스트운영자";
            mailSO.dateSent = new DateTime(2024, 03, 17);
            expireDate = mailSO.dateSent.AddDays(14);
            mailSO.remainingTime = expireDate - DateTime.Now;
            if (expireDate <= DateTime.Now)
            {
                mailSO.remainingTime = TimeSpan.Zero;
            }
            else
            {
                mailSO.remainingTime = expireDate - DateTime.Now;
            }
            s_accountData.mailBox.Add(mailSO);

            // 앱 프레임 60으로 고정
            Application.targetFrameRate = 60;
        }
    }
}
