using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private SimplePlayer playerController;
    private SimpleOre currentOre;

    // 动画参数ID（性能更好）
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private readonly int isMiningHash = Animator.StringToHash("IsMining");
    private readonly int isDownMiningHash = Animator.StringToHash("IsDownMining");

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<SimplePlayer>();

        if (animator == null)
        {
            Debug.LogError("PlayerAnimationController: Animator组件未找到！");
        }
    }

    void Update()
    {
        if (animator == null || playerController == null) return;

        // 检测是否在移动
        bool isMoving = IsPlayerMoving();

        // 检测是否在挖矿
        bool isMining = IsPlayerMining();

        // 更新动画参数
        UpdateAnimationParameters(isMoving, isMining);

        // 输出当前状态（调试用）
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"动画状态: 移动={isMoving}, 挖矿={isMining}");
        }
    }

    // 检测玩家是否在移动
    bool IsPlayerMoving()
    {
        // 方法1：通过Rigidbody速度
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            return Mathf.Abs(rb.velocity.x) > 0.1f;
        }

        // 方法2：通过输入
        float horizontalInput = Input.GetAxis("Horizontal");
        return Mathf.Abs(horizontalInput) > 0.1f;
    }

    // 检测玩家是否在挖矿
    bool IsPlayerMining()
    {
        // 检查是否按着挖矿键
        if (SimplePlayer.canmove)
        {
            if (Input.GetKey(KeyCode.J))
            {
                return true;
            }
        }

        // 或者检查是否有目标矿石正在被挖掘
        // 这需要修改SimplePlayer脚本来暴露currentOre
        return false;
    }

    // 更新动画参数
    void UpdateAnimationParameters(bool isMoving, bool isMining)
    {
        // 优先级：挖矿 > 移动 > 待机
        if (isMining)
        {
            animator.SetBool(isMiningHash, true);
            if(SimplePlayer.Instance.currentDirection == Vector2.down)
            {
                animator.SetBool(isDownMiningHash, true);
            }
            else
            {
                animator.SetBool(isDownMiningHash, false);
            }
            animator.SetBool(isMovingHash, false);
        }
        else if (isMoving)
        {
            animator.SetBool(isMovingHash, true);
            animator.SetBool(isMiningHash, false);
            animator.SetBool(isDownMiningHash, false);
        }
        else
        {
            animator.SetBool(isMovingHash, false);
            animator.SetBool(isMiningHash, false);
            animator.SetBool(isDownMiningHash, false);
        }
    }

    // 外部调用：强制设置挖矿动画
    public void StartMiningAnimation()
    {
        if (SimplePlayer.Instance.currentDirection == Vector2.down)
        {
            animator.SetBool(isDownMiningHash, true);
        }
        else
        {
            animator.SetBool(isDownMiningHash, false);
        }
        animator.SetBool(isMiningHash, true);
        animator.SetBool(isMovingHash, false);
    }

    // 外部调用：强制停止挖矿动画
    public void StopMiningAnimation()
    {
        animator.SetBool(isMiningHash, false);
        animator.SetBool(isDownMiningHash, false);
    }

    // 外部调用：开始移动动画
    public void StartMovingAnimation()
    {
        animator.SetBool(isMovingHash, true);
        animator.SetBool(isMiningHash, false);
        animator.SetBool(isDownMiningHash, false);
    }

    // 外部调用：停止移动动画
    public void StopMovingAnimation()
    {
        animator.SetBool(isMovingHash, false);
    }

    // 获取当前动画状态
    public string GetCurrentState()
    {
        if (animator == null) return "Idle";
        if (animator.GetBool(isMiningHash) || animator.GetBool(isDownMiningHash))
            return "Mining";
        if (animator.GetBool(isMovingHash))
            return "Moving";
        return "Idle";
    }
}