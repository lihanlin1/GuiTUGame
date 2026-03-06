using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("生成设置")]
    public GameObject obstaclePrefab;
    public GameObject coinPrefab;
    public float spawnInterval = 2f;
    public float obstacleSpeed = 5f;

    [Header("跑道设置")]
    public float bottomLaneY = 0f;
    public float topLaneY = 2f;
    public float spawnDistanceAhead = 15f;  // 在玩家前方多少距离生成
    public float despawnX = -10f;            // 物体消失的X坐标

    [Header("玩家引用")]
    public Transform playerTransform;        // 玩家的Transform

    private float timer;

    void Start()
    {
        timer = spawnInterval;

        // 如果没有手动设置玩家，尝试自动查找
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("找不到玩家对象！请手动设置playerTransform或在玩家身上添加'Player'标签");
            }
        }
    }

    void Update()
    {
        // 如果没有玩家引用，不生成物体
        if (playerTransform == null) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SpawnObject();
            timer = Random.Range(spawnInterval * 0.8f, spawnInterval * 1.2f);
        }
    }

    void SpawnObject()
    {
        // 随机选择跑道 (0: 下方, 1: 上方)
        int lane = Random.Range(0, 2);
        float laneY = lane == 0 ? bottomLaneY : topLaneY;

        // 随机决定生成障碍物还是金币 (70%障碍物, 30%金币)
        bool spawnObstacle = Random.Range(0f, 1f) > 0.3f;
        GameObject prefab = spawnObstacle ? obstaclePrefab : coinPrefab;

        // 计算生成位置 - 在玩家前方固定距离
        Vector3 spawnPosition = new Vector3(
            playerTransform.position.x + spawnDistanceAhead,  // 玩家位置 + 前方距离
            laneY,
            0
        );

        // 生成物体
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // 添加移动脚本
        MovingObject movingObj = obj.AddComponent<MovingObject>();
        movingObj.speed = obstacleSpeed;
        movingObj.despawnX = despawnX;  // 传递消失位置

        // 设置标签
        obj.tag = spawnObstacle ? "Obstacle" : "Coin";
    }

    // 在Scene视图中显示生成范围（用于调试）
    void OnDrawGizmosSelected()
    {
        if (playerTransform != null)
        {
            // 显示生成位置范围
            Gizmos.color = Color.yellow;
            Vector3 spawnPosBottom = new Vector3(
                playerTransform.position.x + spawnDistanceAhead,
                bottomLaneY,
                0
            );
            Vector3 spawnPosTop = new Vector3(
                playerTransform.position.x + spawnDistanceAhead,
                topLaneY,
                0
            );

            Gizmos.DrawWireSphere(spawnPosBottom, 0.5f);
            Gizmos.DrawWireSphere(spawnPosTop, 0.5f);
            Gizmos.DrawLine(spawnPosBottom, spawnPosTop);
        }
    }
}