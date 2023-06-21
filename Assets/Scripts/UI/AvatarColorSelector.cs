using UnityEngine;
using UnityEngine.UI;

public class AvatarColorSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightEffect;
    public void SetShowHighlight(bool value) => highlightEffect.SetActive(value);

    [SerializeField]
    private Image colorImage;
    public Material Color => colorImage.material;

    public void HandleSelectAvatarColor()
        => PlayerEditorUI.Instance.SelectedAvatarColor = this;
}
