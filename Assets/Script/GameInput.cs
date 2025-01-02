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

    // 定义 OnInteractAcion 事件
    public event EventHandler OnInteractAcion;

    // 定义 OnInteractAlternateAction 事件
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

        // 判断用户是否有自定义键位   有就加载 
        if (PlayerPrefs.HasKey(PLAYER_PREFES_BINDING))
        {
            playerInputActioins.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFES_BINDING));
        }

        // 导入新建输入
        playerInputActioins = new PlayerInputActions();

        // 为player启用新建的输入
        playerInputActioins.Player.Enable();

        // 给 Interact行为 增加一个事件  
        playerInputActioins.Player.Interact.performed += Interact_performed;

        // 给InteractAlternate 增加事件
        playerInputActioins.Player.InteractAlternate.performed += InteractAlternate_performed;

        playerInputActioins.Player.Pause.performed += Pause_performed;

    }

    // 取消监听
    // OnDestroy() 销毁时调用这个函数
    private void OnDestroy()
    {
        playerInputActioins.Player.Interact.performed -= Interact_performed;

        playerInputActioins.Player.InteractAlternate.performed -= InteractAlternate_performed;

        playerInputActioins.Player.Pause.performed -= Pause_performed;

        // 清除对象
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

    // 事件函数
    // 触发事件的对象 UnityEngine.InputSystem.InputAction.CallbackContext obj
    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
            //OnInteractAcion(发送人, 事件参数);
            // EventArgs.Empty  事件参数.空
            OnInteractAcion?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        // 读取在输入设置里的 Move 所绑定的 WSAD 的值 (相当于自定义了按键)
        Vector2 InputVector = playerInputActioins.Player.Move.ReadValue<Vector2>();

        // 让玩家斜着运动时 速度等于直线运动
        InputVector = InputVector.normalized;

        return InputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            // 如果 binding不为 Binding.Interact 则执行 default 里的语句 （这段代码不管是否为Binding.Interact 都可以输出 Binding.Interact所绑定的按键） 
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
        // 先禁用 playerInputActioins
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

            // 存储用户更改后的键位数据
            PlayerPrefs.SetString(PLAYER_PREFES_BINDING, playerInputActioins.SaveBindingOverridesAsJson());

            PlayerPrefs.Save();

            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        })
            .Start();
    }   
}
