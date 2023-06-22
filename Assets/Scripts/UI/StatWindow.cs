using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWindow : MonoBehaviour
{
    [SerializeField]
    private Transform contentPanel;
    [SerializeField]
    private Image row;

    /// <summary>
    /// Fills the stat window with the provided player data.
    /// </summary>
    /// <param name="players">The players to display stats for</param>
    public void FillStat(IEnumerable<Player> players)
    {
        // clear children
        foreach (Transform child in contentPanel)
        {
            Destroy(child);
        }

        // fill content
        bool lightBg = true; // for alternating background color
        int i = 1;
        foreach (var player in players)
        {
            var img = Instantiate(row, contentPanel);

            SetStatRowText(img.transform, i, player);

            // alternate background color
            if (lightBg)
            {
                var color = img.color;
                color.a = 0;
                img.color = color;
            }
            lightBg = !lightBg;

            i++;
        }
    }

    private void SetStatRowText(Transform row, int index, Player player)
    {
        var fields = row.GetComponentsInChildren<TextMeshProUGUI>();
        fields[0].text = index.ToString();
        fields[1].text = player.name;
        fields[2].text = player.TurnCount.ToString();
        fields[3].text = player.BonusSectorCount.ToString();
        fields[4].text = player.FailSectorCount.ToString();
    }
}
