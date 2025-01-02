using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{

    // 切菜声
    public AudioClip[] chop;

    // 送货柜台的成功音效
    public AudioClip[] deliverySuccess;

    // 送货柜台的失败音效
    public AudioClip[] deliveryFaild;

    // 脚步声
    public AudioClip[] footstep;

    // 对象删除
    public AudioClip[] objectDrop;

    // 物体拾取
    public AudioClip[] objectPickup;

    // 炉子滋滋声
    public AudioClip stoveSizzle;

    public AudioClip[] trash;


    public AudioClip[] warring;

}
