using UnityEngine;
using UnityEngine.Diagnostics;
public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    private static ResourceManager s_resourceManager = new ResourceManager();
    private static SoundManager s_soundManager = new SoundManager();
    private static UIManager s_uiManager = new UIManager();
    public static ResourceManager Resource { get { Init(); return s_resourceManager; } }
    public static SoundManager Sound { get { Init(); return s_soundManager; } }
    public static UIManager UI { get {  Init(); return s_uiManager; } }

    private static MapManager s_mapManager = new MapManager();
    public static MapManager MapManager { get { Init(); return s_mapManager; } }

    private static BattleManager s_battleManager = new BattleManager();
    public static BattleManager BattleManager { get { Init(); return s_battleManager; } }
    
    private static AccountData s_accountData = new AccountData();
    public static AccountData AccountData { get { Init(); return s_accountData; } }


    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
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
                s_instance = managers;
            }
            // 없으면 Add
            else
            {
                s_instance = go.AddComponent<Managers>();
            }

            // 씬 넘어가도 유지 되도록 DontDestroy
            DontDestroyOnLoad(go);
            
            // 들고있는 매니저들 Init
            s_resourceManager.Init();
            s_soundManager.Init();
            s_uiManager.Init();
            s_mapManager.Init();
            s_battleManager.Init();

            // 앱 프레임 60으로 고정
            Application.targetFrameRate = 60;
        }
    }
}
