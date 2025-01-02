using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用于创建一个菜单 可以在 unity 中添加对象
[CreateAssetMenu()]

// 继承 ScriptableObject（脚本对象）
public class kitchenObjectSO : ScriptableObject
{

    public Transform prefab;
    public Sprite sprite;
    public string objectName;

}
