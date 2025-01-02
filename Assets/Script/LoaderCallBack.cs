using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    private bool IsFirstLoading = true;

    private void Update()
    {
        if (IsFirstLoading)
        {
            IsFirstLoading = false;

            Loader.LoaderCallBack();
        }
    }
}
