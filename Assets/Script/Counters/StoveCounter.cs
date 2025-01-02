using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IHasProgress;
using System;
using static UnityEngine.CullingGroup;

public class StoveCounter : BaseCounter, IHasProgress
{

    // ���Ľ�����UI�¼�
    public event EventHandler<IHasProgress.OnPregressChangedEventArgs> OnPregressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private State state;

    public enum State
    {
        Idel,
        frying,
        fried,
        Burned
    }

    private void Start()
    {
        state = State.Idel;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            //״̬��
            switch (state)
                {
                   case State.Idel:
                       break;
                   case State.frying:
                        fryingTimer += Time.deltaTime;

                        OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                        });

                        if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                        {
                            // Fried
                            fryingTimer = 0f;
                            
                            // ��������
                            GetKitchenObject().DestroySelf();
                            // ���ɼ����
                            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                            state = State.fried;

                            burningTimer = 0f;

                            burningRecipeSO = GetBorningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state = state
                            });
                        }
                    break;
                   case State.fried:
                       burningTimer += Time.deltaTime;

                       OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                       {
                           progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                       });

                       if (burningTimer >= burningRecipeSO.burningTimerMax)
                       {
                           // ��������
                           GetKitchenObject().DestroySelf();
                           // ���ɼ����
                           KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                           state = State.Burned;

                           OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                           {
                               state = state
                           });

                           OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                           {
                               progressNormalized = 0f
                           });
                       }   
                   break;
                   case State.Burned:
                   break;
                }
        }
    }

    public override void Interact(Player player)
    {
        // �жϳ������Ƿ�����Ʒ
        if (!HasKitchenObject())
        {
            // ������û����Ʒ�����ж���������Ƿ�����Ʒ
            // There is no KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring sthing 
                // �ж�������ϵ�ʳ���Ƿ���Խ��м�ը
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // ���Լ�ը����ڳ�����
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    state = State.frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });
                }
            } else
            {
                // player not carring anything
            }
        } else
        {
            // ����������Ʒ�����ж���������Ƿ�����Ʒ
            // There is a KitchenObject
            if (player.HasKitchenObject())
            {
                // player is carring somthing
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a Plate

                    // �ѳ����ϵ���Ʒ��ӽ� plateKitchenObject
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // �ж��Ƿ���ӳɹ� �����ӳɹ������ٳ����ϵ���Ʒ
                        GetKitchenObject().DestroySelf();
                    }
                }

                state = State.Idel;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            } else
            {
                // player is not carring anything
                GetKitchenObject().SetKitchenObjectParent(player);

                // ������������֮����״̬���������״̬
                state = State.Idel;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs 
                { 
                    state = state
                });

                OnPregressChanged?.Invoke(this, new IHasProgress.OnPregressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }

    // �������ײ˵�FryingRecipeSO �жϷ������ϵ�ʳ�� �Ƿ��ڲ���(input)��  �����򷵻ض�Ӧ�Ĳ���
    private FryingRecipeSO GetFryingRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // ѭ������ FryingRecipeSO �е� ���� ����
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            // ����鵽 �� ����ȥ��Ԥ���� һ������ ������ �ӹ�֮�������
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        // ������������û�з���
        return null;
    }

    // �������ײ˵�BurningRecipeSO �жϷ������ϵ�ʳ�� �Ƿ��ڲ���(input)��  �����򷵻ض�Ӧ�Ĳ���
    private BurningRecipeSO GetBorningRecipeSOWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // ѭ������ FryingRecipeSO �е� ���� ����
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            // ����鵽 �� ����ȥ��Ԥ���� һ������ ������ �ӹ�֮�������
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        // ������������û�з���
        return null;
    }

    // ���ݲ��׷����и���ʳ��
    private kitchenObjectSO GetOutputForInput(kitchenObjectSO inputKitchenObjectSO)
    {
        // ���� FryingRecipeSO ���ͱ��� ������ GetFryingRecipeSOWithInput() ���صĲ���
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

        // ������صĲ��ײ�Ϊ�� �򷵻� ���׵� ���
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        } else
        {
            return null;
        }
    }

    // �������Ƿ�Ϊ�� ��Ϊ��������з��ϵĲ���(true) ��֮û�з��ϵ�(false)
    private bool HasRecipeWithInput(kitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingrecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingrecipeSO != null;
    }

    public bool IsFried()
    {
        return state == State.fried;
    }
}
