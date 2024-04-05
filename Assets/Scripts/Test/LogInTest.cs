using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;
 
public class FirebaseGoogleAuth : MonoBehaviour
{
    public Text text;


    void Start()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .RequestServerAuthCode(false /* Don't force refresh */)
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        // 구글 플레이 게임 활성화
 
    }
 
    private void Log(string str)
    {
        text.text += str;
    }
 
    public void TryGoogleLogin()
    {
        Social.localUser.Authenticate(sucess =>
        {
            if (sucess)
            {
                FirebaseAuth(PlayGamesPlatform.Instance.GetServerAuthCode());           
            }
        });
    }

    public void FirebaseAuth(string authcode)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = 
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authcode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Log("SignInWithCredentialAsync was canceled.");
                return;
            }
    
            if (task.IsFaulted)
            {
                Log("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }
    
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            Log("Sucess");
        });
    }
 

}