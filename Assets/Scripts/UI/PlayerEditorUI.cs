using TMPro;
using UnityEngine;

public class PlayerEditorUI : Singleton<PlayerEditorUI>
{
    [SerializeField]
    private TMP_InputField nameInput;
    [SerializeField]
    private Transform avatarSelectors;
    [SerializeField]
    private Transform avatarColorSelectors;


    private AvatarSelector _selectedAvatar;
    public AvatarSelector SelectedAvatar
    {
        get => _selectedAvatar;
        set
        {
            if (_selectedAvatar == value) return;

            _selectedAvatar?.SetShowHighlight(false);
            value?.SetShowHighlight(true);
            _selectedAvatar = value;

            PlayerManagerUI.Instance.SelectedPlayer.SetAvatar(value.Mesh);
        }
    }

    private AvatarColorSelector _selectedAvatarColor;
    public AvatarColorSelector SelectedAvatarColor
    {
        get => _selectedAvatarColor;
        set
        {
            if (_selectedAvatarColor == value) return;

            _selectedAvatarColor?.SetShowHighlight(false);
            value?.SetShowHighlight(true);
            _selectedAvatarColor = value;

            PlayerManagerUI.Instance.SelectedPlayer.SetAvatarColor(value.Color);

            // Apply the selected avatar color to all avatar selectors
            foreach (var ava in avatarSelectors
                    .GetComponentsInChildren<AvatarSelector>())
            {
                ava.Color = value.Color;
            }

        }
    }

    /// <summary>
    /// The name of the selected player.
    /// </summary>
    public string Name
    {
        get => PlayerManagerUI.Instance.SelectedPlayer.Player.name;
        set => PlayerManagerUI.Instance.SelectedPlayer.SetName(value);
    }

    /// <summary>
    /// Initializes the player editor with the selected player's settings.
    /// </summary>
    public void Init()
    {
        var selectedPlayer = PlayerManagerUI.Instance.SelectedPlayer.Player;

        // init avatar
        foreach (var ava in avatarSelectors
                .GetComponentsInChildren<AvatarSelector>())
        {
            if (selectedPlayer.Mesh == ava.Mesh)
            {
                SelectedAvatar = ava;
                break;
            }
        }

        // init avatar color
        foreach (var avaColor in avatarColorSelectors
                .GetComponentsInChildren<AvatarColorSelector>())
        {
            if (selectedPlayer.Color == avaColor.Color)
            {
                SelectedAvatarColor = avaColor;
                break;
            }
        }

        // init name
        nameInput.text = Name;
    }
}
