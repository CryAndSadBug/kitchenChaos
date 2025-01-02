using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI KeyMoveUpText;
    [SerializeField] private TextMeshProUGUI KeyMoveDownText;
    [SerializeField] private TextMeshProUGUI KeyMoveLeftText;
    [SerializeField] private TextMeshProUGUI KeyMoveRightText;
    [SerializeField] private TextMeshProUGUI KeyInteractText;
    [SerializeField] private TextMeshProUGUI KeyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI KeyPauseText;
    [SerializeField] private TextMeshProUGUI KeyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI KeyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI KeyGamepadPauseText;

    private void Start()
    {

        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;

        KitchenGameManager.Instance.OnStateChange += KitchenGameManager_OnStateChange;

        UpdateVisual();

        Show();
    }

    private void KitchenGameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        KeyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        KeyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        KeyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        KeyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        KeyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        KeyInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        KeyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        KeyGamepadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);
        KeyGamepadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlternate);
        KeyGamepadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
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
