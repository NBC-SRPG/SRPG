using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;

public class Database
{
    private FirebaseUser user;
    private DatabaseReference reference = null;
    public DatabaseReference userDB = null;
    private string uid;
    private const int dataCount = 9;
    public delegate void Func(DataSnapshot snapshot);
    public static event Action<float> OnLoadingProgressChanged;

    public static void UpdateLoadingProgress(float progress)
    {
        //Debug.Log($"UpdateLoadingProgress: {progress}");
        OnLoadingProgressChanged?.Invoke(progress);
    }

    // test를 위해 MonoBehaviour 사용
    // 추후 연결시 Init()으로 변경
    public void Init()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        uid = user != null ? user.UserId : null;

        // test용 임시 uid
        uid = "uid";

        
        // 데이터베이스의 경로설정
        FirebaseApp app = FirebaseDatabase.DefaultInstance.App;
        app.Options.DatabaseUrl = new Uri("https://nbc-srpg-default-rtdb.asia-southeast1.firebasedatabase.app/");
        // 데이터베이스의 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userDB = reference.Child("users").Child(uid);

        // 친구, 메일 등 실시간 업데이트가 필요한 데이터 업데이트 시 이벤트
        userDB.Child("friendData").ValueChanged += FriendDataValueChange;
        userDB.Child("mailBox").ValueChanged += MailBoxValueChange;
    }

    private void FriendDataValueChange(object sender, ValueChangedEventArgs args)
    {
        FriendDataUpdate();
    }

    private void MailBoxValueChange(object sender, ValueChangedEventArgs args)
    {

    }

    public IEnumerator DataLoad()
    {
        yield return Read(userDB.Child("stageClearData"), data =>
        {
            Managers.AccountData.InitStageClearData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("characterData"), data =>
        {
            Managers.AccountData.InitCharacterData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("playerData"), data =>
        {
            Managers.AccountData.InitPlayerData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("inventory"), data =>
        {
            Managers.AccountData.InitInventoryData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("friendData"), data =>
        {
            Managers.AccountData.InitFriendData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("formationData"), data =>
        {
            Managers.AccountData.InitFormationData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("versionData"), data =>
        {
            Managers.AccountData.InitVersionData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("gachaPoint"), data =>
        {
            Managers.AccountData.InitGachaPoint(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
        yield return Read(userDB.Child("missionData"), data =>
        {
            Managers.AccountData.InitMissionData(data);
            UpdateLoadingProgress(1.0f / dataCount);
        });
    }

    private IEnumerator FriendDataUpdate()
    {
        yield return Read(userDB.Child("friendData"), data =>
        {
            Managers.AccountData.InitFriendData(data);
        });
    }

    public IEnumerator MailLoad()
    {
        yield return Read(userDB.Child("mailBox"), data =>
        {
            Managers.AccountData.InitMailBox(data);
        });
    }
    #region CRUD

    /// <summary>
    /// DB에서 하나의 value를 write
    /// </summary>
    /// <typeparam name="T"> write할 타입(number, boolean, string) </typeparam>
    /// <param name="path"> 경로 </param>
    /// <param name="value"> write할 값 </param>
    public void Write<T>(DatabaseReference path, T value)
    {
        path.SetValueAsync(value);
    }

    /// <summary>
    /// DB에서 하나의 value를 Json으로 write
    /// </summary>
    /// <param name="path"> 경로 </param>
    /// <param name="obj"> Json으로 변환할 값 </param>
    public void WriteWithJson(DatabaseReference path, object obj)
    {
        string json = JsonConvert.SerializeObject(obj);
        Debug.Log(json);
        path.SetRawJsonValueAsync(json);
    }

    
    /// <summary>
    /// DB에서 여러 개의 value를 update
    /// </summary>
    /// <param name="path"> 경로 </param>
    /// <param name="childUpates"> 하위경로를 key, 값을 value로 가지는 Dictionary </param>
    public void UpdateChilds(DatabaseReference path, Dictionary<string, object> childUpates)
    {
        path.UpdateChildrenAsync(childUpates);
    }

    /// <summary>
    /// DB에서 하나의 값을 제거
    /// </summary>
    /// <param name="path"> 경로 </param>
    public void Delete(DatabaseReference path)
    {
        path.RemoveValueAsync();
    }

    /// <summary>
    /// DB에서 데이터를 snapshot에 읽어온 후, callback 함수로 사용
    /// </summary>
    /// <param name="path"> 경로 </param>
    /// <param name="action"> callback 함수 </param>
    public IEnumerator Read(DatabaseReference path, Func action)
    {
        // 스냅샷 생성
        DataSnapshot snapshot = null;
        //path.GetValueAsync().ContinueWithOnMainThread(task => 
        var task = path.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        // 데이터 읽기 실패
        if  (task.IsFaulted)
        {
            Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            //return;
        }
        // 데이터 읽기 성공
        else if (task.IsCompleted)
        {
            // 스냅샷에 데이터 저장
            snapshot = task.Result;
            Debug.Log($"{path} 데이터 레코드 갯수 : {snapshot.ChildrenCount}");
            // Callback 함수 실행
            action(snapshot);
        }
    }

    /// <summary>
    /// 
    /// <param uId> 삭제하려는 유저의 uId </param>
    /// <param friendTab> 삭제하려는 친구탭 </param>
    /// </summary>
    public void FriendDataDelete(string uId, int friendTab)
    {

    }

    #endregion

    #region testCodes

    private void WriteTest()
    {
        Debug.Log(userDB.ToString());
        Write<int>(reference.Child("users").Child("uid").Child("characterData"), 135);
    }

    private void UpdateTest()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic["/characterData/" + "135"] = 100;
        dic["/characterData/" + "137"] = 200;

        UpdateChilds(userDB, dic);
    }

    private void ReadTest()
    {
        Read(userDB.Child("characterData"), LogRead);
    }


    private void LogRead(DataSnapshot snapshot)
    {
        foreach (DataSnapshot item in snapshot.Children)
        {
            Debug.Log(item.Key + ":" + item.Value);
        }
    }

    #endregion
}
