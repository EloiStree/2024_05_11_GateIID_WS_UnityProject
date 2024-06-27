using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnFocusAppTickMono : MonoBehaviour
{

    public UnityEvent<bool> onFocus;
    public UnityEvent m_onFocusTrue;
    public UnityEvent m_onFocusFalse;
    public void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            onFocus.Invoke(true);
            m_onFocusTrue.Invoke();
        }
        else
        {
            onFocus.Invoke(false);
            m_onFocusFalse.Invoke();
        }
    }
}
