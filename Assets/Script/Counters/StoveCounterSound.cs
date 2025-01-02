using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{

    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;

    private float warningSoundTiemr;

    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnPregressChanged += StoveCounter_OnPregressChanged;
    }

    private void StoveCounter_OnPregressChanged(object sender, IHasProgress.OnPregressChangedEventArgs e)
    {
        float burnShowPropressAmount = .5f;
        playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowPropressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {

        bool playSound = e.state == StoveCounter.State.frying || e.state == StoveCounter.State.fried;

        if (playSound)
        {
            audioSource.Play();
        }else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playWarningSound) 
        {
            warningSoundTiemr -= Time.deltaTime;
            if (warningSoundTiemr <= 0f)
            {
                float warningSoundTiemrMax = .2f;
                warningSoundTiemr = warningSoundTiemrMax;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
        
    }
}
