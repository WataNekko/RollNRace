using TMPro;
using UnityEngine;

public class HUD : Singleton<HUD>
{
    [field: SerializeField]
    public TextMeshProUGUI TurnText { get; private set; }

}
