using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    // 更改进度条UI事件
    public event EventHandler<OnPregressChangedEventArgs> OnPregressChanged;

    public class OnPregressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
