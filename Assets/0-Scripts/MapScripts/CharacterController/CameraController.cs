using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera battleCamera;

    [Header("Idle_Camera")]
    private Transform PrimeCamera;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera followingCharacterCamera;
    [SerializeField] private CinemachineVirtualCamera followingTileCamera;
    [SerializeField] private CinemachineVirtualCamera followingCharacterGroupCamera;
    [SerializeField] private CinemachineTargetGroup followingTargetGroup;

    [Header("BattleCamera")]
    [SerializeField] private CinemachineVirtualCamera BattleGroupCameara;
    public CinemachineTargetGroup battleTargetGroup;

    private CinemachineFramingTransposer mainComposer;
    private CinemachineFramingTransposer characterComposer;
    private CinemachineFramingTransposer characterGroupComposer;

    private CinemachineFramingTransposer battleGroupComposer;
    private CinemachineBasicMultiChannelPerlin noise;

    private CinemachineConfiner2D mainConfinder;

    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isSelected;
    [HideInInspector] public float moveSpeed;

    private Vector2 lastTouchPosition;
    private bool dragMove;
    private Vector2 moveDelta;

    private BattleUI Ui;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dragMove = false;

    }

    private void Start()
    {
        canMove = true;
        moveSpeed = 20f;

        mainComposer = mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        characterComposer = followingCharacterCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        characterGroupComposer = followingCharacterGroupCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        mainConfinder = mainCamera.GetComponent<CinemachineConfiner2D>();

        if (MapManager.instance != null && MapManager.instance.cameraArea != null)
        {
            mainConfinder.m_BoundingShape2D = MapManager.instance.cameraArea;
            followingCharacterCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = MapManager.instance.cameraArea;
            followingTileCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = MapManager.instance.cameraArea;
            followingCharacterGroupCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = MapManager.instance.cameraArea;
        }

        battleGroupComposer = BattleGroupCameara.GetCinemachineComponent<CinemachineFramingTransposer>();
        noise = BattleGroupCameara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Ui = Managers.UI.FindUI<BattleUI>();
        Ui.joyStick.OnPressJoystick += ResetCamera;

        ResetCamera();
    }

    public void CameraIntro()
    {
        StartCoroutine(Intro());
    }

    private IEnumerator Intro()
    {
        float time = 0;

        while (time < 0.5f)
        {
            mainCamera.m_Lens.OrthographicSize = Mathf.Lerp(20, 10, time / 0.5f);
            time += Time.deltaTime;

            yield return null;
        }

    }

    private void Update()
    {
        if (canMove)
        {
            MoveCamera();
            followingCharacterCamera.transform.position = PrimeCamera.position;
            followingTileCamera.transform.position = PrimeCamera.position;
            followingCharacterGroupCamera.transform.position = PrimeCamera.position;
        }

        if(PrimeCamera != mainCamera.transform)
        {
            mainCamera.transform.position = PrimeCamera.position;
        }


        if (!AnimationController.instance.CheckAnimation())
        {
            battleTargetGroup.transform.position = Vector3.zero;
        }

        if(mainCamera.transform.position.x < mainConfinder.m_BoundingShape2D.bounds.min.x ) 
        {
            mainCamera.transform.position = new Vector3(mainConfinder.m_BoundingShape2D.bounds.min.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
        else if(mainCamera.transform.position.x > mainConfinder.m_BoundingShape2D.bounds.max.x)
        {
            mainCamera.transform.position = new Vector3(mainConfinder.m_BoundingShape2D.bounds.max.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }

        if (mainCamera.transform.position.y < mainConfinder.m_BoundingShape2D.bounds.min.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainConfinder.m_BoundingShape2D.bounds.min.y, mainCamera.transform.position.z);
        }
        else if (mainCamera.transform.position.y > mainConfinder.m_BoundingShape2D.bounds.max.y)
        {
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainConfinder.m_BoundingShape2D.bounds.max.y, mainCamera.transform.position.z);
        }
    }


    private void MoveCamera()//조이스틱으로 카메라 이동
    {
        float x = Ui.joyStick.Horizontal();
        float y = Ui.joyStick.Vertical();

        if(x != 0 || y != 0)
        {
            mainCamera.transform.position += new Vector3(x, y, 0) * (moveSpeed * Time.deltaTime);
        }

        CameraMoveWithTouch();

        if (Input.mouseScrollDelta.y > 0)
        {
            mainCamera.m_Lens.OrthographicSize -= 0.2f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            mainCamera.m_Lens.OrthographicSize += 0.2f;
        }

        ZoomWithTouch();

        if (mainCamera.m_Lens.OrthographicSize < 5)
        {
            mainCamera.m_Lens.OrthographicSize = 5;
        }
        if (mainCamera.m_Lens.OrthographicSize > 12)
        {
            mainCamera.m_Lens.OrthographicSize = 12;
        }
    }

    private void CameraMoveWithTouch()
    {
        if(Input.touchCount == 1 && EventSystem.current.IsPointerOverGameObject() == false && !isSelected)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began )
            {
                ResetCamera();
                dragMove = true;
                lastTouchPosition = touch.position;
            }
            else if(touch.phase == TouchPhase.Moved )
            {
                moveDelta = Camera.main.ScreenToWorldPoint(lastTouchPosition) - Camera.main.ScreenToWorldPoint(touch.position);

                Vector2 move = moveDelta * (moveSpeed * 0.05f);

                if (mainCamera.transform.position.x >= mainConfinder.m_BoundingShape2D.bounds.min.x && mainCamera.transform.position.x <= mainConfinder.m_BoundingShape2D.bounds.max.x &&
                    mainCamera.transform.position.y >= mainConfinder.m_BoundingShape2D.bounds.min.y && mainCamera.transform.position.y <= mainConfinder.m_BoundingShape2D.bounds.max.y)
                {
                    mainCamera.transform.position += new Vector3(move.x, move.y, 0);
                }

                lastTouchPosition = touch.position;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                dragMove = false;
            }


        }
    }

    private void ZoomWithTouch()
    {
        if (Input.touchCount == 2) //손가락 2개가 눌렸을 때
        {
            Touch touchZero = Input.GetTouch(0); //첫번째 손가락 터치
            Touch touchOne = Input.GetTouch(1); //두번째 손가락 터치

            //터치에 대한 이전 위치값을 각각 저장함
            //처음 터치한 위치(touchZero.position)에서 이전 프레임에서의 터치 위치와 이번 프로임에서 터치 위치의 차이를 뺌
            Vector2 touchZeroPrev = touchZero.position - touchZero.deltaPosition; //deltaPosition는 이동방향 추적할 때 사용
            Vector2 touchOnePrev = touchOne.position - touchOne.deltaPosition;

            // 각 프레임에서 터치 사이의 벡터 거리 구함
            float prevTouch = (touchZeroPrev - touchOnePrev).magnitude; //magnitude는 두 점간의 거리 비교(벡터)
            float nowTouch = (touchZero.position - touchOne.position).magnitude;

            // 거리 차이 구함(거리가 이전보다 크면(마이너스가 나오면)손가락을 벌린 상태_줌인 상태)
            float deltaMagnitudeDiff = prevTouch - nowTouch;

            mainCamera.m_Lens.OrthographicSize += deltaMagnitudeDiff * 0.05f;

        }
    }

    //애니메이션 재생 도중엔 카메라 움직임이 멈추도록
    public void SetCharacterCameraMove(int n)//n 이 1일 경우 움직임 멈춤, 0일 경우 움직임
    {
        characterComposer.m_DeadZoneHeight = n;
        characterComposer.m_DeadZoneWidth = n;
        characterComposer.m_SoftZoneHeight = n + 0.8f;
        characterComposer.m_SoftZoneWidth = n + 0.8f;

        characterGroupComposer.m_DeadZoneHeight = n;
        characterGroupComposer.m_DeadZoneWidth = n;
        characterGroupComposer.m_SoftZoneHeight = n + 0.8f;
        characterGroupComposer.m_SoftZoneWidth = n + 0.8f;
    }

    //카메라가 캐릭터를 따라다니게
    public void SetCameraOnCharacter(CharacterBase character)
    {
        canMove = false;

        mainCamera.Priority = 5;
        followingTileCamera.Priority = 5;
        followingCharacterCamera.Priority = 10;
        followingCharacterGroupCamera.Priority = 5;

        PrimeCamera = followingCharacterCamera.transform;

        followingCharacterCamera.Follow = character.transform;
    }

    //카메라가 타일을 따라다니게
    public void SetCameraOnTile(OverlayTile tile)
    {
        mainCamera.Priority = 5;
        followingTileCamera.Priority = 10;
        followingCharacterCamera.Priority = 5;
        followingCharacterGroupCamera.Priority = 5;

        PrimeCamera = followingTileCamera.transform;

        followingTileCamera.Follow = tile.transform;
    }

    //선택된 캐릭터 따라가기
    public void SetCameraOnSelected()
    {
        mainCamera.Priority = 5;
        followingTileCamera.Priority = 5;
        followingCharacterCamera.Priority = 5;
        followingCharacterGroupCamera.Priority = 10;

        PrimeCamera = followingCharacterGroupCamera.transform;

        followingCharacterGroupCamera.Follow = followingTargetGroup.transform;
    }

    //그룹 카메라에 목표물 추가
    public void AddGroup(CharacterBase character)
    {
        if(character == null)
        {
            return;
        }

        if (!Array.Exists(followingTargetGroup.m_Targets, x => x.target == character.transform))
        {
            followingTargetGroup.AddMember(character.transform, 1, 1);
        }
    }

    public void AddGroupRange(List<CharacterBase> list)
    {
        if(list.Count == 0)
        {
            return;
        }

        foreach(CharacterBase character in list)
        {
            AddGroup(character);
        }
    }

    public void AddTargetGroup(Transform trans)
    {
        followingTargetGroup.AddMember(trans, 1, 5);
    }

    //그룹에서 목표물 제거
    public void RemoveGroup(CharacterBase character)
    {
        if (Array.Exists(followingTargetGroup.m_Targets, x => x.target == character.transform))
        {
            followingTargetGroup.RemoveMember(character.transform);
        }
    }

    public void ResetGroup()
    {
        followingTargetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
    }

    public void ResetCamera()// 카메라 초기화
    {
        canMove = true;

        mainCamera.Priority = 10;
        followingTileCamera.Priority = 5;
        followingCharacterCamera.Priority = 5;
        followingCharacterGroupCamera.Priority = 5;

        followingCharacterCamera.Follow = null;
        followingTileCamera.Follow = null;
        followingCharacterGroupCamera.Follow = null;

        PrimeCamera = mainCamera.transform;
    }

    //------------------------------------------------------------------------------------------------
    //전투 연출 카메라

    public void AddBattleTargetGroup(Transform transform, float scale)
    {
        battleTargetGroup.AddMember(transform, 1, scale);
    }

    public void RemoveTargetGroup(Transform transform)
    {
        if (Array.Exists(battleTargetGroup.m_Targets, x => x.target == transform))
        {
            battleTargetGroup.RemoveMember(transform);
        }
    }

    public void ResetBattleGroup()
    {
        battleTargetGroup.m_Targets = new CinemachineTargetGroup.Target[0];
    }

    public void SetMinOrtho(int min)
    {
        battleGroupComposer.m_MinimumOrthoSize = min;
    }

    public void ShakeCamera(float duration, float scale, float frequency)
    {
        noise.m_AmplitudeGain = scale;
        noise.m_FrequencyGain = frequency;

        Invoke(nameof(StopShake), duration);
    }

    public void StopShake()
    {
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
