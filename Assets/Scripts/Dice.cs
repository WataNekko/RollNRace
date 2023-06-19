using System;
using System.Collections;
using UnityEngine;

public class Dice : Singleton<Dice>
{
    private Coroutine rollCoroutine;

    public IEnumerator Roll(Action<int> callback = null)
    {
        yield return new WaitForSeconds(1f);
        var n = UnityEngine.Random.Range(1, 7);
        callback?.Invoke(n);
    }


}
