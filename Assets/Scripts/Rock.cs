using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Rock : MonoBehaviour
{
    [SerializeField]
    private RockType _type;
    public RockType Type
    {
        get => _type;
        set
        {
            _type = value;
            switch (_type)
            {
                case RockType.Bonus:
                    GetComponent<MeshFilter>().sharedMesh = RockPath.Instance.BonusMesh;
                    break;
                case RockType.Fail:
                    GetComponent<MeshFilter>().sharedMesh = RockPath.Instance.FailMesh;
                    break;
            }
        }
    }

    /// <summary>
    /// Activates the effect of the rock on the current player.
    /// </summary>
    public IEnumerator ActivateEffectOnCurrentPlayer()
    {
        var player = GameManager.Instance.CurrentPlayer;

        switch (Type)
        {
            case RockType.Fail:
                player.FailSectorCount++;
                yield return new WaitForSeconds(.5f);
                yield return player.MoveSteps(-3);
                break;
            case RockType.Bonus:
                player.BonusSectorCount++;
                player.CurrentTurnGain++;
                break;
        }
    }
}

public enum RockType
{
    Normal,
    Bonus,
    Fail
}
