using UnityEngine;
using UnityEngine.UI;

public class FMineModeManager : MonoBehaviour
{
    private static FMineModeManager instance;
    public static FMineModeManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance=FindObjectOfType<FMineModeManager>();
                if (instance == null)
                {
                    Debug.Log("No F");
                }
            }

            return instance;
        }
    }

    public bool isFMouseMineActive = false;  // F键模式是否激活
    public bool hasUsedFMouseMine = false;   // 本次是否已使用
    public Image skillimage;
    public Image skilltext;

    void Awake()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFMouseMineMode();
        }

        if (hasUsedFMouseMine || isFMouseMineActive)
        {
            skillimage.color = Color.grey;
            skilltext.color = Color.grey;
        }
        else
        {
            skillimage.color = Color.white;
            skilltext.color = Color.white;
        }
    }

    void ToggleFMouseMineMode()
    {
        isFMouseMineActive = !isFMouseMineActive;
        hasUsedFMouseMine = false; // 重置使用状态

        if (isFMouseMineActive)
        {
            Debug.Log("F键挖矿模式激活！可以鼠标点击挖掉一块矿石");
            MouseCursorChanger.Instance.ChangeCursor(MouseCursorChanger.Instance.cursorTexture, MouseCursorChanger.Instance.hotspot);
        }
        else
        {
            Debug.Log("F键挖矿模式关闭");
            MouseCursorChanger.Instance.RestoreDefault();
        }
    }

    public bool TryUseFMouseMine()
    {
        if (isFMouseMineActive && !hasUsedFMouseMine)
        {
            hasUsedFMouseMine = true;
            isFMouseMineActive = false; // 使用后自动关闭
            MouseCursorChanger.Instance.RestoreDefault();
            enabled = false;
            return true;
        }
        return false;
    }

    public bool CanUseFMouseMine()
    {
        return isFMouseMineActive && !hasUsedFMouseMine;
    }
}