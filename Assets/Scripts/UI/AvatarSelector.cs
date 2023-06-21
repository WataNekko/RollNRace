using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightEffect;
    public void SetShowHighlight(bool value) => highlightEffect.SetActive(value);

    [SerializeField]
    private MeshFilter meshFilter;
    public Mesh Mesh => meshFilter.sharedMesh;

    [SerializeField]
    private MeshRenderer meshRenderer;
    public Material Color { set => meshRenderer.sharedMaterial = value; }

    public void HandleSelectAvatar()
        => PlayerEditorUI.Instance.SelectedAvatar = this;
}
