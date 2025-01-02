using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public kitchenObjectSO input;

    public kitchenObjectSO output;

    public float fryingTimerMax;
}
