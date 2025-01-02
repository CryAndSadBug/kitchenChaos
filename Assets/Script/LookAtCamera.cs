using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamara : MonoBehaviour
{

    // ö��
    private enum Mode
    {
        LookAt,
        // ��תЧ��
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
                // ��������
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
