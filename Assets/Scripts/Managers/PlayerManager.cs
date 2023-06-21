using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private Player playerPrefab;

    public Transform GetPlayer(int index) => transform.GetChild(index);

    public Player[] GetPlayers() => GetComponentsInChildren<Player>();

    public Player AddPlayer()
    {
        var newPlayer = Instantiate(playerPrefab, transform);
        newPlayer.name = "Player " + transform.childCount;
        return newPlayer;
    }
}
