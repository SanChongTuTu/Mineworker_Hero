using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ResultPanelUI : MonoBehaviour
{
    public TextMeshProUGUI numberText;  // 显示数字的Text组件
    public Button returnButton;  // 返回按钮（始终显示）
    public Button nextButton;    // 下一关按钮（胜利时显示）
    public int number;
    private Coroutine countingCoroutine;

    void Start()
    {
        // 初始隐藏
        //gameObject.SetActive(false);

        // 设置按钮监听
        if (returnButton != null)
            returnButton.onClick.AddListener(() => Debug.Log("返回主页"));

        if (nextButton != null)
            nextButton.onClick.AddListener(() => Debug.Log("下一关"));

        // 初始隐藏下一关按钮
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }

    // 显示胜利界面（两个按钮）
    public void ShowVictory()
    {
        gameObject.SetActive(true);

        if (nextButton != null)
            nextButton.gameObject.SetActive(true);

        StartCounting();
    }

    // 显示失败界面（一个按钮）
    public void ShowDefeat()
    {
        gameObject.SetActive(true);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        StartCounting();
    }

    // 隐藏界面
    public void Hide()
    {
        gameObject.SetActive(false);

        // 停止计数
        if (countingCoroutine != null)
        {
            StopCoroutine(countingCoroutine);
            countingCoroutine = null;
        }
    }

    // 开始计数动画：1秒内从0数到10
    void StartCounting()
    {
        // 停止之前的计数
        if (countingCoroutine != null)
            StopCoroutine(countingCoroutine);

        // 重置为0
        if (numberText != null)
            numberText.text = "0";

        // 开始新的计数
        countingCoroutine = StartCoroutine(CountToTen());
    }


    
    IEnumerator CountToTen()
    {
        if (numberText == null || number <= 0) yield break;

        numberText.text = "0";

        
        float nowtime = 0;
        while (nowtime < 1.2f)
        {
            nowtime += Time.deltaTime;
            numberText.text = ((int)Mathf.Lerp(0, (int)(0.8f * number), nowtime / 1.2f)).ToString();
            yield return null;
        }
        nowtime = 0;
        while (nowtime < 0.8f)
        {
            nowtime += Time.deltaTime;
            numberText.text = ((int)Mathf.Lerp((int)(0.8f * number), (int)( number), nowtime / 0.8f)).ToString();
            yield return null;
        }
        

    }


}