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

    private Rigidbody2D rb;
    private float currentMoveSpeed; // 当前实际移动速度
    private PlayerAnimationController animationController;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0; // 设置为无重力
        }

        // 初始化移动速度
        UpdateMoveSpeedFromController();
        animationController = GetComponent<PlayerAnimationController>();
        if (animationController == null)
        {
            animationController = gameObject.AddComponent<PlayerAnimationController>();
        }
        sr=transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 实时更新移动速度（确保movespeed变化时立即生效）
        UpdateMoveSpeedFromController();

        // 移动
        HandleMovement();

        // 转向
        HandleDirectionInput();

        // 找矿石
        FindOre();

        // 挖矿控制
        HandleDigging();

        // 可视化射线
        Vector2 rayStart = (Vector2)transform.position + currentDirection * rayStartOffset;
        Debug.DrawRay(rayStart, currentDirection * (rayDistance - rayStartOffset), GetDirectionColor());
        float horizontalInput = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        // 检测挖矿状态变化
        bool isMining = Input.GetKey(KeyCode.J);

        // 更新动画（如果动画控制器存在）
        if (animationController != null)
        {
            if (isMining && animationController.GetCurrentState() != "Mining")
            {
                animationController.StartMiningAnimation();
            }
            else if (isMoving && animationController.GetCurrentState() != "Moving")
            {
                animationController.StartMovingAnimation();
            }
        }

        float h = Input.GetAxis("Horizontal");
        if (h != 0)
        {
            sr.flipX = h > 0;   
        }
    }

    void HandleMovement()
    {
        if(Input.GetKey(KeyCode.A)||Input.GetKey(KeyCode.D))
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
            if (newTarget != null)
            {
                //Debug.Log($"找到矿石: 类型={newTarget.oreType}");
            }
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

    // 公开方法：获取当前移动速度（用于UI显示等）
    public float GetCurrentMoveSpeed()
    {
        return currentMoveSpeed;
    }
}