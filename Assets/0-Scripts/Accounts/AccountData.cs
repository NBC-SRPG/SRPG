using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AccountData
{
    public Dictionary<string, int> stageClearData { get; set; }
    public Dictionary<int, Character> characterData { get; set; }
    public PlayerData playerData { get; set; }
    public Dictionary<int, int> inventory { get; set; }
    public Dictionary<int, List<string>> friendData { get; set; }
    public Dictionary<int, FormationData> formationData { get; set; }
    public VersionData versionData { get; set; }
    public int gachaPoint { get; set; }
    public List<MailSO> mailBox { get; set; }

    public Dictionary<int, Mission> ongoingMissions { get; set; } = new(); // 진행중인 미션들

    public HashSet<int> completeMissions { get; set; } = new(); // 완료 한 미션들

    public HashSet<int> receiveMissions { get; set; } = new();// 보상 수령한 미션들

    private bool isOngoingMissionsLoaded = false;
    private bool isCompleteMissionsLoaded = false;
    private bool hasOngoingMissions = false;
    private bool hasCompleteMissions = false;


    #region Init
    public void InitStageClearData(DataSnapshot snapshot)
    {
        Dictionary<string, int> data = snapshot.Exists ? JsonUtility.FromJson<Dictionary<string, int>>(snapshot.GetRawJsonValue()) : new Dictionary<string, int>();
        stageClearData = data;
    }
    public void InitCharacterData(DataSnapshot snapshot)
    {

        foreach (var character in snapshot.Children)
        {
            characterData = new();

            CharacterGrowth growth = JsonConvert.DeserializeObject<CharacterGrowth>(character.GetRawJsonValue());

            Utility.Id2SO<CharacterSO>(growth.id, (result) =>
            {
                CharacterSO so = (CharacterSO)result;
                characterData.Add(growth.id, new Character(so, growth));


                Debug.Log(growth.id);
            });
        }

    }
    public void InitPlayerData(DataSnapshot snapshot)
    {
        // PlayerData의 변수들이 private set이라 DB에서 불러온 값 적용 불가
        /*
        PlayerData data = snapshot.Exists ? JsonUtility.FromJson<PlayerData>(snapshot.GetRawJsonValue()) : new PlayerData();
        playerData = data;
        */
        playerData = new();

        if (snapshot.Exists)
        {
            playerData.Init(
                snapshot.Child("uId").Value as string,
                snapshot.Child("playerName").Value as string,
                snapshot.Child("playerComment").Value as string,
                Convert.ToInt32(snapshot.Child("Diamond").Value),
                Convert.ToInt32(snapshot.Child("Gold").Value),
                Convert.ToInt32(snapshot.Child("Ap").Value),
                Convert.ToInt32(snapshot.Child("maxAp").Value),
                Convert.ToInt32(snapshot.Child("Level").Value),
                Convert.ToInt32(snapshot.Child("exp").Value),
                Convert.ToInt32(snapshot.Child("maxExp").Value),
                snapshot.Child("birthday").Value as string,
                //snapshot.Child("favoriteCharacter").Value as int[],
                Convert.ToInt32(snapshot.Child("lobbyCharacter").Value)
                //Convert.ToInt32(snapshot.Child("characterIcon").Value)
            );
        }
        else
        {
            // TODO
            // 저장된 값이 없을 때 기본 세팅
        }
    }
    public void InitInventoryData(DataSnapshot snapshot)
    {
        inventory = new();

        foreach (DataSnapshot childSnapshot in snapshot.Children)
        {
            inventory.Add(Convert.ToInt32(childSnapshot.Key), Convert.ToInt32(childSnapshot.Value));
        }
    }
    public void InitFriendData(DataSnapshot snapshot)
    {
        friendData = new();

        DataSnapshot friendSnapshot = snapshot.Child("Friend");
        DataSnapshot applyingSnapshot = snapshot.Child("Applying");
        DataSnapshot waitingSnapshot = snapshot.Child("Waiting");

        List<string> friendUids = ExtractUidsFromSnapshot(friendSnapshot);
        List<string> applyingUids = ExtractUidsFromSnapshot(applyingSnapshot);
        List<string> waitingUids = ExtractUidsFromSnapshot(waitingSnapshot);

        friendData.Add(Constants.FriendTabs, friendUids);
        friendData.Add(Constants.ApplyingTabs, applyingUids);
        friendData.Add(Constants.WaitingTabs, waitingUids);
    }
    private List<string> ExtractUidsFromSnapshot(DataSnapshot snapshot)
    {
        List<string> uids = new();

        foreach (DataSnapshot childSnapshot in snapshot.Children)
        {
            string uid = childSnapshot.Value.ToString();
            uids.Add(uid);
        }

        return uids;
    }
    public void InitFormationData(DataSnapshot snapshot)
    {
        formationData = new();

        foreach (var formation in snapshot.Children)
        {
            FormationData formationdata = new();

            formationdata.partyName = formation.Child("partyName").Value.ToString();
            
            foreach (var character in formation.Child("characterId").Children)
            {
                formationdata.characterId[int.Parse(character.Key)] = Convert.ToInt32(character.Value);
            }

            formationData.Add(int.Parse(formation.Key), formationdata);
        }
    }
    public void InitVersionData(DataSnapshot snapshot)
    {
        versionData = new();
        versionData.curGacha = new();
        versionData.curEvents = new();

        versionData.version = snapshot.Child("version").Value.ToString();

        foreach (var curGacha in snapshot.Child("curGacha").Children)
        {
            versionData.curGacha.Add(Convert.ToInt32(curGacha.Value));
        }

        foreach (var curEvent in snapshot.Child("curEvent").Children)
        {
            versionData.curEvents.Add((string)curEvent.Value);
        }
    }
    public void InitGachaPoint(DataSnapshot snapshot)
    {
        int data = snapshot.Exists ? Convert.ToInt32(snapshot.Value) : 0;
        gachaPoint = data;
    }
    // TODO
    // 메일을 받았을 때 이벤트를 걸어서 업데이트 하는걸로 변경
    public void InitMailBox(DataSnapshot snapshot)
    {
        if (mailBox == null)
        {
            mailBox = new();
        }
        else
        {
            mailBox.Clear();
        }


        foreach (var mail in snapshot.Children)
        {
            MailSO mailSO = ScriptableObject.CreateInstance<MailSO>();
            mailSO.key = mail.Key;
            mailSO.title = mail.Child("title").Value.ToString();
            foreach (var reward in mail.Child("rewards").Children)
            {
                mailSO.rewards.Add(int.Parse(reward.Key), int.Parse(reward.Value.ToString()));
            }
            mailSO.ap = int.Parse(mail.Child("ap").Value.ToString());
            mailSO.gold = int.Parse(mail.Child("gold").Value.ToString());
            mailSO.diamond = int.Parse(mail.Child("diamond").Value.ToString());

            mailSO.dateSent = DateTime.Parse(mail.Child("dateSent").Value.ToString());
            mailSO.expiration = int.Parse(mail.Child("expiration").Value.ToString());

            if (!mailSO.isExpired())
            {
                mailBox.Add(mailSO);
            }
            else
            {
                Managers.DB.Delete(Managers.DB.userDB.Child("mailBox").Child(mail.Key));
            }
        }
    }
    public void InitMissionData(DataSnapshot snapshot)
    {
        OngoingMissionInit(snapshot.Child("ongoingMissionsData"));
        CompleteMissionInit(snapshot.Child("completeMissionsData"));
    }
    
    public void Init()
    {
        
    }
    
    /*
    public void Init(
        Dictionary<string, int> stageClearData,
        Dictionary<int, Character> characterData,
        PlayerData playerData,
        //List<int> ongoingMissions,
        //List<int> completeMissions,
        //List<int> receiveMissions,
        //Dictionary<int, int> inventory,
        Dictionary<int, string[]> friendData,
        Dictionary<int, FormationData> formationData,
        List<MailSO> mailBox
        )
    {
        this.stageClearData = stageClearData ?? new Dictionary<string, int>();
        this.characterData = characterData ?? new Dictionary<int, Character>();
        this.playerData = playerData ?? new PlayerData();
        //this.inventory = inventory ?? new Dictionary<int, int>();
        this.friendData = friendData ?? new Dictionary<int, string[]>();
        this.formationData = formationData ?? new Dictionary<int, FormationData>();
        this.mailBox = mailBox ?? new List<MailSO>();
    }
    */
    private void OngoingMissionInit(DataSnapshot snapshot)
    {
        // 데이터가 존재하는지 확인
        if (snapshot.Exists && snapshot.ChildrenCount > 0)
        {
            // 데이터가 있다면 순회
            foreach (DataSnapshot mission in snapshot.Children)
            {
                // 데이터의 키(미션Id)와 값(진행정도)으로 MissionStart를 메인쓰레드에서 실행 
                MainThreadExecutor.ExecuteInMainThread(() => Managers.Mission.MissionStart(int.Parse(mission.Key), Convert.ToInt32(mission.Value)));
                Debug.Log(mission.Key + ":" + mission.Value);
            }
            // 데이터가 있음을 체크
            hasOngoingMissions = true;
        }
        else
        {
            // 데이터가 없으면 hasOngoingMissions은 false
            hasOngoingMissions = false;
        }
        // 진행 중 미션 초기화 완료
        isOngoingMissionsLoaded = true;

        CheckAndInitializeDefaultMissions();
    }

    private void CompleteMissionInit(DataSnapshot snapshot)
    {
        // 데이터가 존재하는지 확인
        if (snapshot.Exists && snapshot.ChildrenCount > 0)
        {
            // 데이터가 있다면 순회
            foreach (DataSnapshot mission in snapshot.Children)
            {
                // 데이터의 키(미션Id)로 미션 시작, 진행 정도는 완료 미션이기 때문에 해당 미션의 count를 그대로 적용
                MainThreadExecutor.ExecuteInMainThread(() => {
                    Managers.Mission.MissionStart(int.Parse(mission.Key), TestDatabase.Mission.Get(int.Parse(mission.Key)).count);
                    // 바로 클리어 처리
                    Managers.Mission.MissionClear(int.Parse(mission.Key));

                    // 데이터의 값이 true라면 보상 수령을 한 것
                    if ((bool)mission.Value)
                    {
                        // 보상 수령 처리
                        Managers.Mission.MissionReceive(int.Parse(mission.Key));
                    }
                });
                Debug.Log(mission.Key + ":" + mission.Value);
            }
            // 데이터가 있음을 체크
            hasCompleteMissions = true;
        }
        else
        {
            // 데이터가 없으면 hasCompleteMissions은 false
            hasCompleteMissions = false;
        }
        // 완료 미션 초기화 완료
        isCompleteMissionsLoaded = true;

        CheckAndInitializeDefaultMissions();
    }

    private void CheckAndInitializeDefaultMissions()
    {
        // 두 데이터 로드가 모두 완료되었는지 확인
        if (isOngoingMissionsLoaded && isCompleteMissionsLoaded)
        {
            // 두 데이터가 모두 비어 있으면 기본 미션 설정
            if (!hasOngoingMissions && !hasCompleteMissions)
            {
                MainThreadExecutor.ExecuteInMainThread(() =>
                {
                    // 기본 미션들을 설정
                    Managers.Mission.MissionStart(90001000);
                    Managers.Mission.MissionStart(90001001);
                    Managers.Mission.MissionStart(90001002);
                });
            }
        }
    }
    #endregion

    public void UpdateStageClearData(string stageName, int achievement)
    {
        if(stageClearData.TryAdd(stageName, achievement) == false)
        {
            stageClearData[stageName] = achievement;
        }
        Managers.DB.Write<int>(Managers.DB.userDB.Child("stageClearData").Child(stageName), achievement);
    }

    public void AcquireCharacter(int id)
    {
        if(!characterData.ContainsKey(id))
        {
            Utility.Id2SO<CharacterSO>(id, (result) =>
            {
                CharacterGrowth characterGrowth = new CharacterGrowth((CharacterSO)result);
                characterData.Add(id, new Character((CharacterSO)result, characterGrowth));
                Managers.DB.WriteWithJson(Managers.DB.userDB.Child("characterData").Child(id.ToString()), characterGrowth);
            });
        }
        else
        {
            // 캐릭터 조각 추가
        }
    }

    public void AcquireItems(int id, int count)
    {
        if(inventory.TryAdd(id, count) == false)
        {
            inventory[id] += count;
        }
        Managers.DB.Write<int>(Managers.DB.userDB.Child("inventory").Child(id.ToString()), inventory[id]);
    }

    public void ConsumeItems(int id, int count)
    {
        inventory[id] -= count;
        Managers.DB.Write<int>(Managers.DB.userDB.Child("inventory").Child(id.ToString()), inventory[id]);
    }

    public void DeleteMail(MailSO mail)
    {
        Managers.DB.Delete(Managers.DB.userDB.Child("mailBox").Child(mail.key));
        mailBox.Remove(mail);
    }

    public void SetFormationPartyName(int presetIndex, string partyName)
    {
        if (!formationData.ContainsKey(presetIndex))
        {
            formationData[presetIndex] = new FormationData()
            {
                partyName = partyName,
                characterId = new int[5]
            };
        }
        else
        {
            formationData[presetIndex].partyName = partyName;
        }

        Managers.DB.WriteWithJson(Managers.DB.userDB.Child("formationData").Child(presetIndex.ToString()), formationData[presetIndex]);
    }

    public void SetFormationCharacter(int presetIndex, int characterIndex, int characterId)
    {
        if (!formationData.ContainsKey(presetIndex))
        {
            formationData[presetIndex] = new FormationData()
            {
                partyName = "레이드용 파티",
                characterId = new int[5]
            };
        }

        formationData[presetIndex].characterId[characterIndex] = characterId;

        Managers.DB.WriteWithJson(Managers.DB.userDB.Child("formationData").Child(presetIndex.ToString()), formationData[presetIndex]);
    }
}
