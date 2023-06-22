using UnityEngine.SceneManagement;

public class MainMenuUI : Singleton<MainMenuUI>
{
    /// <summary>
    /// Starts the gameplay by loading the "Gameplay" scene.
    /// </summary>
    public void StartGame() => SceneManager.LoadScene("Gameplay");

    /// <summary>
    /// Quits the game from the main menu. This is because the reference to the
    /// GameManager wouldn't work in the Unity Editor due to it being a
    /// persistent Singleton.
    /// </summary>
    public void Quit() => GameManager.Instance.Quit();

    private void Start()
    {
        // Generate a random map as an effect when the main menu loads
        GameManager.Instance.GenerateRandomMap();
    }
}
