using UnityEngine.SceneManagement;

public class MainMenuUI : Singleton<MainMenuUI>
{
    public void StartGame() => SceneManager.LoadScene("Gameplay");

    public void Quit() => GameManager.Instance.Quit();
}
