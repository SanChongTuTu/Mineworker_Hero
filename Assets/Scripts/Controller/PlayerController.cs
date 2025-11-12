using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("角色属性")]
    public float moveSpeed = 5f;
    public float digRange = 1f;
    public float gravityScale = 2f;

    [Header("玩家属性")]
    public int health = 4;
    public int attack = 1;
    public int maxHealth = 4;

    [Header("角色朝向")]
    public CharacterDirection currentDirection = CharacterDirection.Down;

    [Header("UI显示")]
    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Text attackText;
    public UnityEngine.UI.Text directionText;

    private Rigidbody2D rb;
    private Vector2 digDirection = Vector2.down;
    private UltraSimpleOre targetOre;
    private bool isGrounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 设置物理属性
        if (rb != null)
        {
            rb.gravityScale = gravityScale;
            rb.freezeRotation = true;
        }

        Debug.Log("玩家控制器启动");
        UpdateUI();
    }

    void Update()
    {
        HandleMovement();
        HandleDirectionInput();
        FindTargetOre();
        HandleDigging();
        CheckGrounded();
        UpdateUI();

        // 调试可视化
        Debug.DrawRay(transform.position, digDirection * digRange, GetDirectionColor());
    }

    void HandleMovement()
    {
        // 只能左右移动，不能上下移动
        float horizontal = Input.GetAxis("Horizontal");

        // 水平移动速度
        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * moveSpeed;

        // 保持重力产生的下落速度
        rb.velocity = velocity;
    }

    void HandleDirectionInput()
    {
        // A/D控制左右转向，S控制向下转向
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentDirection = CharacterDirection.Left;
            digDirection = Vector2.left;
            Debug.Log("面向左边");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentDirection = CharacterDirection.Right;
            digDirection = Vector2.right;
            Debug.Log("面向右边");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentDirection = CharacterDirection.Down;
            digDirection = Vector2.down;
            Debug.Log("面向下边");
        }

        // 移动时也自动转向（可选）
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > 0 && currentDirection != CharacterDirection.Right)
        {
            currentDirection = CharacterDirection.Right;
            digDirection = Vector2.right;
        }
        else if (horizontal < 0 && currentDirection != CharacterDirection.Left)
        {
            currentDirection = CharacterDirection.Left;
            digDirection = Vector2.left;
        }
    }

    void FindTargetOre()
    {
        // 使用OverlapCircle检测挖掘位置的矿石
        Vector2 digPosition = (Vector2)transform.position + digDirection * digRange;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(digPosition, 0.3f);

        UltraSimpleOre newTargetOre = null;

        foreach (Collider2D collider in hitColliders)
        {
            // 检查标签
            if (collider.CompareTag("Ore"))
            {
                UltraSimpleOre newOre = collider.GetComponent<UltraSimpleOre>();
                if (newOre != null)
                {
                    newTargetOre = newOre;
                    break;
                }
            }
        }

        // 如果目标发生变化
        if (newTargetOre != targetOre)
        {
            // 停止挖掘之前的矿石
            if (targetOre != null)
            {
                targetOre.StopDigging();
            }

            targetOre = newTargetOre;

            if (targetOre != null)
            {
                Debug.Log($"发现目标矿石: {targetOre.oreType}，方向: {GetDirectionName()}");
            }
        }
    }

    void HandleDigging()
    {
        // 按下J键开始挖掘
        if (Input.GetKeyDown(KeyCode.J) && targetOre != null)
        {
            Debug.Log($"开始挖掘: {targetOre.oreType}，方向: {GetDirectionName()}");
            targetOre.StartDigging();
        }

        // 按住J键持续挖掘
        if (Input.GetKey(KeyCode.J) && targetOre != null)
        {
            targetOre.ContinueDigging();
        }

        // 松开J键停止挖掘
        if (Input.GetKeyUp(KeyCode.J) && targetOre != null)
        {
            Debug.Log("停止挖掘");
            targetOre.StopDigging();
        }
    }

    void CheckGrounded()
    {
        // 检测是否站在地面上
        Vector2 rayStart = (Vector2)transform.position + Vector2.down * 0.5f;
        float rayLength = 0.6f;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, rayLength);
        isGrounded = hit.collider != null;

        // 调试射线
        Debug.DrawRay(rayStart, Vector2.down * rayLength, isGrounded ? Color.green : Color.yellow);
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = "生命: " + health;

        if (attackText != null)
            attackText.text = "攻击: " + attack;

        if (directionText != null)
            directionText.text = "朝向: " + GetDirectionName();
    }

    string GetDirectionName()
    {
        return currentDirection switch
        {
            CharacterDirection.Down => "向下",
            CharacterDirection.Left => "向左",
            CharacterDirection.Right => "向右",
            _ => "未知"
        };
    }

    Color GetDirectionColor()
    {
        return currentDirection switch
        {
            CharacterDirection.Down => Color.red,      // 向下 - 红色
            CharacterDirection.Left => Color.blue,     // 向左 - 蓝色
            CharacterDirection.Right => Color.green,   // 向右 - 绿色
            _ => Color.white
        };
    }

    // 当矿石被挖掉时调用
    public void OnOreDestroyed(UltraSimpleOre ore)
    {
        if (targetOre == ore)
        {
            targetOre = null;
        }

        // 矿石被挖掉后检查是否需要下落
        CheckFallAfterMining();
    }

    void CheckFallAfterMining()
    {
        // 检查玩家下方是否有支撑
        Vector2 rayStart = (Vector2)transform.position + Vector2.down * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, 1f);

        if (hit.collider == null)
        {
            Debug.Log("下方矿石被挖掉，开始下落");
            // 重力会自动让玩家下落
        }
    }

    // 应用矿石效果后调用
    public void RefreshStats()
    {
        // 确保生命值不超过最大值
        health = Mathf.Min(health, maxHealth);
        UpdateUI();
    }

    // 碰撞检测
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ore"))
        {
            Debug.Log("玩家落地");
        }
    }

    // 防止玩家用W键向上移动
    private void FixedUpdate()
    {
        // 确保玩家不能通过W键或其他方式向上移动
        // 只允许重力作用在Y轴上
        if (rb.velocity.y > 0 && !isGrounded)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = Mathf.Min(velocity.y, 0); // 限制向上的速度
            rb.velocity = velocity;
        }
    }
}

public enum CharacterDirection
{
    Down,
    Left,
    Right
}