using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : Singleton<GameplayUI>
{
    [field: SerializeField]
    public HUD HUD { get; private set; }

    [field: SerializeField]
    public StatWindow StatWindow { get; private set; }

    /// <summary>
    /// Quits the game from the gameplay. This is because the reference to the
    /// GameManager wouldn't work in the Unity Editor due to it being a
    /// persistent Singleton.
    /// </summary>
    public void Quit() => GameManager.Instance.Quit();

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void BackToMainMenu() => SceneManager.LoadScene("Main_Menu");
}
