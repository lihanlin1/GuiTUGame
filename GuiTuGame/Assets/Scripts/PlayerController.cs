using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    public float runSpeed = 8f;
    public float laneChangeSpeed = 10f;
    public float laneWidth = 2f;  // 跑道之间的垂直距离

    [Header("跑道设置")]
    public int currentLane = 0;  // 0: 下方, 1: 上方
    public float bottomLaneY = 0f;
    public float topLaneY = 2f;

    [Header("血量设置")]
    public GameObject heartUI;
    public float heart = 3f;


    private float targetY;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isChangingLane = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 初始化位置
        targetY = bottomLaneY;
        transform.position = new Vector3(transform.position.x, bottomLaneY, 0);
    }

    void Update()
    {
        // 检测空格键切换跑道
        if (Input.GetKeyDown(KeyCode.Space) && !isChangingLane)
        {
            SwitchLane();
        }

        // 平滑移动到目标Y位置
        if (isChangingLane)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetY, laneChangeSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, 0);

            // 检查是否到达目标位置
            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                transform.position = new Vector3(transform.position.x, targetY, 0);
                isChangingLane = false;

                // 触发动画
                if (animator != null)
                {
                    animator.SetBool("isChanging", false);
                }
            }
        }

        // 自动向前移动
        transform.Translate(Vector3.right * runSpeed * Time.deltaTime);
    }

    void SwitchLane()
    {
        // 切换跑道
        currentLane = 1 - currentLane;  // 在0和1之间切换
        targetY = currentLane == 0 ? bottomLaneY : topLaneY;
        isChangingLane = true;

        // 触发切换动画
        if (animator != null)
        {
            animator.SetBool("isChanging", true);
            animator.SetTrigger("switchLane");
        }

        // 可以添加切换音效
        // AudioSource.PlayClipAtPoint(switchSound, transform.position);
    }

    // 碰撞检测
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {

            // 游戏结束
            Debug.Log("游戏结束！");
            Time.timeScale = 0f;  // 暂停游戏
        }
        else if (other.CompareTag("Coin"))
        {
            // 收集金币
            Destroy(other.gameObject);
            Debug.Log("收集到金币！");
        }
    }
}