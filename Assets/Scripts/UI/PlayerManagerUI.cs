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

    private void Start()
    {
        foreach (var player in PlayerManager.Instance.GetPlayers())
        {
            var p = Instantiate(playerSelectorPrefab, chooserContent);
            p.Init(player);
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

    }

    public void AddPlayer()
    {

    }
}
