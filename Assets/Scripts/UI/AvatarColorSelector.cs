using UnityEngine;
using UnityEngine.UI;

public class AvatarColorSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightEffect;

    /// <summary>
    /// Sets the visibility of the highlight effect.
    /// </summary>
    /// <param name="value">The visibility state of the highlight effect.</param>
    public void SetShowHighlight(bool value) => highlightEffect.SetActive(value);

    [SerializeField]
    private Image colorImage;

    /// <summary>
    /// Retrieves the material of the color image.
    /// </summary>
    public Material Color => colorImage.material;

    public void HandleSelectAvatarColor()
        => PlayerEditorUI.Instance.SelectedAvatarColor = this;
}
