using Unity.Netcode; // 유니티 넷코드
using Unity.Netcode.Transports.UTP; // 유니티 넷코드 UTP 트랜스포트
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Text infoText; // 연결 정보를 표시할 텍스트
    public InputField hostAddressInputField; // 호스트 주소를 입력할 인풋 필드
    private const ushort DefaultPort = 7777; // 기본 포트

    private void Awake()
    {
        infoText.text = string.Empty;
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionApproval = true;
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        // 접속이 종료된 경우 호출되는 콜백을 등록
        NetworkManager.Singleton.OnClientDisconnectCallback
            += OnClientDisconnectCallback;
    }

    private void OnDisable()
    {
        // 게임 종료시 네트워크 매니저가 먼저 파괴되는 경우에 대한 예외 처리
        if (NetworkManager.Singleton != null)
        {
            // 접속이 종료된 경우 호출되는 콜백을 해제
            NetworkManager.Singleton.OnClientDisconnectCallback
                -= OnClientDisconnectCallback;
        }
    }

    // 클라이언트가 연결을 끊었을 때 호출되는 콜백
    private void OnClientDisconnectCallback(ulong obj)
    {
        var reason = NetworkManager.Singleton.DisconnectReason;
        infoText.text = $"Disconnected: {reason}";
        Debug.Log($"Disconnected: {reason}");
    }

    // 연결을 승인할 때 호출되는 콜백
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        // 총 플레이어 수가 2명 이상이면 연결을 거부
        if (NetworkManager.Singleton.ConnectedClients.Count >= 2)
        {
            response.Approved = false;
            response.Reason = "Server full";
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
    }

    // 호스트로 게임을 생성할 때 호출되는 메서드
    public void CreateGameAsHost()
    {
        var networkManager = NetworkManager.Singleton;
        var transport = networkManager.NetworkConfig.NetworkTransport
            as UnityTransport;

        transport.ConnectionData.Port = DefaultPort;
        bool result = networkManager.StartHost();
        if (result)
        {
            networkManager.SceneManager
                .LoadScene("Lobby", LoadSceneMode.Single);
        }
        else
        {
            infoText.text = "Failed to start host";
            Debug.LogError("Failed to start host");
        }
    }

    // 클라이언트로 게임에 참여할 때 호출되는 메서드
    public void JoinGameAsClient()
    {
        var networkManager = NetworkManager.Singleton;
        var transport = networkManager.NetworkConfig.NetworkTransport
            as UnityTransport;

        transport.ConnectionData.Port = DefaultPort;
        transport.ConnectionData.Address = hostAddressInputField.text;
        bool result = networkManager.StartClient();
        if (!result)
        {
            infoText.text = "Failed to start client";
            Debug.LogError("Failed to start client");
        }

    }
}