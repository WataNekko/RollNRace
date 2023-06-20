using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Represents a player GameObject in the game.
/// </summary>
public class Player : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// The player's current position on the rock path (game board). From 0 to 36, 0 is the starting point before the 1st rock, 36 is the finishing point after the last rock.
    /// </summary>
    [field: Header("Properties")]
    [field: SerializeField]
    public int CurrentPosition { get; private set; } = 0;
    /// <summary>
    /// Whether the player has finished the game
    /// </summary>
    public bool IsFinished => CurrentPosition > 35;
    /// <summary>
    /// The number of turns the player has played
    /// </summary>
    [field: SerializeField]
    public int TurnCount { get; set; } = 0;
    /// <summary>
    /// The number of bonus sectors the player has entered
    /// </summary>
    [field: SerializeField]
    public int BonusSectorCount { get; set; } = 0;
    /// <summary>
    /// The number of fail sectors the player has entered
    /// </summary>
    [field: SerializeField]
    public int FailSectorCount { get; set; } = 0;
    #endregion



    [Header("Movement")]
    [SerializeField]
    [Tooltip("The maximum height of the hop effect during movement.")]
    private float hopHeight = 0.5f;



    #region Methods

    /// <summary>
    /// A coroutine that moves the GameObject to the specified position over a given duration, with a little hopping animation.
    /// </summary>
    /// <param name="targetPosition">The target position to move towards.</param>
    /// <param name="duration">The duration of the movement in seconds.</param>
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

    public IEnumerator MoveSteps(int steps)
    {

        var from = CurrentPosition;
        CurrentPosition = Mathf.Clamp(CurrentPosition + steps, 0, RockPath.Instance.Length - 1);

        steps = CurrentPosition - from; // clamped step count
        var range = steps >= 0 ?
            Enumerable.Range(from + 1, steps) :
            Enumerable.Range(from + steps, -steps).Reverse();
        foreach (var i in range)
        {
            var pos = RockPath.Instance.GetRock(i).position;
            pos.y += 0.00225f; // offset to stand on top of the rock
            yield return MoveTo(pos, facingBackward: steps < 0);
        }
    }

    #endregion
}

