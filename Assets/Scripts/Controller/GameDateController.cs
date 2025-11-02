using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDateController : MonoBehaviour
{
    [System.Serializable]
    public class ResourceInfo
    {
        private int mask;
        private int check;
        public InGameResource resource;
        public int num;
        public int realnum
        {
            get
            {
                int v=mask^Game.IntMask;
                if (v + 0x47 != check)
                {

                }
                return v;
            }
            set
            {
                mask= value^Game.IntMask;
                check = value + 0x47;
            }
        }

        public void SetAResource(InGameResource res, int amount)
        {
            resource = res;
            mask = 0;
            check = 0;
            realnum = amount;
            num = realnum;
        }

        public void Addnum(int amount)
        {
            realnum += amount;
            num = realnum;
        }
    }

    [Header("ÓÎÏ·×ÊÔ´")]
    public List<ResourceInfo> resources;

    private static GameDateController instance;
    public static GameDateController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameDateController>();
                if (instance == null)
                {
                    Debug.Log("No GameDateController");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null&&instance!= this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        var scriptobj=Resources.FindObjectsOfTypeAll<InGameResource>();
        foreach(var obj in scriptobj)
        {
            if (obj.hideFlags == HideFlags.HideAndDontSave) continue;
            bool have=false;
            foreach(ResourceInfo resource in resources)
            {
                if(resource.resource == obj)
                {
                    have= true;
                    break;
                }
            }
            if (have)
            {
                continue;
            }
            else
            {
                ResourceInfo newresource = new ResourceInfo();
                newresource.SetAResource(obj,PlayerPrefs.GetInt(obj.name,0));
                resources.Add(newresource);
            }
        }
    }

    void Update()
    {
        
    }

}
