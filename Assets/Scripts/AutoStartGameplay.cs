using UnityEngine;

/// <summary>
/// Script to auto run the game on start.
/// </summary>
public class AutoStartGameplay : MonoBehaviour
{
    private void Start() => GameManager.Instance.StartGame();
}
