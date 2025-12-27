using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtnController : MonoBehaviour
{
    [Header("��ʼ��Ϸ��ť")]
    public Button startBtn;
    [Header("��Ϸ˵����ť")]
    public Button illustratedBtn;
    [Header("�˳���Ϸ��ť")]
    public Button exitgameBtn;
    [Header("ͼ����ť")]
    public Button atlasBtn;
    //[Header("���ð�ť")]
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
        
        //Initialize PC
        PowerCrystalManager.InitializeFile("powerCrystalStats.json", 0);
        
        //Cheat PC
        //PowerCrystalManager.AddCrystals("powerCrystalStats.json", 500);
    }

    public void OnStartBtnClick()
    {
        SceneController.Instance.ToScene(1);
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
