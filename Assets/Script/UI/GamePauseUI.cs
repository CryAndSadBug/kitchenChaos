using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{

    [SerializeField] private Button resumeButton;

    [SerializeField] private Button mainMenuButton;

    [SerializeField] private Button optionsButton;

    private void Awake()
    {

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainmenuScene);
        });

        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.ToggPauseGame();
        });

        optionsButton.onClick.AddListener(() =>
        {
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnGamePused += KitchenGameManager_OnGamePused;

        KitchenGameManager.Instance.OnGameUnpused += KitchenGameManager_OnGameUnpused;

        Hide();
    }

    private void KitchenGameManager_OnGameUnpused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePused(object sender, System.EventArgs e)
    {
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
