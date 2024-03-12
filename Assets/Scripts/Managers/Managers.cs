using UnityEngine;
using UnityEngine.Diagnostics;
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

            // 앱 프레임 60으로 고정
            Application.targetFrameRate = 60;
        }
    }
}
