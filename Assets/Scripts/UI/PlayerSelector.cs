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

    public void SetAvatar(Mesh mesh)
    {
        Player.Mesh = mesh;
        meshFilter.sharedMesh = mesh;
    }

    public void SetAvatarColor(Material material)
    {
        Player.Color = material;
        meshRenderer.sharedMaterial = material;
    }

    public void SetName(string name)
    {
        Player.name = name;
        playerName.text = name;
    }
}
