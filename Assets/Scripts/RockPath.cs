using UnityEngine;

public class RockPath : Singleton<RockPath>
{
    /// <summary>
    /// The length of the rock path.
    /// </summary>
    public int Length => transform.childCount;

    public Mesh BonusMesh;
    public Mesh FailMesh;

    /// <summary>
    /// Returns the rock at the specified index.
    /// </summary>
    /// <param name="index">The index of the rock.</param>
    /// <returns>The transform of the rock at the specified index.</returns>
    public Transform GetRock(int index) => transform.GetChild(index);

    /// <summary>
    /// Returns an array of all the rocks in the rock path.
    /// </summary>
    /// <returns>An array of Rock components representing the rocks in the rock path.</returns>
    public Rock[] GetRocks() => GetComponentsInChildren<Rock>();
}
