using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.UIElements;

public class Database : MonoBehaviour
{
    private FirebaseUser user;
    private DatabaseReference reference = null;
    private DatabaseReference userDB = null;
    private string uid;

    public delegate void Func(DataSnapshot snapshot);



    // test를 위해 MonoBehaviour 사용
    // 추후 연결시 Init()으로 변경
    void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        uid = user != null ? user.UserId : null;

        // test용 임시 uid
        uid = "uid";

        
        // 데이터베이스의 경로설정
        FirebaseApp app = FirebaseDatabase.DefaultInstance.App;
        app.Options.DatabaseUrl = new System.Uri("https://nbc-srpg-default-rtdb.asia-southeast1.firebasedatabase.app/");
        // 데이터베이스의 RootReference를 가리킴
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userDB = reference.Child("users").Child(uid);

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
        string json = JsonUtility.ToJson(obj);
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
    public void Read(DatabaseReference path, Func action)
    {
        // 스냅샷 생성
        DataSnapshot snapshot = null;
        path.GetValueAsync().ContinueWith(task => 
        {
            // 데이터 읽기 실패
            if  (task.IsFaulted)
            {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
                return;
            }
            // 데이터 읽기 성공
            else if (task.IsCompleted)
            {
                // 스냅샷에 데이터 저장
                snapshot = task.Result;
                Debug.Log($"데이터 레코드 갯수 : {snapshot.ChildrenCount}");

                // Callback 함수 실행
                action(snapshot);
            }
        });
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
