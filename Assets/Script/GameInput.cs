using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFES_BINDING = "InputBindings";

    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActioins;

    // ���� OnInteractAcion �¼�
    public event EventHandler OnInteractAcion;

    // ���� OnInteractAlternateAction �¼�
    public event EventHandler OnInteractAlternateAction;

    public event EventHandler OnPauseAction;

    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    private void Awake()
    {
        Instance = this;

        // �ж��û��Ƿ����Զ����λ   �оͼ��� 
        if (PlayerPrefs.HasKey(PLAYER_PREFES_BINDING))
        {
            playerInputActioins.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFES_BINDING));
        }

        // �����½�����
        playerInputActioins = new PlayerInputActions();

        // Ϊplayer�����½�������
        playerInputActioins.Player.Enable();

        // �� Interact��Ϊ ����һ���¼�  
        playerInputActioins.Player.Interact.performed += Interact_performed;

        // ��InteractAlternate �����¼�
        playerInputActioins.Player.InteractAlternate.performed += InteractAlternate_performed;

        playerInputActioins.Player.Pause.performed += Pause_performed;

    }

    // ȡ������
    // OnDestroy() ����ʱ�����������
    private void OnDestroy()
    {
        playerInputActioins.Player.Interact.performed -= Interact_performed;

        playerInputActioins.Player.InteractAlternate.performed -= InteractAlternate_performed;

        playerInputActioins.Player.Pause.performed -= Pause_performed;

        // �������
        playerInputActioins.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // �¼�����
    // �����¼��Ķ��� UnityEngine.InputSystem.InputAction.CallbackContext obj
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
            //OnInteractAcion(������, �¼�����);
            // EventArgs.Empty  �¼�����.��
            OnInteractAcion?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        // ��ȡ������������� Move ���󶨵� WSAD ��ֵ (�൱���Զ����˰���)
        Vector2 InputVector = playerInputActioins.Player.Move.ReadValue<Vector2>();

        // �����б���˶�ʱ �ٶȵ���ֱ���˶�
        InputVector = InputVector.normalized;

        return InputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            // ��� binding��Ϊ Binding.Interact ��ִ�� default ������ ����δ��벻���Ƿ�ΪBinding.Interact ��������� Binding.Interact���󶨵İ����� 
            default:
            case Binding.Move_Up:
                return playerInputActioins.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActioins.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActioins.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActioins.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActioins.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputActioins.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActioins.Player.Pause.bindings[0].ToDisplayString();
            case Binding.Gamepad_Interact:
                return playerInputActioins.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputActioins.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputActioins.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        // �Ƚ��� playerInputActioins
        playerInputActioins.Player.Disable();

        InputAction inputAction;

        int bindingIndex;

        switch (binding)
        {
            default :
            case Binding.Move_Up:
                inputAction = playerInputActioins.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerInputActioins.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerInputActioins.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerInputActioins.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActioins.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputAction = playerInputActioins.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActioins.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.Gamepad_Interact:
                inputAction = playerInputActioins.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputActioins.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.Gamepad_Pause:
                inputAction = playerInputActioins.Player.Pause;
                bindingIndex = 1;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)

        .OnComplete((callback) =>
        {
            callback.Dispose();
            playerInputActioins.Enable();
            onActionRebound();

            // �洢�û����ĺ�ļ�λ����
            PlayerPrefs.SetString(PLAYER_PREFES_BINDING, playerInputActioins.SaveBindingOverridesAsJson());

            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        })
            .Start();
    }   
}
