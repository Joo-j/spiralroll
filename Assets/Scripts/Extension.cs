using System.Collections;
using UnityEngine;
using System;

public static class Extension
{
    public static void Invoke(float time, Action callback)
    {
        GameManager.Instance.StartCoroutine(Co_CountTime());
        IEnumerator Co_CountTime()
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}
