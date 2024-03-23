using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera battaleCamera;

    private Transform PrimeCamera;
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera followingCharacterCamera;
    [SerializeField] private CinemachineVirtualCamera followingTileCamera;

    [SerializeField] private CinemachineTargetGroup followingTargetGroup;

    private CinemachineFramingTransposer characterComposer;

    [HideInInspector] public bool canMove;
    [HideInInspector] public float moveSpeed;

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

    }

    private void Start()
    {
        canMove = true;
        moveSpeed = 10f;

        characterComposer = followingCharacterCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        Ui = Managers.UI.FindPopup<BattleUI>();
        Ui.joyStick.OnPressJoystick += ResetCamera;
    }

    private void Update()
    {
        if (canMove)
        {
            MoveCamera();
            followingCharacterCamera.transform.position = PrimeCamera.position;
            followingTileCamera.transform.position = PrimeCamera.position;
        }

        if(PrimeCamera != mainCamera.transform)
        {
            mainCamera.transform.position = PrimeCamera.position;
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

        if (Input.mouseScrollDelta.y > 0)
        {
            if (mainCamera.m_Lens.OrthographicSize > 5)
            {
                mainCamera.m_Lens.OrthographicSize -= 0.2f;
            }
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if (mainCamera.m_Lens.OrthographicSize < 12)
            {
                mainCamera.m_Lens.OrthographicSize += 0.2f;
            }
        }
    }

    //애니메이션 재생 도중엔 카메라 움직임이 멈추도록
    public void SetCharacterCameraMove(int n)//n 이 1일 경우 움직임 멈춤, 0일 경우 움직임
    {
        characterComposer.m_DeadZoneHeight = n;
        characterComposer.m_DeadZoneWidth = n;
    }

    //카메라가 캐릭터를 따라다니게
    public void SetCameraOnCharacter(CharacterBase character)
    {
        canMove = false;

        mainCamera.Priority = 5;
        followingTileCamera.Priority = 5;
        followingCharacterCamera.Priority = 10;

        PrimeCamera = followingCharacterCamera.transform;

        followingCharacterCamera.Follow = character.transform;
    }

    //카메라가 타일을 따라다니게
    public void SetCameraOnTile(OverlayTile tile)
    {
        mainCamera.Priority = 5;
        followingTileCamera.Priority = 10;
        followingCharacterCamera.Priority = 5;

        PrimeCamera = followingTileCamera.transform;

        followingTileCamera.Follow = tile.transform;
    }

    //선택된 캐릭터 따라가기
    public void SetCameraOnSelected()
    {
        mainCamera.Priority = 5;
        followingTileCamera.Priority = 10;
        followingCharacterCamera.Priority = 5;

        PrimeCamera = followingTileCamera.transform;

        followingTileCamera.Follow = followingTargetGroup.transform;
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

    //그룹에서 목표물 제거
    public void RemoveGroup(CharacterBase character)
    {
        if (Array.Exists(followingTargetGroup.m_Targets, x => x.target == character.transform))
        {
            followingTargetGroup.RemoveMember(character.transform);
        }
    }

    public void ResetCamera()// 카메라 초기화
    {
        canMove = true;

        mainCamera.Priority = 10;
        followingTileCamera.Priority = 5;
        followingCharacterCamera.Priority = 5;

        followingCharacterCamera.Follow = null;
        followingTileCamera.Follow = null;

        PrimeCamera = mainCamera.transform;

    }
}
