using Unity.Netcode;
using UnityEngine;

// 네트워크 동기화되는 공 컨트롤러
public class BallController : NetworkBehaviour
{
    [Header("Ball Settings")]
    public float moveSpeed = 5f;
    public float maxSpeed = 15f;
    public float speedIncrease = 0.1f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Vector2 currentVelocity;

    // 네트워크 변수로 공의 위치와 속도 동기화
    private NetworkVariable<Vector2> networkPosition = new NetworkVariable<Vector2>();
    private NetworkVariable<Vector2> networkVelocity = new NetworkVariable<Vector2>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // 서버에서만 공 초기화
            ResetBall();
        }

        // 네트워크 변수 변경 이벤트 구독
        networkPosition.OnValueChanged += OnPositionChanged;
        networkVelocity.OnValueChanged += OnVelocityChanged;
    }

    public override void OnNetworkDespawn()
    {
        // 이벤트 구독 해제
        networkPosition.OnValueChanged -= OnPositionChanged;
        networkVelocity.OnValueChanged -= OnVelocityChanged;
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            // 서버에서만 물리 시뮬레이션
            // 속도 제한
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            // 네트워크 변수 업데이트
            networkPosition.Value = transform.position;
            networkVelocity.Value = rb.velocity;
        }
        else
        {
            // 클라이언트는 서버의 위치/속도를 따라감
            transform.position = Vector2.Lerp(transform.position, networkPosition.Value, Time.fixedDeltaTime * 15f);
            rb.velocity = networkVelocity.Value;
        }
    }

    // 공 초기화
    public void ResetBall()
    {
        if (!IsServer) return;

        transform.position = startPosition;
        rb.velocity = Vector2.zero;

        // 랜덤한 방향으로 공 발사
        Invoke(nameof(LaunchBall), 1f);
    }

    private void LaunchBall()
    {
        if (!IsServer) return;

        float randomDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
        Vector2 force = new Vector2(randomDirection, Random.Range(-0.5f, 0.5f)).normalized * moveSpeed;
        rb.velocity = force;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        // 골대와 충돌 시 점수 처리
        if (collision.CompareTag("LeftGoal"))
        {
            PongGameManager.Instance?.PlayerScored(2); // Player 2 득점
            ResetBall();
        }
        else if (collision.CompareTag("RightGoal"))
        {
            PongGameManager.Instance?.PlayerScored(1); // Player 1 득점
            ResetBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        // 패들과 충돌 시 속도 증가
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Vector2 newVelocity = rb.velocity;
            newVelocity = newVelocity.normalized * (newVelocity.magnitude + speedIncrease);
            rb.velocity = newVelocity;
        }
    }

    // 네트워크 변수 변경 이벤트 핸들러
    private void OnPositionChanged(Vector2 oldPos, Vector2 newPos)
    {
        if (!IsServer)
        {
            // 클라이언트에서 위치 보간
            // FixedUpdate에서 처리되므로 여기서는 직접 설정하지 않음
        }
    }

    private void OnVelocityChanged(Vector2 oldVel, Vector2 newVel)
    {
        if (!IsServer)
        {
            // 클라이언트에서 속도 동기화
            // FixedUpdate에서 처리되므로 여기서는 직접 설정하지 않음
        }
    }
}