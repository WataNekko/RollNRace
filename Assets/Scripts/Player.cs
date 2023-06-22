using System;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a player entity in the game.
/// </summary>
public class Player : MonoBehaviour
{
    #region Properties

    [field: Header("Properties")]
    [SerializeField]
    private MeshFilter meshFilter;
    public Mesh Mesh
    {
        get => meshFilter.sharedMesh;
        set => meshFilter.sharedMesh = value;
    }

    [SerializeField]
    private MeshRenderer meshRenderer;
    public Material Color
    {
        get => meshRenderer.sharedMaterial;
        set => meshRenderer.sharedMaterial = value;
    }

    /// <summary>
    /// The player's current position on the rock path (game board). From 0 to 36, 0 is the starting point before the 1st rock, 36 is the finishing point after the last rock.
    /// </summary>
    [field: SerializeField]
    public int CurrentPosition { get; private set; }
    /// <summary>
    /// Whether the player has finished the game
    /// </summary>
    public bool IsFinished => CurrentPosition > 35;
    /// <summary>
    /// The number of turns the player has played
    /// </summary>
    [field: SerializeField]
    public int TurnCount { get; set; }
    /// <summary>
    /// The number of bonus sectors the player has entered
    /// </summary>
    [field: SerializeField]
    public int BonusSectorCount { get; set; }
    /// <summary>
    /// The number of fail sectors the player has entered
    /// </summary>
    [field: SerializeField]
    public int FailSectorCount { get; set; }
    #endregion


    /// <summary>
    /// The current gain of turn of this player. E.g., CurrentTurnGain == 1 means the player have gained an extra turn, while CurrentTurnGain == -1 means the player have lost their next turn.
    /// </summary>
    public int CurrentTurnGain;

    [Header("Movement")]
    [SerializeField]
    [Tooltip("The maximum height of the hop effect during movement.")]
    private float hopHeight = 0.5f;



    #region Methods

    /// <summary>
    /// Initialize the player's data to initial values.
    /// </summary>
    public void Init()
    {
        CurrentPosition = 0;
        TurnCount = 0;
        BonusSectorCount = 0;
        FailSectorCount = 0;
        CurrentTurnGain = 0;

        transform.position = GetRockPosition(0);
    }

    /// <summary>
    /// A coroutine that moves the GameObject to the specified position over a given duration, with a little hopping animation.
    /// </summary>
    /// <param name="targetPosition">The target position to move towards.</param>
    /// <param name="duration">The duration of the movement in seconds.</param>
    /// <param name="facingBackward">Whether the player should face backward during the movement. This is for the fail sectors that push the player back.</param>
    public IEnumerator MoveTo(Vector3 targetPosition, float duration = 0.5f, bool facingBackward = false)
    {
        var startPosition = transform.position;

        var startRotation = transform.rotation;
        var _lookingDir = targetPosition - transform.position;
        _lookingDir.y = 0f; // ignore y axis
        if (facingBackward) _lookingDir *= -1;
        var targetRotation = Quaternion.LookRotation(_lookingDir);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Lerp factor
            float t = elapsedTime / duration;

            // The new position at the current time frame
            Vector3 pos = Vector3.Lerp(startPosition, targetPosition, t);

            // vertical position offset using a sine wave for a hopping effect
            float yOffset = Mathf.Sin(t * Mathf.PI) * hopHeight;
            pos.y += yOffset;

            transform.position = pos;

            // Rotate towards the target rotation
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final transform is set accurately
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    /// <summary>
    /// Moves the player by a specific number of steps on the rock path.
    /// </summary>
    /// <param name="steps">The number of steps to move.</param>
    public IEnumerator MoveSteps(int steps)
    {
        var from = CurrentPosition;
        CurrentPosition = Mathf.Clamp(CurrentPosition + steps,
                0, RockPath.Instance.Length - 1);

        steps = CurrentPosition - from; // clamped step count
        var range = steps >= 0 ?
            Enumerable.Range(from + 1, steps) :
            Enumerable.Range(from + steps, -steps).Reverse();
        foreach (var i in range)
        {
            var pos = GetRockPosition(i);
            yield return MoveTo(pos, facingBackward: steps < 0);
        }
    }

    /// <summary>
    /// Gets the world position of the rock at the specified index.
    /// </summary>
    /// <param name="index">The index of the rock.</param>
    /// <returns>The world position of the rock.</returns>
    public Vector3 GetRockPosition(int index)
    {
        var pos = RockPath.Instance.GetRock(index).position;
        pos.y += 0.00225f; // offset to stand on top of the rock
        return pos;
    }

    #endregion
}

