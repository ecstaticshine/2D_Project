using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;

    private GridManager gridManager;

    private void Awake()
    {
        Instance = this;
        gridManager = GridManager.Instance;
    }

    //상하좌우
    private readonly Vector2Int[] fourDirections = new Vector2Int[]
{
        Vector2Int.right,
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.down
};

    //파이어 엠블렘 방식 이동 범위 포함한 이동
    public HashSet<Vector2Int> GetMovableTiles(Vector2Int start, int moveRange)
    {
        //중복 안 되는 자료형 구조
        HashSet<Vector2Int> closedList = new HashSet<Vector2Int>();
        //openList
        Queue<(Vector2Int pos, int dist)> openList = new Queue<(Vector2Int, int)>();

        //자기 자신을 넣고 시작
        closedList.Add(start);
        //오픈 리스트에 넣기
        openList.Enqueue((start, 0));


        while (openList.Count > 0)
        {
            // 자기 자신 꺼내기 시작
            var (current, dist) = openList.Dequeue();

            //가려고 하는 범위보다 긴 곳을 선택하면 스킵
            if (dist >= moveRange)
                continue;

            //상하좌우 확인
            foreach (var dir in fourDirections)
            {
                // 지금 좌표에서 상하좌우 넣기
                Vector2Int next = current + dir;

                // 그 앞은 걸을 수 없으면 스킵
                if (!gridManager.IsWalkable(next))
                    continue;

                // 클로즈드 리스트에 포함 안 된 곳이면 
                if (!closedList.Contains(next))
                {
                    //해당 좌표를 클로즈드 리스트에 넣고
                    closedList.Add(next);
                    //  그 다음 좌표(+1)를 확인하러 다음 좌표를 오픈리스트에 넣음
                    openList.Enqueue((next, dist + 1));
                }
            }
        }
        return closedList;
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        // 열린 목록
        var openList = new Queue<Node>();
        // 닫힌 목록
        var closedList = new HashSet<Vector2Int>();

        Node startNode = new Node(start, 0, Heuristic(start, goal));
        openList.Enqueue(startNode);

        Dictionary<Vector2Int, Node> allNodes = new Dictionary<Vector2Int, Node>();
        allNodes[start] = startNode;

        while (openList.Count > 0)
        {
            var current = openList.Dequeue();

            if (current.pos == goal)
            {
                return ReconstructPath(current);
            }

            closedList.Add(current.pos);

            foreach (var dir in fourDirections)
            {
                Vector2Int nextPos = current.pos + dir;

                if (!gridManager.IsWalkable(nextPos))
                    continue;

                if (closedList.Contains(nextPos))
                    continue;

                int newG = current.G + 1; // 이동 비용

                if (!allNodes.TryGetValue(nextPos, out Node nextNode))
                {
                    nextNode = new Node(nextPos, newG, Heuristic(nextPos, goal));
                    nextNode.ParentNode = current;

                    allNodes[nextPos] = nextNode;
                    openList.Enqueue(nextNode);
                }
               
            }
        }

        return null; // 경로 없음
    }

    private int Heuristic(Vector2Int a, Vector2Int b)
    {
        // 맨해튼 거리 (상하좌우 이동에 최적)
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> ReconstructPath(Node endNode)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        var current = endNode;

        while (current != null)
        {
            result.Add(current.pos);
            current = current.ParentNode;
        }

        result.Reverse();
        return result;
    }
}

