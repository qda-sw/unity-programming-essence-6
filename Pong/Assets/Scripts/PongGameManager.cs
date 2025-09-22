using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 퐁 게임의 전체적인 관리를 담당하는 게임 매니저
public class PongGameManager : NetworkBehaviour
{
    public static PongGameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int winningScore = 5;

    [Header("UI References")]
    public Text player1ScoreText;
    public Text player2ScoreText;
    public Text gameStatusText;
    public GameObject gameOverUI;
    public Button restartButton;
    public Button menuButton;

    [Header("Game Objects")]
    public BallController ball;
    public PaddleController[] paddles;

    // 네트워크 동기화되는 점수
    private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    private NetworkVariable<int> player2Score = new NetworkVariable<int>(0);
    private NetworkVariable<bool> gameOver = new NetworkVariable<bool>(false);

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public override void OnNetworkSpawn()
    {
        // 네트워크 변수 변경 이벤트 구독
        player1Score.OnValueChanged += OnPlayer1ScoreChanged;
        player2Score.OnValueChanged += OnPlayer2ScoreChanged;
        gameOver.OnValueChanged += OnGameOverChanged;

        // UI 초기화
        UpdateScoreUI();
        UpdateGameStatusUI();

        // 버튼 이벤트 설정
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (menuButton != null)
            menuButton.onClick.AddListener(ReturnToMenu);

        // 게임 시작
        if (IsServer)
        {
            StartGame();
        }
    }

    public override void OnNetworkDespawn()
    {
        // 이벤트 구독 해제
        player1Score.OnValueChanged -= OnPlayer1ScoreChanged;
        player2Score.OnValueChanged -= OnPlayer2ScoreChanged;
        gameOver.OnValueChanged -= OnGameOverChanged;
    }

    // 게임 시작
    private void StartGame()
    {
        if (!IsServer) return;

        player1Score.Value = 0;
        player2Score.Value = 0;
        gameOver.Value = false;

        // 공 리셋
        if (ball != null)
        {
            ball.ResetBall();
        }

        // 패들 리셋
        foreach (var paddle in paddles)
        {
            if (paddle != null)
            {
                paddle.ResetPosition();
            }
        }
    }

    // 플레이어 득점 처리
    public void PlayerScored(int playerNumber)
    {
        if (!IsServer) return;

        if (gameOver.Value) return;

        if (playerNumber == 1)
        {
            player1Score.Value++;
        }
        else if (playerNumber == 2)
        {
            player2Score.Value++;
        }

        // 승리 조건 체크
        CheckWinCondition();
    }

    // 승리 조건 확인
    private void CheckWinCondition()
    {
        if (!IsServer) return;

        if (player1Score.Value >= winningScore || player2Score.Value >= winningScore)
        {
            gameOver.Value = true;
        }
    }

    // 게임 재시작
    public void RestartGame()
    {
        if (IsServer)
        {
            RestartGameServerRpc();
        }
        else
        {
            RestartGameServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RestartGameServerRpc()
    {
        StartGame();
    }

    // 메뉴로 돌아가기
    public void ReturnToMenu()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }
        SceneManager.LoadScene("MenuScene");
    }

    // 네트워크 변수 변경 이벤트 핸들러들
    private void OnPlayer1ScoreChanged(int oldScore, int newScore)
    {
        UpdateScoreUI();
    }

    private void OnPlayer2ScoreChanged(int oldScore, int newScore)
    {
        UpdateScoreUI();
    }

    private void OnGameOverChanged(bool oldValue, bool newValue)
    {
        UpdateGameStatusUI();
        
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(newValue);
        }
    }

    // UI 업데이트 메서드들
    private void UpdateScoreUI()
    {
        if (player1ScoreText != null)
            player1ScoreText.text = player1Score.Value.ToString();

        if (player2ScoreText != null)
            player2ScoreText.text = player2Score.Value.ToString();
    }

    private void UpdateGameStatusUI()
    {
        if (gameStatusText == null) return;

        if (gameOver.Value)
        {
            int winner = player1Score.Value >= winningScore ? 1 : 2;
            gameStatusText.text = $"Player {winner} Wins!";
        }
        else
        {
            gameStatusText.text = "Playing...";
        }
    }

    private void OnDestroy()
    {
        // 버튼 이벤트 해제
        if (restartButton != null)
            restartButton.onClick.RemoveListener(RestartGame);

        if (menuButton != null)
            menuButton.onClick.RemoveListener(ReturnToMenu);

        // 싱글톤 정리
        if (Instance == this)
        {
            Instance = null;
        }
    }
}