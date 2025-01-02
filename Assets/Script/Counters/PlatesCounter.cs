using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    // ����һ���¼������ ���ӵ�����
    public event EventHandler OnPlatesSpawned;
    // ����һ���¼������ ������ߺ����ӵ��Ƴ�
    public event EventHandler OnPlatesMove;

    [SerializeField] private kitchenObjectSO platesKitchenObjectSO;

    // ��¼����ʱ��
    private float spawPlatesTimer;
    // ����������
    private float spawPlatesTimerMax = 4f;

    // ��Ϊÿ�������ϵĸ�����ֻ�ܴ���һ��������
    // ������������ KitchenObjectSO Ȼ���� platesSpawnedAmount ��¼���ɸ��� platesSpawnedAmountMax��¼������ɶ��ٸ�
    // �����ȡʱ�����ɶ�Ӧ�Ķ���
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;

    private void Update()
    {
        spawPlatesTimer += Time.deltaTime;

        if (spawPlatesTimer > spawPlatesTimerMax)
        {
            spawPlatesTimer = 0;

            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpawnedAmount < platesSpawnedAmountMax)
            {
                platesSpawnedAmount++;

                OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // player is empty hande
            if (platesSpawnedAmount > 0)
            {
                // There is at least on plate here
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(platesKitchenObjectSO, player);

                OnPlatesMove?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
    