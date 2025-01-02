using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    #region 单例
    // 设置player为单例
    public static Player Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There is more than one player instance!");
        }
        Instance = this;
    }
    #endregion

    public event EventHandler OnPickedSomething;

    #region 定义一个用于更改 被选中柜子 的事件
    // 定义事件
    public event EventHandler<OnSelectedCounterChangedEvenrArgs> OnSelectedCounterChanged;

    // 定义一个继承 EventArgs 的类 
    public class OnSelectedCounterChangedEvenrArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    #endregion


    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float turnSpeed = 10f;
    // 是否处于行走状态
    private bool isWalking;

    // 记录最后一次移动的方向
    private Vector3 lastMoveDir;


    // 定义一个 clearCounter 的图层蒙版  让激光射线 只检测这个图层的物体
    [SerializeField] private LayerMask ClearCounterLayerMask;

    #region Compoment
    // 获取到游戏按键输入
    [SerializeField] private GameInput gameInput;
    #endregion

    #region GameObject
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    #endregion


    private void Start()
    {
        // 当 OnInteractAcion 执行时 也调用 GameInput_OnInteractAcion
        gameInput.OnInteractAcion += GameInput_OnInteractAcion;

        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update() 
    {
        HandleInteractions();
        HandleMovement();
    }

    #region 监听事件
    private void GameInput_OnInteractAcion(object sender, System.EventArgs e)
    {

        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // 交互
        // 如果 selectCounter 不为空 则调用selectedCounter(本质类型为ClearCounter) 的 Interact() 方法
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {

        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    #endregion

    // 返回是否处于行走状态
    public bool PlayerIsWalking()
    {
        return isWalking;
    }

    // 处理交互的函数
    private void HandleInteractions()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();

        // 一个物体的运动速度乘上 Time.deltaTime 目的是让物体的运动速度 不再受到帧数的影响 
        Vector3 MoveDir = new Vector3(InputVector.x, 0f, InputVector.y);

        // 当停止移动时 也会和最后一次移动方向上的物体发生交互
        if (MoveDir != Vector3.zero)
        {
            lastMoveDir = MoveDir;
        }

        // 交互距离
        float InteractDistance = 2f;

        // 物体检测
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, InteractDistance, ClearCounterLayerMask))
        {
            // 尝试去获取clearCounter 这个组件 如果返回true就是获取到了  参数 out 将会返回获取到的数据
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has ClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }    
            } else
            {
                SetSelectedCounter(null);
            }
        } else
        {
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);
    }

    // 处理移动的函数
    private void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();

        // 一个物体的运动速度乘上 Time.deltaTime 目的是让物体的运动速度 不再受到帧数的影响 
        Vector3 MoveDir = new Vector3(InputVector.x, 0, InputVector.y);

        #region Coliider 胶囊碰器的制作
        // 用代码实现的 box collider(碰撞器)
        float playerRadius = .7f;
        float playerHight = 2f;
        float moveDistance = playerSpeed * Time.deltaTime;

        // 胶囊体物理检测  Physics.CapsuleCast(起始位置, 终点位置, 检测距离, 检测方向, 移动距离);
        bool isWall = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, MoveDir, moveDistance);
        #endregion

        #region 当面朝方向有墙体时 斜方向移动的判断
        if (isWall)
        {
            // Cannot move towards(朝向) moveDir

            // Attemp only X movement
            Vector3 moveDirX = new Vector3(MoveDir.x, 0, 0).normalized;  // 让贴墙按下W A/D 键行走时的速度 等于 正常情况下行走的速度
            isWall = (MoveDir.x < -.5f || MoveDir.x > +.5f) && Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirX, moveDistance);
            
            if (!isWall)
            {
                // Can move only X
                MoveDir = moveDirX;
            } else
            {
                // Attempt(尝试) only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, MoveDir.z).normalized;
                isWall = (MoveDir.z < -.5f || MoveDir.z > +.5f) && Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirZ, moveDistance);
                
                if (!isWall)
                {
                    // Can move only Z
                    MoveDir = moveDirZ;
                } else
                {
                    // cannot move in any direction
                }
            }
        }
        # endregion

        if (!isWall)
        {
            transform.position += MoveDir * moveDistance;
        }

        // 面朝方向等于移动方向
        // (简单)转向的平滑处理 ----- 插值 Lerp(仅处理位置) 和 Slerp(仅处理转向)
        transform.forward = Vector3.Slerp(transform.forward, MoveDir, turnSpeed * Time.deltaTime);

        // 根据玩家的移动速度是否为0对 isWalking 进行判断 true of false
        isWalking = MoveDir != Vector3.zero;
    }

    // 设置 被选中的柜子
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEvenrArgs()
        {
            // ( OnSelectedCounterChangedEvenrArgs 里的 selectedCounter ) = ( ClearCounter 为类型的 变量selectedCounter )
            selectedCounter = selectedCounter
        });
    }

    // 返回 橱柜的最上方位置 
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    #region 通过接口实现的 设置，获取，清空，判断存在 (kitchenObject)
    // 设置 kitchenObject
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        // 对玩家设置了一个厨房材料 相当于玩家捡起了一个物品
        this.kitchenObject = kitchenObject;

        OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    // 获取 kitchenObject
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // 清空 KitchenObject
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // 返回一个布尔值 用于判断 kitchenObject 是否存在
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    #endregion 
}
