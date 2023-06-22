using TMPro;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightEffect;
    public void SetShowHighlight(bool value) => highlightEffect.SetActive(value);

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshRenderer meshRenderer;
    [SerializeField]
    private TextMeshProUGUI playerName;

    public Player Player { get; private set; }

    /// <summary>
    /// Initializes and link the player selector to the provided player object.
    /// </summary>
    /// <param name="player">The player to link to</param>

    public void Init(Player player)
    {
        Player = player;
        playerName.text = Player.name;
        meshFilter.sharedMesh = Player.Mesh;
        meshRenderer.sharedMaterial = Player.Color;
    }

    private float lastClickTime = 0f;
    // the maximum delay between clicks
    private float doubleClickDelay = 0.3f;

    /// <summary>
    /// Handles the click event on the player selector.
    /// </summary>
    public void HandleClick()
    {
        PlayerManagerUI.Instance.SelectedPlayer = this;

        if (Time.time - lastClickTime <= doubleClickDelay)
        {
            // Double-click
            PlayerManagerUI.Instance.EditPlayer();
        }

        lastClickTime = Time.time;
    }

    /// <summary>
    /// Sets the avatar mesh for the associated player.
    /// </summary>
    /// <param name="mesh">The new avatar mesh</param>
    public void SetAvatar(Mesh mesh)
    {
        Player.Mesh = mesh;
        meshFilter.sharedMesh = mesh;
    }

    /// <summary>
    /// Sets the avatar color for the associated player.
    /// </summary>
    /// <param name="material">The new avatar color material</param>
    public void SetAvatarColor(Material material)
    {
        Player.Color = material;
        meshRenderer.sharedMaterial = material;
    }

    /// <summary>
    /// Sets the name for the associated player.
    /// </summary>
    /// <param name="name">The new name</param>
    public void SetName(string name)
    {
        Player.name = name;
        playerName.text = name;
    }
}
