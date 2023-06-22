using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayUI : Singleton<GameplayUI>
{
    [field: SerializeField]
    public HUD HUD { get; private set; }

    [field: SerializeField]
    public StatWindow StatWindow { get; private set; }

    public void Quit() => GameManager.Instance.Quit();

    public void BackToMainMenu() => SceneManager.LoadScene("Main_Menu");
}
