using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private string PLAYER_PREFS_SOUND_VOLUME = "soundVolum";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;

        // 设置默认值
        PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, 1f);
    }

    private void Start()
    {

        // 成功事件的监听
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;

        // 失败事件的监听
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        // 所有切菜橱柜的切菜音效
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        // 玩家捡起物品时的音效
        Player.Instance.OnPickedSomething += Instance_OnPickedSomething;

        // 把东西放置在任何物体上的音效
        BaseCounter.OnAnyObjectPlaceHere += BaseCounter_OnAnyObjectPlaceHere;

        // 垃圾桶销毁物品时的音效
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaceHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Instance_OnPickedSomething(object sender, System.EventArgs e)
    {

        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        // 把 sender 作为 CuttingCounter 类型赋值给 cuttingCounter
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        // 让音效从送货的柜台发出
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFaild, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        // 让音效从送货的柜台发出
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    } 

    private void PlaySound(AudioClip[] audioClipArrray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArrray[Random.Range(0, audioClipArrray.Length)], position, volume);
    }

    public void PlayFootStepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefsSO.warring, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefsSO.warring, position);
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        // 保存数据
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
