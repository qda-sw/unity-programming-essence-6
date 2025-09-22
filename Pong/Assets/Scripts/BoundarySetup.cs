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
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        
        // 렌더러 비활성화 (보이지 않는 벽)
        Renderer renderer = wall.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Collider는 유지하여 충돌 감지
        BoxCollider collider = wall.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }
    }

    private void CreateGoal(string name, Vector3 position, Vector3 scale, string tag)
    {
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.name = name;
        goal.transform.position = position;
        goal.transform.localScale = scale;
        goal.tag = tag;
        
        // 렌더러 비활성화 (보이지 않는 골대)
        Renderer renderer = goal.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Trigger로 설정하여 공이 통과할 때 감지
        BoxCollider collider = goal.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    // 기즈모로 경계 표시 (에디터에서만 보임)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
        // 게임 영역 표시
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(gameWidth, gameHeight, 0));
        
        // 벽 위치 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, gameHeight / 2, 0), new Vector3(gameWidth, wallThickness, 1));
        Gizmos.DrawWireCube(new Vector3(0, -gameHeight / 2, 0), new Vector3(gameWidth, wallThickness, 1));
        
        // 골대 위치 표시
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(-gameWidth / 2 - wallThickness / 2, 0, 0), new Vector3(wallThickness, gameHeight, 1));
        Gizmos.DrawWireCube(new Vector3(gameWidth / 2 + wallThickness / 2, 0, 0), new Vector3(wallThickness, gameHeight, 1));
    }
}