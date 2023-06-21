using UnityEngine;

public class AutoStartGameplay : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.State = GameState.Running;
    }
}
