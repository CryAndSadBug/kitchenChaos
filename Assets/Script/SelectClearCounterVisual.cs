using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClearCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        // += �൱�ڽ���������֮�����ϵ �� OnSelectedCounterChanged ������ʱ Instance_OnSelectedCounterChanged Ҳ�ᱻ����
        // -= �൱��ȡ������֮�����ϵ
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEvenrArgs e)
    {
        // �����ȡ���Ĺ����� clearCounter �Ļ� ��ʾѡ��ͼ��
        if (e.selectedCounter == baseCounter)
        {
            show();
        }else
        {
            hide();
        }
    }

    private void show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }

    private void hide() 
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
