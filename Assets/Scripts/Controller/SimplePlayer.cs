using System.Collections;
using UnityEngine;

public class SimplePlayer : MonoBehaviour
{
    private SimpleOre targetOre;
    public Vector2 currentDirection = Vector2.down;

    public static bool canmove;

    [Header("移动设置")]
    public float baseMoveSpeed = 5f; // 基础移动速度
    [Header("超能矿石计算器")]
    public CrystalCalc crystalCalculator;

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


    public static IEnumerator PlayerHurt(int damage)
    {
        Color color = damage > 0 ? Color.red : Color.green;
        sr.color = color;
        yield return new WaitForSeconds(0.25f);
        sr.color = Color.white;
        yield break;
    }

    // 受到伤害
    public void TakeDamage(int damage)
    {
        if (instance.isDead) return;
        StartCoroutine(PlayerHurt(damage));
        StartCoroutine(Takedamage(damage));
    }

    IEnumerator Takedamage(int damage)
    {
        damage=Mathf.Clamp(damage, (int)(GameDateController.Instance.blood-GameDateController.Instance.maxblood), (int)GameDateController.Instance.blood);
        Color color=damage>=0 ? Color.red : Color.green;
        UIManager.Instance.bloodText.color = color;
        GameDateController.Instance.blood -= damage;
        instance.CheckHealth();
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.bloodText.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.bloodText.color = color;
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.bloodText.color = Color.white;
        yield break;
    }

    public void AddATK(int num)
    {
        StartCoroutine(AddaTK(num));
    }

    IEnumerator AddaTK(int num)
    {
        num = Mathf.Clamp(num, (int)(1-GameDateController.Instance.attack- GameDateController.Instance.tempAttackBonus ), (int)(99 - GameDateController.Instance.attack- GameDateController.Instance.tempAttackBonus));
        Color color = num >= 0 ? Color.green : Color.red;
        BattleController.Instance.playerATKtext.color = color;
        GameDateController.Instance.tempAttackBonus+=num;
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.attackText.color = Color.white;
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.attackText.color = color;
        yield return new WaitForSeconds(0.25f);
        UIManager.Instance.attackText.color = Color.white;
        yield break;
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

    int CalculateOreGained()
    {
        // 如果有计算器，使用计算器
        if (crystalCalculator != null)
        {
            // 获取当前怪物信息
            if (MonsterInfoUI.Instance != null && MonsterInfoUI.Instance.monster != null)
            {
                Monster monster = MonsterInfoUI.Instance.monster;

                // 获取当前层数
                int currentLayer = MonsterInfoUI.Instance.currentFloor;

                // 获取怪物最大血量（使用maxblood）
                int monsterMaxHP = monster.maxblood;

                // 怪物当前血量（玩家死亡时怪物还活着）
                int monsterCurrentHP = monster.MonsterHP;

                // 玩家死亡，所以没有击败怪物
                bool isDefeated = false;

                // 计算超能矿石
                return crystalCalculator.CalcLayerCrystals(
                    currentLayer,
                    monsterMaxHP,
                    monsterCurrentHP,
                    isDefeated
                );
            }
            else
            {
                Debug.LogWarning("无法获取怪物信息，使用默认计算");
                // 没有怪物信息时，只获得挖矿结束的1个
                return 1;
            }
        }
        else
        {
            Debug.LogWarning("crystalCalculator未设置，使用随机值");
            return Random.Range(0, 10);
        }
    }

    // 显示失败面板
    void ShowDefeat()
    {
        if (resultPanel != null)
        {
            // 计算超能矿石
            int crystalCount = CalculateOreGained();

            // 设置矿石数量并显示
            resultPanel.number = crystalCount;
            resultPanel.ShowDefeat(crystalCount);
        }
        else
        {
            Debug.LogError("结果面板引用未设置！");
        }
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