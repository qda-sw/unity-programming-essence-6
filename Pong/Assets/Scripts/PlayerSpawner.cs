using Unity.Netcode;
using UnityEngine;

// 플레이어가 게임에 참가할 때 패들을 할당하는 스크립트
public class PlayerSpawner : NetworkBehaviour
{
    [Header("Spawn Points")]
    public Transform[] paddleSpawnPoints;
    
    [Header("Paddle Prefab")]
    public GameObject paddlePrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // 클라이언트 연결 이벤트 구독
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            // 이벤트 구독 해제
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
            }
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected");
        
        // 최대 2명의 플레이어만 허용
        if (NetworkManager.Singleton.ConnectedClients.Count <= 2)
        {
            SpawnPaddleForClient(clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} disconnected");
    }

    private void SpawnPaddleForClient(ulong clientId)
    {
        if (paddlePrefab == null || paddleSpawnPoints.Length == 0) return;

        // 클라이언트 인덱스 계산 (0 또는 1)
        int playerIndex = (int)(clientId % 2);
        
        if (playerIndex < paddleSpawnPoints.Length)
        {
            // 패들 생성
            GameObject paddle = Instantiate(paddlePrefab, paddleSpawnPoints[playerIndex].position, paddleSpawnPoints[playerIndex].rotation);
            
            // 네트워크 오브젝트로 스폰하고 소유권을 해당 클라이언트에게 할당
            NetworkObject networkObject = paddle.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnWithOwnership(clientId);
                
                // 플레이어별 조작키 설정
                PaddleController paddleController = paddle.GetComponent<PaddleController>();
                if (paddleController != null)
                {
                    if (playerIndex == 0)
                    {
                        // Player 1 (왼쪽) - W, S 키
                        paddleController.upKey = KeyCode.W;
                        paddleController.downKey = KeyCode.S;
                    }
                    else
                    {
                        // Player 2 (오른쪽) - 화살표 키
                        paddleController.upKey = KeyCode.UpArrow;
                        paddleController.downKey = KeyCode.DownArrow;
                    }
                }
            }
        }
    }
}