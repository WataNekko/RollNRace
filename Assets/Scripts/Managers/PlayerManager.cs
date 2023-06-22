using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField]
    private Player playerPrefab;

    /// <summary>
    /// Retrieves all current players in the game.
    /// </summary>
    /// <returns>An array of all current players.</returns>
    public Player[] GetPlayers() => GetComponentsInChildren<Player>();

    /// <summary>
    /// Retrieves the current number of players in the game.
    /// </summary>
    public int PlayerCount => transform.childCount;

    /// <summary>
    /// Adds a new player to the game.
    /// </summary>
    /// <returns>The newly added player.</returns>
    public Player AddPlayer()
    {
        var newPlayer = Instantiate(playerPrefab, transform);
        newPlayer.name = "Player " + PlayerCount;
        return newPlayer;
    }
}
