using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public Transform GetPlayer(int index) => transform.GetChild(index);
    public Player[] GetPlayers() => GetComponentsInChildren<Player>();
}
