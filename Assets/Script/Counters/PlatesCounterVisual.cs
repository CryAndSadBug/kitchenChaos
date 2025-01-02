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
        // ���ӵ�����
        platesCounter.OnPlatesSpawned += PlatesCounter_OnPlatesSpawned;
        // ������ߺ����ӵ��Ƴ�
        platesCounter.OnPlatesMove += PlatesCounter_OnPlatesMove;
    }

    // ������ߺ����ӵ��Ƴ�
    private void PlatesCounter_OnPlatesMove(object sender, System.EventArgs e)
    {
        GameObject plateGameObjecy = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];

        plateVisualGameObjectList.Remove(plateGameObjecy);

        Destroy(plateGameObjecy);
    }

    // ���ӵ�����
    private void PlatesCounter_OnPlatesSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platesVisualPrefab, counterTopPoint);

        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }
}
