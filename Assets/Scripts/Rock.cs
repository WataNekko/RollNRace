using UnityEngine;

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
}

public enum RockType
{
    Normal,
    Bonus,
    Fail
}
