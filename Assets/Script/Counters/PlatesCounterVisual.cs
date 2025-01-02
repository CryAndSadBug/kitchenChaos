using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platesVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        // 盘子的生成
        platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        // 玩家拿走后盘子的移除
        platesCounter.OnPlatesMove += PlatesCounter_OnPlatesMove;
    }

    // 玩家拿走后盘子的移除
    private void PlatesCounter_OnPlatesMove(object sender, System.EventArgs e)
    {
        GameObject plateGameObjecy = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        plateVisualGameObjectList.Remove(plateGameObjecy);

        Destroy(plateGameObjecy);
    }

    // 盘子的生成
    private void PlatesCounter_OnPlatesSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platesVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
