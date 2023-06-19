using UnityEngine;

public class RockPath : Singleton<RockPath>
{
    public int Length => transform.childCount;

    public Mesh BonusMesh;
    public Mesh FailMesh;

    public Transform GetRock(int index) => transform.GetChild(index);
}
