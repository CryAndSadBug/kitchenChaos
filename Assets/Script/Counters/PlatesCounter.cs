using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatesCounter : BaseCounter
{
    // 定义一个事件来完成 盘子的生成
    public event EventHandler OnPlatesSpawned;
    // 定义一个事件来完成 玩家拿走后盘子的移除
    public event EventHandler OnPlatesMove;

    [SerializeField] private kitchenObjectSO platesKitchenObjectSO;

    // 记录生成时间
    private float spawPlatesTimer;
    // 多少秒生成
    private float spawPlatesTimerMax = 4f;

    // 因为每个橱柜上的父物体只能存在一个子物体
    // 所以我们生成 KitchenObjectSO 然后用 platesSpawnedAmount 记录生成个数 platesSpawnedAmountMax记录最多生成多少个
    // 玩家拿取时再生成对应的对象
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
    