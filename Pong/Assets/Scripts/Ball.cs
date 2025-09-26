using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    private Vector2 direction; // 이동 방향
    private const float StartSpeed = 3f; // 최초 이동 속도
    private const float MaxSpeed = 15f; // 최대 이동 속도
    private const float AdditionalSpeedPerHit = 0.2f; // 충돌시 추가되는 속도
    private float currentSpeed = StartSpeed; // 현재 속도

    [SerializeField] private Rigidbody2D _rb;

    // 최초 공의 방향을 결정
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        // 게임 시작시 공의 이동 방향은
        // 무작위성이 조금 추가된 왼쪽 방향
        direction =
            (Vector2.left + Random.insideUnitCircle).normalized;
    }

    private void FixedUpdate()
    {
        // 서버가 아니거나 게임이 종료된 경우 이동 처리를 하지 않음
        if (!IsServer || !GameManager.Instance.IsGameActive)
        {
            return;
        }

        _rb.MovePosition(
            gameObject.transform.position + (Vector3)(direction * currentSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer || !GameManager.Instance.IsGameActive) return;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            var paddlePosition = collision.transform.position;
            var contactPoint = collision.GetContact(0).point;

            var offset = contactPoint.y - paddlePosition.y;
            var height = collision.collider.bounds.size.y;
            var normalOffset = offset / (height / 2f);
            var bounceAngle = normalOffset * 45f;
            var angleRad = bounceAngle * Mathf.Deg2Rad;
            var newDirection = new Vector2(
                direction.x > 0 ? -Mathf.Cos(angleRad) : Mathf.Cos(angleRad),
                Mathf.Sin(angleRad)).normalized;
            direction = newDirection;

            currentSpeed += AdditionalSpeedPerHit;
            if (currentSpeed > MaxSpeed)
            {
                currentSpeed = MaxSpeed;
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y = -direction.y;

        }
        else if (collision.gameObject.CompareTag("ScoringZone"))
        {
            if (collision.transform.position.x < 0f)
            {
                GameManager.Instance.AddScore(1, 1);
                direction = (Vector2.right + Random.insideUnitCircle).normalized;
            }
            else
            {
                GameManager.Instance.AddScore(0, 1);
                direction = (Vector2.left + Random.insideUnitCircle).normalized;
            }
            transform.position = new Vector2(0f, Random.Range(-3f, 3f));
            currentSpeed = StartSpeed;
        }
    }
}