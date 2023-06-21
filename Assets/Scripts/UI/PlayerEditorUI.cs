using UnityEngine;

public class PlayerEditorUI : Singleton<PlayerEditorUI>
{
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

            foreach (var ava in avatarSelectors
                    .GetComponentsInChildren<AvatarSelector>())
            {
                ava.Color = value.Color;
            }

        }
    }

    public void Init()
    {
        var selectedPlayer = PlayerManagerUI.Instance.SelectedPlayer.Player;

        foreach (var ava in avatarSelectors
                .GetComponentsInChildren<AvatarSelector>())
        {
            if (selectedPlayer.MeshFilter.sharedMesh == ava.Mesh)
            {
                SelectedAvatar = ava;
                break;
            }
        }

        foreach (var avaColor in avatarColorSelectors
                .GetComponentsInChildren<AvatarColorSelector>())
        {
            if (selectedPlayer.MeshRenderer.sharedMaterial == avaColor.Color)
            {
                SelectedAvatarColor = avaColor;
                break;
            }
        }
    }
}
