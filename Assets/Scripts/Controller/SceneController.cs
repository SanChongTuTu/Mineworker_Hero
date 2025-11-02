using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{

    private static SceneController instance;
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneController>();
                if (instance == null)
                {
                    Debug.Log("No SceneController");
                }
            }
            return instance;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
