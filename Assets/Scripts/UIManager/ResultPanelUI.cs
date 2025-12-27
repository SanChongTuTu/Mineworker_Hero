using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelUI : MonoBehaviour
{
    [Header("UI组件")]
    public TextMeshPro resultText;          // 结果文本（胜利/失败）
    public TextMeshPro oreCounterText;      // 超能矿石计数器
    public Button nextLevelButton;   // 下一关按钮
    public Button returnButton;      // 返回主页面按钮

    private int oreCount = 0;
    private float counterSpeed = 10f; // 计数器变化速度

    void Start()
    {
        // 初始化按钮监听
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(() => Debug.Log("下一关"));
        }

        if (returnButton != null)
        {
            returnButton.onClick.AddListener(() => Debug.Log("返回主页面"));
        }

        // 启动计数器动画
        StartCoroutine(AnimateOreCounter());
    }

    // 设置结果文本
    public void SetResultText(string result)
    {
        if (resultText != null)
        {
            resultText.text = result;
        }
    }

    // 模拟计数器动画（从0快速增加到随机数）
    System.Collections.IEnumerator AnimateOreCounter()
    {
        int targetOre = Random.Range(5, 30); // 随机生成一个矿石数量
        int currentOre = 0;

        while (currentOre < targetOre)
        {
            currentOre += Mathf.CeilToInt(counterSpeed * Time.deltaTime);
            if (currentOre > targetOre) currentOre = targetOre;

            if (oreCounterText != null)
            {
                oreCounterText.text = currentOre.ToString();
            }

            yield return null;
        }
    }
}