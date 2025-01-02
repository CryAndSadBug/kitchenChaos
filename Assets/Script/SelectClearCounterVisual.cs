using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClearCounterVisual : MonoBehaviour
{

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        // += 相当于建立起他们之间的联系 当 OnSelectedCounterChanged 被触发时 Instance_OnSelectedCounterChanged 也会被调用
        // -= 相当于取消他们之间的联系
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEvenrArgs e)
    {
        // 如果获取到的柜子是 clearCounter 的话 显示选中图层
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
