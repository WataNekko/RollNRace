using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Dice : Singleton<Dice>
{
    public event Action<int> OnDiceRolled;
    public UnityEvent OnDiceRollingEnabled;
    public UnityEvent OnDiceRollingDisabled;

    private Coroutine rollCoroutine;

    public void Roll()
    {
        OnDiceRollingDisabled.Invoke();

        if (rollCoroutine != null)
        {
            StopCoroutine(rollCoroutine);
        }
        rollCoroutine = StartCoroutine(RollCoroutine());
    }

    private IEnumerator RollCoroutine()
    {
        yield return new WaitForSeconds(1f);
        var n = UnityEngine.Random.Range(1, 7);
        OnDiceRolled?.Invoke(n);
    }
}
