using UnityEngine;
using UnityEngine.Events;

public class PlayerManagerUI : Singleton<PlayerManagerUI>
{
    private PlayerSelector _selectedPlayer;
    public PlayerSelector SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            if (_selectedPlayer == value) return;

            _selectedPlayer?.SetShowHighlight(false);
            value?.SetShowHighlight(true);
            _selectedPlayer = value;
        }
    }

    [SerializeField]
    private Transform chooserContent;
    [SerializeField]
    private PlayerSelector playerSelectorPrefab;

    /// <summary>
    /// Adds a player to the player edit list in the UI.
    /// </summary>
    /// <param name="player">The player to add</param>
    /// <returns>The created PlayerSelector component</returns>
    private PlayerSelector AddPlayerToEditList(Player player)
    {
        var p = Instantiate(playerSelectorPrefab, chooserContent);
        p.Init(player);
        return p;
    }

    private void Start()
    {
        // Add existing players to the edit list
        foreach (var player in PlayerManager.Instance.GetPlayers())
        {
            AddPlayerToEditList(player);
        }
    }

    public UnityEvent OnEditPlayer;

    /// <summary>
    /// Triggers the event to edit the selected player.
    /// </summary>
    public void EditPlayer()
    {
        if (SelectedPlayer == null) return;
        OnEditPlayer.Invoke();
    }

    /// <summary>
    /// Removes the selected player from the game.
    /// </summary>
    public void RemovePlayer()
    {
        if (SelectedPlayer == null) return;

        // minimum 2 players
        if (PlayerManager.Instance.PlayerCount <= 2) return;

        var parent = SelectedPlayer.transform.parent;
        var selectedIndex = SelectedPlayer.transform.GetSiblingIndex();
        var nextSelectedIndex = selectedIndex == parent.childCount - 1
                // is last child
                ? selectedIndex - 1
                : selectedIndex + 1;
        var nextSelected = parent.GetChild(nextSelectedIndex);


        Destroy(SelectedPlayer.Player.gameObject);
        Destroy(SelectedPlayer.gameObject);
        SelectedPlayer = nextSelected.GetComponent<PlayerSelector>();
    }

    /// <summary>
    /// Adds a new player to the game.
    /// </summary>
    public void AddPlayer()
    {
        var newPlayer = PlayerManager.Instance.AddPlayer();
        SelectedPlayer = AddPlayerToEditList(newPlayer);
    }
}
