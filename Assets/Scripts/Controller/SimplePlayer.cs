using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private SimpleOre targetOre;
    private Vector2 currentDirection = Vector2.down;

    [Header("移动设置")]
    public float baseMoveSpeed = 5f; // 基础移动速度


    [Header("检测设置")]
    public float rayDistance = 1.5f;
    public float rayStartOffset = 0.3f;


    [Header("UI引用")]
    public ResultPanelUI resultPanel; // 直接引用场景中的面板


    private Rigidbody2D rb;
    private float currentMoveSpeed; // 当前实际移动速度
    private PlayerAnimationController animationController;
    private SpriteRenderer sr;
    private bool isDead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


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

        // 转向
        HandleDirectionInput();

        // 找矿石
        FindOre();

        // 挖矿控制
        HandleDigging();

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

        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            sr.flipX = h > 0;
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

    // 受到伤害
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"玩家受到{damage}点伤害，剩余生命值: {currentHealth}");

        CheckHealth();
    }

    // 检查生命值
    void CheckHealth()
    {
        if (currentHealth <= 0 && !isDead)
        {
            currentHealth = 0;
            Die();
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
        currentHorizontalVelocity = 0f;
        targetHorizontalVelocity = 0f;

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
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    // 立即停止移动（外部调用）
    public void StopImmediately()
    {
        rb.velocity = new Vector2(0, rb.velocity.y); // 只停止水平移动
        currentHorizontalVelocity = 0f;
        targetHorizontalVelocity = 0f;
    }
}