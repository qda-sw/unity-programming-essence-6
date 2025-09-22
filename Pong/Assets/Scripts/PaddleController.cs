using Unity.Netcode;
using UnityEngine;

// 네트워크 동기화되는 패들 컨트롤러
public class PaddleController : NetworkBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float moveBounds = 4f; // Y축 이동 범위

    [Header("Input Keys")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    private Vector3 startPosition;

    private void Start()
    {
        // 시작 위치 저장
        startPosition = transform.position;
    }

    private void Update()
    {
        // 오직 소유자만 입력을 처리
        if (!IsOwner) return;

        HandleInput();
    }

    private void HandleInput()
    {
        float moveInput = 0f;

        // 키보드 입력 처리
        if (Input.GetKey(upKey))
            moveInput = 1f;
        else if (Input.GetKey(downKey))
            moveInput = -1f;

        // 움직임 적용
        if (moveInput != 0f)
        {
            Vector3 newPosition = transform.position;
            newPosition.y += moveInput * moveSpeed * Time.deltaTime;
            
            // 경계 체크
            newPosition.y = Mathf.Clamp(newPosition.y, 
                startPosition.y - moveBounds, 
                startPosition.y + moveBounds);
                
            transform.position = newPosition;
        }
    }

    // 패들 위치 초기화
    public void ResetPosition()
    {
        transform.position = startPosition;
    }

    // 서버에서 위치 동기화 (필요시 사용)
    [ServerRpc]
    public void UpdatePositionServerRpc(Vector3 position)
    {
        transform.position = position;
        UpdatePositionClientRpc(position);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 position)
    {
        if (!IsOwner)
        {
            transform.position = position;
        }
    }
}