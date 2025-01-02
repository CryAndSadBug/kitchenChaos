using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashBarUI : MonoBehaviour
{

    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnPregressChanged += StoveCounter_OnPregressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnPregressChanged(object sender, IHasProgress.OnPregressChangedEventArgs e)
    {
        float burnShowPregressAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowPregressAmount;

        animator.SetBool(IS_FLASHING, show);
    }
}
