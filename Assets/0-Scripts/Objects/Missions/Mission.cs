using static Constants;

// 미션의 동적인 상태를 관리
// 미션의 정적 상태는 DB를 참고하기
public class Mission
{
    private int missionId; // 미션 ID
    public int MissionId
    {
        get => missionId;
        private set => missionId = value;
    }

    private int missionProgress; // 미션 진행 상황
    public int MissionProgress
    {
        get => missionProgress;
        private set => missionProgress = value;
    }

    private MissionState missionState; // 미션 상태
    public MissionState MissionState
    {
        get => missionState;
        private set => missionState = value;
    }

    public Mission(int missionId)
    {
        MissionId = missionId;
        MissionProgress = 0;
        MissionState = MissionState.Waite;
    }
    public Mission(int missionId, int progress)
    {
        MissionId = missionId;
        MissionProgress = progress;
        MissionState = MissionState.Progress;
    }

    public void Start()
    {
        MissionState = MissionState.Progress;
    }

    public int Update(int amount)
    {
        MissionProgress += amount;
        return MissionProgress;
    }

    public int StarUpdate(int amount)
    {
        if (MissionProgress < amount)
        {
            MissionProgress = amount;
        }

        return MissionProgress;
    }

    public void Complete()
    {
        MissionState = MissionState.Complete;
    }
}
