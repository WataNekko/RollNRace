using UnityEngine;

public class Players : Singleton<Players>
{
    public Transform GetPlayer(int index) => transform.GetChild(index);
}
