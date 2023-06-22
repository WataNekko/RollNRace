using UnityEngine;

public class AvatarSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightEffect;

    /// <summary>
    /// Sets the visibility of the highlight effect.
    /// </summary>
    /// <param name="value">The visibility state of the highlight effect.</param>
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
