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

    public IEnumerator ActivateEffectOnCurrentPlayer()
    {
        var player = GameManager.Instance.CurrentPlayer;

        switch (Type)
        {
            case RockType.Fail:
                yield return player.MoveSteps(-3);
                break;
            case RockType.Bonus:
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
