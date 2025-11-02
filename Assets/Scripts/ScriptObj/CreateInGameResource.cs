using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InGameResource", menuName = "CreateDate/InGameResource", order = 0)]
public class CreateInGameResource : ScriptableObject
{
    [Header("ID")]
    public int id;
    [Header("资源名称")]
    public string resourceName;
    [Header("资源图标")]
    public Sprite resourceIcon;
    [Header("资源描述")]
    [TextArea]
    public string resourceDescript;

}
