// UltraSimpleOre.cs - 超级简单的矿石
using UnityEngine;


public class UltraSimpleOre : MonoBehaviour
{
    
    [Header("矿石类型")]
    public string oreType = "Normal";

    private float digProgress = 0f;
    private bool isBeingDug = false;

    void Start()
    {
        // 自动设置标签（如果还没设置）
        if (gameObject.tag != "Ore")
        {
            gameObject.tag = "Ore";
        }
        Debug.Log($"矿石创建: {oreType}, 位置: {transform.position}");
    }

    void Update()
    {
        if (isBeingDug)
        {
            digProgress += Time.deltaTime;
            Debug.Log($"挖掘 {oreType} 进度: {digProgress:F1}/1.0");

            if (digProgress >= 1f)
            {
                CompleteDigging();
            }
        }
    }

    public void StartDigging()
    {
        if (!isBeingDug && oreType != "Hard")
        {
            isBeingDug = true;
            digProgress = 0f;
            Debug.Log($"开始挖掘 {oreType}");
        }
        else if (oreType == "Hard")
        {
            Debug.Log("这是坚硬矿石，无法挖掘！");
        }
    }

    public void ContinueDigging()
    {
        // 持续挖掘逻辑在Update中处理
    }

    public void StopDigging()
    {
        if (isBeingDug)
        {
            isBeingDug = false;
            digProgress = 0f;
            Debug.Log($"停止挖掘 {oreType}");
        }
    }

    void CompleteDigging()
    {
        Debug.Log($"完成挖掘: {oreType}");
        ApplyOreEffect();
        DestroyOre();
    }

    void ApplyOreEffect()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("找不到玩家！");
            return;
        }

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("玩家没有PlayerController组件！");
            return;
        }

        switch (oreType)
        {
            case "Ruby":
                // 红宝石效果
                playerController.health += 2;
                Debug.Log("红宝石！生命值+2");
                break;

            case "Blue":
                // 蓝宝石效果
                playerController.attack += 1;
                Debug.Log("蓝宝石！攻击力+1");
                break;

            case "Purple":
                // 紫水晶效果
                playerController.health += 3;
                playerController.attack += 2;
                Debug.Log("紫水晶！生命+3，攻击+2");
                break;

            case "Lava":
                // 熔岩效果
                if (playerController.health > 1)
                {
                    playerController.health -= 1;
                    Debug.Log("熔岩！生命值-1");
                }
                break;

            default:
                // 普通矿石无效果
                Debug.Log("普通矿石被挖掉");
                break;
        }
    }

    void DestroyOre()
    {
        // 通知玩家这个矿石被挖掉了
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnOreDestroyed(this);
            }
        }

        // 销毁矿石
        Destroy(gameObject);
    }
}