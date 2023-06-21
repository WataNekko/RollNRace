using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private MeshFilter turnMeshFilter;
    [SerializeField]
    private MeshRenderer turnMeshRenderer;

    public Player PlayerTurn
    {
        set
        {
            turnText.text = $"{value.name}'s turn";
            turnMeshFilter.sharedMesh = value.Mesh;
            turnMeshRenderer.sharedMaterial = value.Color;
        }
    }
}
