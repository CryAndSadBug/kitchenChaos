using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public kitchenObjectSO input;

    public kitchenObjectSO output;

    public float burningTimerMax;
}
