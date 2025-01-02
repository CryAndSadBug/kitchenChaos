using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image playingTimer;

    private void Update()
    {
        playingTimer.fillAmount = KitchenGameManager.Instance.GetPlayingTimerNormalized();
    }
}
