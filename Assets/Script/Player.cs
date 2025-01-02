using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    #region ����
    // ����playerΪ����
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

    #region ����һ�����ڸ��� ��ѡ�й��� ���¼�
    // �����¼�
    public event EventHandler<OnSelectedCounterChangedEvenrArgs> OnSelectedCounterChanged;

    // ����һ���̳� EventArgs ���� 
    public class OnSelectedCounterChangedEvenrArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    #endregion


    [SerializeField] private float playerSpeed = 10f;
    [SerializeField] private float turnSpeed = 10f;
    // �Ƿ�������״̬
    private bool isWalking;

    // ��¼���һ���ƶ��ķ���
    private Vector3 lastMoveDir;


    // ����һ�� clearCounter ��ͼ���ɰ�  �ü������� ֻ������ͼ�������
    [SerializeField] private LayerMask ClearCounterLayerMask;

    #region Compoment
    // ��ȡ����Ϸ��������
    [SerializeField] private GameInput gameInput;
    #endregion

    #region GameObject
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    #endregion


    private void Start()
    {
        // �� OnInteractAcion ִ��ʱ Ҳ���� GameInput_OnInteractAcion
        gameInput.OnInteractAcion += GameInput_OnInteractAcion;

        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void Update() 
    {
        HandleInteractions();
        HandleMovement();
    }

    #region �����¼�
    private void GameInput_OnInteractAcion(object sender, System.EventArgs e)
    {

        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        // ����
        // ��� selectCounter ��Ϊ�� �����selectedCounter(��������ΪClearCounter) �� Interact() ����
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

    // �����Ƿ�������״̬
    public bool PlayerIsWalking()
    {
        return isWalking;
    }

    // �������ĺ���
    private void HandleInteractions()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();

        // һ��������˶��ٶȳ��� Time.deltaTime Ŀ������������˶��ٶ� �����ܵ�֡����Ӱ�� 
        Vector3 MoveDir = new Vector3(InputVector.x, 0f, InputVector.y);

        // ��ֹͣ�ƶ�ʱ Ҳ������һ���ƶ������ϵ����巢������
        if (MoveDir != Vector3.zero)
        {
            lastMoveDir = MoveDir;
        }

        // ��������
        float InteractDistance = 2f;

        // ������
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, InteractDistance, ClearCounterLayerMask))
        {
            // ����ȥ��ȡclearCounter ������ �������true���ǻ�ȡ����  ���� out ���᷵�ػ�ȡ��������
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

    // �����ƶ��ĺ���
    private void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();

        // һ��������˶��ٶȳ��� Time.deltaTime Ŀ������������˶��ٶ� �����ܵ�֡����Ӱ�� 
        Vector3 MoveDir = new Vector3(InputVector.x, 0, InputVector.y);

        #region Coliider ��������������
        // �ô���ʵ�ֵ� box collider(��ײ��)
        float playerRadius = .7f;
        float playerHight = 2f;
        float moveDistance = playerSpeed * Time.deltaTime;

        // ������������  Physics.CapsuleCast(��ʼλ��, �յ�λ��, ������, ��ⷽ��, �ƶ�����);
        bool isWall = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, MoveDir, moveDistance);
        #endregion

        #region ���泯������ǽ��ʱ б�����ƶ����ж�
        if (isWall)
        {
            // Cannot move towards(����) moveDir

            // Attemp only X movement
            Vector3 moveDirX = new Vector3(MoveDir.x, 0, 0).normalized;  // ����ǽ����W A/D ������ʱ���ٶ� ���� ������������ߵ��ٶ�
            isWall = (MoveDir.x < -.5f || MoveDir.x > +.5f) && Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, playerRadius, moveDirX, moveDistance);
            
            if (!isWall)
            {
                // Can move only X
                MoveDir = moveDirX;
            } else
            {
                // Attempt(����) only Z movement
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

        // �泯��������ƶ�����
        // (��)ת���ƽ������ ----- ��ֵ Lerp(������λ��) �� Slerp(������ת��)
        transform.forward = Vector3.Slerp(transform.forward, MoveDir, turnSpeed * Time.deltaTime);

        // ������ҵ��ƶ��ٶ��Ƿ�Ϊ0�� isWalking �����ж� true of false
        isWalking = MoveDir != Vector3.zero;
    }

    // ���� ��ѡ�еĹ���
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEvenrArgs()
        {
            // ( OnSelectedCounterChangedEvenrArgs ��� selectedCounter ) = ( ClearCounter Ϊ���͵� ����selectedCounter )
            selectedCounter = selectedCounter
        });
    }

    // ���� ��������Ϸ�λ�� 
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    #region ͨ���ӿ�ʵ�ֵ� ���ã���ȡ����գ��жϴ��� (kitchenObject)
    // ���� kitchenObject
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        // �����������һ���������� �൱����Ҽ�����һ����Ʒ
        this.kitchenObject = kitchenObject;

        OnPickedSomething?.Invoke(this, EventArgs.Empty);
    }

    // ��ȡ kitchenObject
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    // ��� KitchenObject
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    // ����һ������ֵ �����ж� kitchenObject �Ƿ����
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    #endregion 
}
