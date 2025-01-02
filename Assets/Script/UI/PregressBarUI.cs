using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PregressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    private void Start()    
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();

        if (hasProgress == null)
        {
            Debug.LogError("Game Object" + hasProgressGameObject + "does not have a component that implements IHasProgress!");
        }

        hasProgress.OnPregressChanged += HasProgress_OnPregressChanged;

        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnPregressChanged(object sender, IHasProgress.OnPregressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
