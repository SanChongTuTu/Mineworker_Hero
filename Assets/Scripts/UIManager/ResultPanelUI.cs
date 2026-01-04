using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System.ComponentModel;

public class ResultPanelUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI numberText;  // 显示数字的Text组件
    public TextMeshProUGUI resultText;
    public Button returnButton;  // 返回按钮（始终显示）
    public Button nextButton;    // 下一关按钮（胜利时显示）
    public int number;
    private Coroutine countingCoroutine;

    void Start()
    {
        
        panel.SetActive(false);

        // 设置按钮监听
        if (returnButton != null)
            returnButton.onClick.AddListener(() => {
                Time.timeScale = 1;
                SceneController.Instance.ToScene(0);
            });

        if (nextButton != null)
            nextButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneController.Instance.ToScene((int)SceneController.Instance.nowscene+1);
            });

        // 初始隐藏下一关按钮
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }

    // 显示胜利界面（两个按钮）
    public void ShowVictory(int crystalCount)
    {
        StartCoroutine(Showvictory(crystalCount));
    }

    IEnumerator Showvictory(int crystalCount)
    {
        yield return new WaitForSeconds(2);
        SimplePlayer.Instance.resultPanel.panel.SetActive(true);
        resultText.text = "战斗胜利！";
        resultText.color = Color.green;
        number = crystalCount;
        PowerCrystalManager.AddCrystals("powerCrystalStats.json", number);
        gameObject.SetActive(true);

        if (nextButton != null && SceneController.Instance.nowscene != SceneController.NowScene.Game3)
            nextButton.gameObject.SetActive(true);

        StartCounting();

        yield break;
    }

    // 显示失败界面（一个按钮）
    public void ShowDefeat(int crystalCount)
    {
        StartCoroutine(Showdefeat(crystalCount));
    }

    IEnumerator Showdefeat(int crystalCount)
    {
        yield return new WaitForSeconds(2);
        SimplePlayer.Instance.resultPanel.panel.SetActive(true);
        resultText.text = "战斗失败！";
        resultText.color = Color.red;
        number = crystalCount;
        PowerCrystalManager.AddCrystals("powerCrystalStats.json", number);
        gameObject.SetActive(true);

        if (nextButton != null)
            nextButton.gameObject.SetActive(false);

        StartCounting();

        yield break;
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