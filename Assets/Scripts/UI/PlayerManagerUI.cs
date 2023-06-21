using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    private void AddPlayerToEditList(Player player)
    {
        var p = Instantiate(playerSelectorPrefab, chooserContent);
        p.Init(player);
    }

    private void Start()
    {
        foreach (var player in PlayerManager.Instance.GetPlayers())
        {
            AddPlayerToEditList(player);
        }
    }

    public UnityEvent OnEditPlayer;

    public void EditPlayer()
    {
        if (SelectedPlayer == null) return;
        OnEditPlayer.Invoke();
    }

    public void RemovePlayer()
    {
        if (SelectedPlayer == null) return;

        // minimum 2 players
        if (PlayerManager.Instance.PlayerCount <= 2) return;

        Destroy(SelectedPlayer.Player.gameObject);
        Destroy(SelectedPlayer.gameObject);
        SelectedPlayer = null;
    }

    public void AddPlayer()
    {
        var newPlayer = PlayerManager.Instance.AddPlayer();
        AddPlayerToEditList(newPlayer);
    }
}
