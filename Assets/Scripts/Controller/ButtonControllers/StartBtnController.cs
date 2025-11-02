using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtnController : MonoBehaviour
{
    [Header("开始游戏按钮")]
    public Button startBtn;
    [Header("游戏说明按钮")]
    public Button illustratedBtn;
    [Header("退出游戏按钮")]
    public Button exitgameBtn;
    [Header("图鉴按钮")]
    public Button atlasBtn;
    //[Header("设置按钮")]
    //public Button setBtn;

    private static StartBtnController instance;
    public static StartBtnController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StartBtnController>();
                if (instance == null)
                {
                    Debug.Log("No StartBtnController");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        illustratedBtn.onClick.AddListener(OnIllustratedBtnClick);
        exitgameBtn.onClick.AddListener(OnExitGameBtnClick);
        atlasBtn.onClick.AddListener(OnAtlasBtnClick);
        //setBtn.onClick.AddListener(OnSetBtnClick);
    }

    public void OnStartBtnClick()
    {
        //StartCoroutine(SceneController.Instance.LoadInGameScene());
    }

    public void OnIllustratedBtnClick()
    {
        //UIManager.Instance.ShowIllustratedUI();
    }

    public void OnExitGameBtnClick()
    {
        Game.ExitGame();
    }

    public void OnAtlasBtnClick()
    {
        //UIManager.Instance.ShowAtlasUI();
    }
}
