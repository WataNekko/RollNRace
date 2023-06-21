using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    private Image diceImage;
    [SerializeField]
    private Sprite[] diceSprites;

    private void HandleDiceRolled(int rolledValue)
    {
        StopAllCoroutines();
        StartCoroutine(DisplayDiceResult(rolledValue));
    }

    private IEnumerator DisplayDiceResult(int rolledValue)
    {
        diceImage.overrideSprite = diceSprites[rolledValue - 1];
        diceImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        diceImage.gameObject.SetActive(false);
    }

    private void OnEnable() => Dice.Instance.OnDiceRolled += HandleDiceRolled;
    private void OnDisable() => Dice.Instance.OnDiceRolled -= HandleDiceRolled;
}
