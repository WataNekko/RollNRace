using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
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

    [SerializeField]
    private Transform dropPoint;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    [SerializeField]
    private float MaxRandomTorque = 10;
    [SerializeField]
    private float MaxRandomForce = 30;

    private IEnumerator RollCoroutine()
    {
        // reset transform
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = dropPoint.position;

        // randomize the dice throw
        transform.rotation = UnityEngine.Random.rotation;
        rb.AddTorque(MaxRandomTorque * UnityEngine.Random.insideUnitSphere,
                ForceMode.Impulse);
        rb.AddForce(MaxRandomForce * UnityEngine.Random.insideUnitSphere,
                ForceMode.Impulse);

        // wait for the dice to stop moving
        var timeout = Time.time + 10f; // 10-second timeout
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => rb.velocity.magnitude == 0f ||
                Time.time >= timeout);

        var rolledValue = GetUpSide();
        OnDiceRolled?.Invoke(rolledValue);
    }

    private int GetUpSide()
    {
        // check the face up by using cross and dot products
        var crosses = new List<float>
        {
            Vector3.Cross(Vector3.up, transform.right).magnitude,
            Vector3.Cross(Vector3.up, transform.up).magnitude,
            Vector3.Cross(Vector3.up, transform.forward).magnitude
        };
        int minCross = crosses.IndexOf(crosses.Min());

        var side = minCross switch
        {
            0 => Vector3.Dot(Vector3.up, transform.right) > 0
                ? 4
                : 3,
            1 => Vector3.Dot(Vector3.up, transform.up) > 0
                ? 2
                : 5,
            _ => Vector3.Dot(Vector3.up, transform.forward) > 0
                ? 1
                : 6,
        };
        return side;
    }
}
