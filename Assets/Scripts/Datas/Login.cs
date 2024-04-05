using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;

public class Login : MonoBehaviour
{
    private FirebaseAuth auth;

    void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)     // 연결된 백엔드 서버 애플리케이션에 전달되고 OAuth 토큰으로 교환될 수 있도록 서버 인증 코드를 생성하도록 요청
            .RequestIdToken()       // ID 토큰 생성을 요청(Firebase에서 플레이어를 식별하는 데 사용)
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();       // 구글 플레이 게임 활성화
    }

    public void GoogleLogin()
    {
        if (!Social.localUser.authenticated) // 로그인 되어 있지 않다면
        {
            Social.localUser.Authenticate(success => // 로그인 시도
            {
                if (success) // 성공하면
                {
                    Debug.Log("Success");
                    AuthToFirebase(PlayGamesPlatform.Instance.GetServerAuthCode());    // 플레이어 계정의 인증 코드로 Firebase에 인증 시도
                }
                else // 실패하면
                {
                    Debug.Log("Fail");
                }
            });
        }
    }

    public void GoogleLogout()
    {
        if (Social.localUser.authenticated) // 로그인 되어 있다면
        {
            PlayGamesPlatform.Instance.SignOut(); // Google 로그아웃
            auth.SignOut(); // Firebase 로그아웃
        }
    }

    public void AuthToFirebase(string authCode)
    {
        auth = FirebaseAuth.DefaultInstance;
        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>     // token을 사용하여 Firebase에 비동기적 로그인
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
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
            Debug.Log("Sucess");
        });
    }


}
