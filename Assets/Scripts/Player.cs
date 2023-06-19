using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

/// <summary>
/// Represents a player GameObject in the game.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The maximum height of the hop effect during movement.")]
    private float hopHeight = 0.5f;

    private Coroutine moveCoroutine;

    /// <summary>
    /// Moves the GameObject to the specified position over a given duration, with a little hopping animation.
    /// </summary>
    /// <param name="targetPosition">The target position to move towards.</param>
    /// <param name="duration">The duration of the movement in seconds.</param>
    public void MoveTo(Vector3 targetPosition, float duration = 0.5f)
    {
        if (moveCoroutine != null)
        {
            // If a movement coroutine is already running, stop it
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveCoroutine(targetPosition, duration));
    }

    private IEnumerator MoveCoroutine(Vector3 targetPosition, float duration)
    {
        var startPosition = transform.position;

        var startRotation = transform.rotation;
        var _lookingDir = targetPosition - transform.position;
        _lookingDir.y = 0f; // ignore y axis
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

        // Reset the moveCoroutine reference
        moveCoroutine = null;
    }
}

