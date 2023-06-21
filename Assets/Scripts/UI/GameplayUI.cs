using UnityEngine;

public class GameplayUI : Singleton<GameplayUI>
{
    [field: SerializeField]
    public HUD HUD { get; private set; }

    [field: SerializeField]
    public StatWindow StatWindow { get; private set; }
}
