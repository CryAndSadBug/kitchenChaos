using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{
    // ���Ľ�����UI�¼�
    public event EventHandler<OnPregressChangedEventArgs> OnPregressChanged;

    public class OnPregressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }
}
