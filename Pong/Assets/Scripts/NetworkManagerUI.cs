using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// 네트워크 연결 UI를 관리하는 스크립트
public class NetworkManagerUI : MonoBehaviour
{
    [Header("UI References")]
    public Button hostButton;
    public Button clientButton;
    public GameObject menuUI;
    public GameObject lobbyUI;
    public Text statusText;

    private void Start()
    {
        // 버튼 이벤트 설정
        if (hostButton != null)
            hostButton.onClick.AddListener(StartHost);
            
        if (clientButton != null)
            clientButton.onClick.AddListener(StartClient);

        // 초기 상태 설정
        if (menuUI != null)
            menuUI.SetActive(true);
            
        if (lobbyUI != null)
            lobbyUI.SetActive(false);

        UpdateStatusText("메뉴");
    }

    // 호스트로 시작
    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            UpdateUI(true);
            UpdateStatusText("호스트로 게임 시작됨");
        }
        else
        {
            UpdateStatusText("호스트 시작 실패");
        }
    }

    // 클라이언트로 연결
    public void StartClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            UpdateUI(true);
            UpdateStatusText("서버에 연결 중...");
        }
        else
        {
            UpdateStatusText("서버 연결 실패");
        }
    }

    // UI 상태 업데이트
    private void UpdateUI(bool inGame)
    {
        if (menuUI != null)
            menuUI.SetActive(!inGame);
            
        if (lobbyUI != null)
            lobbyUI.SetActive(inGame);
    }

    // 상태 텍스트 업데이트
    private void UpdateStatusText(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void OnDestroy()
    {
        // 메모리 누수 방지
        if (hostButton != null)
            hostButton.onClick.RemoveListener(StartHost);
            
        if (clientButton != null)
            clientButton.onClick.RemoveListener(StartClient);
    }
}