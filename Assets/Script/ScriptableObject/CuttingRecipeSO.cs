using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �����˵�
[CreateAssetMenu()]

public class CuttingRecipeSO : ScriptableObject
{

    // ����
    public kitchenObjectSO input;

    // ���
    public kitchenObjectSO output;

    // �и�Ľ���
    public int cuttingProgressMax;
}
