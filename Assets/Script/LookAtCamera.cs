using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamara : MonoBehaviour
{

    // 枚举
    private enum Mode
    {
        LookAt,
        // 反转效果
        LookAtInvered,
        CameraForward,
        CameraForwardInvered,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInvered:
                // 往反方向看
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInvered:
                transform.forward = - Camera.main.transform.forward;
                break;
        }
    }
}
