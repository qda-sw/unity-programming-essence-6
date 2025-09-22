using UnityEngine;

// 게임 경계와 골대를 설정하는 스크립트
public class BoundarySetup : MonoBehaviour
{
    [Header("Boundary Settings")]
    public float gameWidth = 16f;
    public float gameHeight = 10f;
    public float wallThickness = 0.5f;

    private void Start()
    {
        CreateBoundaries();
    }

    private void CreateBoundaries()
    {
        // 상단 벽 생성
        CreateWall("TopWall", 
            new Vector3(0, gameHeight / 2, 0), 
            new Vector3(gameWidth, wallThickness, 1));

        // 하단 벽 생성
        CreateWall("BottomWall", 
            new Vector3(0, -gameHeight / 2, 0), 
            new Vector3(gameWidth, wallThickness, 1));

        // 좌측 골대 생성
        CreateGoal("LeftGoal", 
            new Vector3(-gameWidth / 2 - wallThickness / 2, 0, 0), 
            new Vector3(wallThickness, gameHeight, 1),
            "LeftGoal");

        // 우측 골대 생성
        CreateGoal("RightGoal", 
            new Vector3(gameWidth / 2 + wallThickness / 2, 0, 0), 
            new Vector3(wallThickness, gameHeight, 1),
            "RightGoal");
    }

    private void CreateWall(string name, Vector3 position, Vector3 scale)
    {
        GameObject wall = new GameObject(name);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        // 2D Box Collider 추가
        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(scale.x, scale.y);
        collider.isTrigger = false;
    }

    private void CreateGoal(string name, Vector3 position, Vector3 scale, string tag)
    {
        GameObject goal = new GameObject(name);
        goal.name = name;
        goal.transform.position = position;
        goal.transform.localScale = scale;
        goal.tag = tag;
        
        // 2D Box Collider를 Trigger로 설정
        BoxCollider2D collider = goal.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(scale.x, scale.y);
        collider.isTrigger = true;
    }

    // 기즈모로 경계 표시 (에디터에서만 보임)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        // 게임 영역 표시
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gameWidth, gameHeight, 0));
        
        // 벽 위치 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, gameHeight / 2, 0), new Vector3(gameWidth, wallThickness, 0));
        Gizmos.DrawWireCube(new Vector3(0, -gameHeight / 2, 0), new Vector3(gameWidth, wallThickness, 0));
        
        // 골대 위치 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(-gameWidth / 2 - wallThickness / 2, 0, 0), new Vector3(wallThickness, gameHeight, 0));
        Gizmos.DrawWireCube(new Vector3(gameWidth / 2 + wallThickness / 2, 0, 0), new Vector3(wallThickness, gameHeight, 0));
    }
}