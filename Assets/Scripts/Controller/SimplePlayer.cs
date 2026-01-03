using System.Collections;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private SimpleOre targetOre;
    private Vector2 currentDirection = Vector2.down;

    public static bool canmove;

    [Header("移动设置")]
    public float baseMoveSpeed = 5f; // 基础移动速度


    [Header("检测设置")]
    public float rayDistance = 1.5f;
    public float rayStartOffset = 0.3f;


    [Header("UI引用")]
    public ResultPanelUI resultPanel; // 直接引用场景中的面板


    public static Rigidbody2D rb;
    private float currentMoveSpeed; // 当前实际移动速度
    private PlayerAnimationController animationController;
    public static SpriteRenderer sr;
    private bool isDead = false;

    private static SimplePlayer instance;
    public static SimplePlayer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SimplePlayer>();
                if (instance == null)
                {
                    Debug.Log("No SimplePlayer!");
                }
            }
            return instance;
        }
    }


    void Start()
    {
        canmove = true;
        rb = GetComponent<Rigidbody2D>();
        sr=transform.GetChild(0).GetComponent<SpriteRenderer>();

        // 设置Rigidbody2D属性，减少滑动但保持正常物理


        // 初始化移动速度
        UpdateMoveSpeedFromController();

        animationController = GetComponent<PlayerAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<PlayerAnimationController>();
        }

    }

    void Update()
    {
        if (isDead) return; // 死亡后不再响应输入

        // 实时更新移动速度（确保movespeed变化时立即生效）
        UpdateMoveSpeedFromController();

        if (canmove)
        {
            HandleMovement();

            // 转向
            HandleDirectionInput();

            float h = Input.GetAxis("Horizontal");
            if (h != 0)
            {
                sr.flipX = h > 0;
            }
        }

        // 找矿石
        FindOre();

        // 挖矿控制
        if (canmove)
        {
            HandleDigging();
        }

        // 可视化射线
        Vector2 rayStart = (Vector2)transform.position + currentDirection * rayStartOffset;
        Debug.DrawRay(rayStart, currentDirection * (rayDistance - rayStartOffset), GetDirectionColor());

        // 检测挖矿状态变化
        bool isMining = Input.GetKey(KeyCode.J);

        // 更新动画（如果动画控制器存在）
        if (animationController != null)
        {
            if (isMining && animationController.GetCurrentState() != "Mining")
            {
                animationController.StartMiningAnimation();
            }
        }

        // 检查生命值
        CheckHealth();
    }


    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float h = Input.GetAxis("Horizontal");

            // 使用当前实际移动速度
            rb.velocity = new Vector2(h * currentMoveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void UpdateMoveSpeedFromController()
    {
        if (GameDateController.Instance != null)
        {
            // movespeed = 1.0 表示基础速度，movespeed = 1.5 表示速度提升50%
            currentMoveSpeed = baseMoveSpeed * GameDateController.Instance.movespeed;
        }
        else
        {
            currentMoveSpeed = baseMoveSpeed;
            Debug.LogWarning("GameDateController未找到，使用基础移动速度");
        }
    }

    void HandleDirectionInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentDirection = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = Vector2.right;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = Vector2.down;
        }
    }

    void FindOre()
    {
        Vector2 rayStart = (Vector2)transform.position + currentDirection * rayStartOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, currentDirection, rayDistance - rayStartOffset);

        SimpleOre newTarget = null;

        if (hit.collider != null && hit.collider.CompareTag("Ore"))
        {
            newTarget = hit.collider.GetComponent<SimpleOre>();
        }

        if (newTarget != targetOre)
        {
            if (targetOre != null)
            {
                targetOre.StopDigging();
                if (animationController != null && !Input.GetKey(KeyCode.J))
                {
                    animationController.StopMiningAnimation();
                }
            }

            targetOre = newTarget;
        }
    }

    void HandleDigging()
    {
        if (targetOre == null)
        {
            // 没有目标矿石时停止挖矿动画
            if (animationController != null && animationController.GetCurrentState() == "Mining")
            {
                animationController.StopMiningAnimation();
            }
            return;
        }

            if (Input.GetKey(KeyCode.J))
            {
                targetOre.StartDigging();
                // 开始挖矿动画
                if (animationController != null)
                {
                    animationController.StartMiningAnimation();
                }
            }
            else if (Input.GetKeyUp(KeyCode.J))
            {
                targetOre.StopDigging();
                // 停止挖矿动画
                if (animationController != null)
                {
                    animationController.StopMiningAnimation();
                }
            }
    }

    Color GetDirectionColor()
    {
        if (currentDirection == Vector2.down) return Color.red;
        if (currentDirection == Vector2.left) return Color.blue;
        if (currentDirection == Vector2.right) return Color.green;
        return Color.white;
    }


    public static IEnumerator PlayerHurt()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        sr.color = Color.white;
        yield break;
    }

    // 受到伤害
    public static void TakeDamage(int damage)
    {
        if (instance.isDead) return;

        GameDateController.Instance.blood -= damage;
        Debug.Log($"玩家受到{damage}点伤害，剩余生命值: {GameDateController.Instance.blood}");

        instance.CheckHealth();
    }

    // 检查生命值
    void CheckHealth()
    {
        if (FindObjectOfType<GameDateController>() != null)
        {
            if (GameDateController.Instance.blood <= 0 && !isDead)
            {
                GameDateController.Instance.blood = 0;
                Die();
            }
        }
        
    }

    // 玩家死亡
    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("玩家死亡");

        // 停止所有动作
        rb.velocity = Vector2.zero;

        // 停止动画
        if (animationController != null)
        {
            animationController.StopMiningAnimation();
            animationController.StopMovingAnimation();
        }

        // 显示失败面板（只显示返回按钮）
        ShowDefeat();
    }

    // 显示失败面板
    void ShowDefeat()
    {
        if (resultPanel != null)
        {
            // 不再传递矿石数量，脚本内部固定为10
            resultPanel.gameObject.SetActive(true);
            resultPanel.ShowDefeat();
        }
        else
        {
            Debug.LogError("结果面板引用未设置！");
        }
    }

    // 计算获得的超能矿石
    int CalculateOreGained()
    {
        return Random.Range(0, 10);
    }

    // 获取当前移动速度
    public float GetCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }

    // 获取当前生命值
    public float GetCurrentHealth()
    {
        return GameDateController.Instance.blood;
    }

    // 立即停止移动（外部调用）
    public void StopImmediately()
    {
        rb.velocity = new Vector2(0, rb.velocity.y); // 只停止水平移动
    }
}