using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ڴ���һ���˵� ������ unity ����Ӷ���
[CreateAssetMenu()]

// �̳� ScriptableObject���ű�����
public class kitchenObjectSO : ScriptableObject
{

    public Transform prefab;
    public Sprite sprite;
    public string objectName;

}
