using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{

    public static OptionsUI Instance;

    [SerializeField] private Button soundEffectButton;

    [SerializeField] private Button musicButton;

    [SerializeField] private Button CloseButton;

    [SerializeField] private TextMeshProUGUI soundEffectText;
    [SerializeField] private TextMeshProUGUI musicText;

    #region MoveButton And Text
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAlternateButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadInteractAlternateButton;
    [SerializeField] private Button gamePauseButtonButton;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltenateText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;
    #endregion

    [SerializeField] private Transform pressToRebingdKeyTransform;

    private Action onCloseButtonAction;

    private void Awake()
    {

        Instance = this;

        soundEffectButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        CloseButton.onClick.AddListener(() =>
        {
            Hide();
            onCloseButtonAction();
        });

        moveUpButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Move_Up);
        });

        moveDownButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Move_Down);
        });

        moveLeftButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Move_Left);
        });

        moveRightButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Move_Right);
        });

        interactButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Interact);
        });

        interactAlternateButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.InteractAlternate);
        });

        pauseButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Pause);
        });
        gamepadInteractButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Gamepad_Interact);
        });

        gamepadInteractAlternateButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Gamepad_InteractAlternate);
        });

        gamePauseButtonButton.onClick.AddListener(() =>
        {
            ReBindBinding(GameInput.Binding.Gamepad_Pause);
        });
    }

    private void Start()
    {

        KitchenGameManager.Instance.OnGameUnpused += KitchenGameManager_OnGameUnpused;

        UpdateVisual();
        HidPressToRebindKey();
        Hide();
    }

    private void KitchenGameManager_OnGameUnpused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectText.text = "Sound Effect: " + Mathf.Ceil(SoundManager.Instance.GetVolume() * 10f);

        musicText.text = "Music Effect: " + Mathf.Ceil(MusicManager.Instance.GetVolume() * 10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        gamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        gamepadInteractAltenateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        gamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }

    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        soundEffectButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebingdKeyTransform.gameObject.SetActive(true);
    }

    private void HidPressToRebindKey()
    {
        pressToRebingdKeyTransform.gameObject.SetActive(false);
    }

    private void ReBindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();

        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidPressToRebindKey();
            UpdateVisual();
        });
    }
}
