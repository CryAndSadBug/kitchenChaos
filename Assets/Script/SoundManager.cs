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

        // ����Ĭ��ֵ
        PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_VOLUME, 1f);
    }

    private void Start()
    {

        // �ɹ��¼��ļ���
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;

        // ʧ���¼��ļ���
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        // �����в˳�����в���Ч
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;

        // ��Ҽ�����Ʒʱ����Ч
        Player.Instance.OnPickedSomething += Instance_OnPickedSomething;

        // �Ѷ����������κ������ϵ���Ч
        BaseCounter.OnAnyObjectPlaceHere += BaseCounter_OnAnyObjectPlaceHere;

        // ����Ͱ������Ʒʱ����Ч
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
        // �� sender ��Ϊ CuttingCounter ���͸�ֵ�� cuttingCounter
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        // ����Ч���ͻ��Ĺ�̨����
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFaild, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        // ����Ч���ͻ��Ĺ�̨����
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

        // ��������
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }

}
