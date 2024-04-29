using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System;

public class LoadingUI : UIBase
{
    private FirebaseAuth auth;
    private bool isDataLoaded = false;
    public bool isMissionLoaded = false;

    private enum Texts
    {
        StatusText
    }

    private enum Images
    {
        ProgressBar
    }

    private enum Buttons
    {
        GoogleLogInButton,
        Background
    }

    void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)     // 연결된 백엔드 서버 애플리케이션에 전달되고 OAuth 토큰으로 교환될 수 있도록 서버 인증 코드를 생성하도록 요청
            .RequestIdToken()       // ID 토큰 생성을 요청(Firebase에서 플레이어를 식별하는 데 사용)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();       // 구글 플레이 게임 활성화
        Debug.Log("구글 플레이 활성화");
    }

    private void Start()
    {
        Init();
        Addressables.ClearDependencyCacheAsync("SO");
        Addressables.ClearDependencyCacheAsync("Image");
    }

    private void OnDestroy()
    {
        Database.OnLoadingProgressChanged -= UpdateProgress;
    }


    private void Update()
    {
        if (isDataLoaded && isMissionLoaded && Input.GetMouseButtonDown(0))
        {
            GameStart();
        }
    }


    private void Init()
    {
        Managers.Sound.Play(Constants.Sound.Bgm, "BGM/LoadingBGM");
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        Database.OnLoadingProgressChanged += UpdateProgress;

        GetButton((int)Buttons.GoogleLogInButton).onClick.AddListener(OnClickLogIn);

        //StartCoroutine(Managers.DB.DataLoad());
        //StartCoroutine(LoadAllData());
    }

    private void OnClickLogIn()
    {
        GoogleLogin();
    }

    private void UpdateProgress(float progress)
    {
        GetImage((int)Images.ProgressBar).fillAmount += progress;
        int progressPercentage = Mathf.FloorToInt(GetImage((int)Images.ProgressBar).fillAmount * 100);
        GetText((int)Texts.StatusText).text = $"데이터를 초기화하는 중... ({progressPercentage}%)";


        if (progressPercentage >= 100)
        {
            isDataLoaded = true;
            GetText((int)Texts.StatusText).text = "로딩 완료! 화면을 클릭하여 시작하세요.";
            GetButton((int)Buttons.Background).onClick.AddListener(GameStart);
        }
    }

    private void GameStart()
    {
        if (Managers.AccountData.playerData.Level == 1 && Managers.AccountData.playerData.exp == 0)
        {
            SignUpUI ui = Managers.UI.ShowUI<SignUpUI>();
            ui.Init();
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private void GoogleLogin()
    {
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Debug.Log("로그인시도");
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    Debug.Log("로그인성공");
                    AuthToFirebase(PlayGamesPlatform.Instance.GetServerAuthCode());    // 플레이어 계정의 인증 코드로 Firebase에 인증 시도
                    return;
                }
                else // 실패하면
                {
                    Debug.Log("로그인실패");
                }
            });
        }
        else
        {
            AuthToFirebase(PlayGamesPlatform.Instance.GetServerAuthCode());
        }
    }

    public void AuthToFirebase(string authCode)
    {
        GetButton((int)Buttons.GoogleLogInButton).gameObject.SetActive(false);
        auth = FirebaseAuth.DefaultInstance;
        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>     // token을 사용하여 Firebase에 비동기적 로그인
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
    
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
    
            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",user.DisplayName, user.UserId);
            Debug.Log("Sucess");

            GetButton((int)Buttons.GoogleLogInButton).gameObject.SetActive(true);

            StartCoroutine(LoadData());
        });
    }

    private IEnumerator LoadData()
    {
        Managers.DB.Init();
        yield return new WaitUntil(() => Managers.DB.isInited);
        StartCoroutine(Managers.DB.DataLoad());
    }


}
