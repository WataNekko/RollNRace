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

    public void FillStat(IEnumerable<Player> players)
    {
        // clear children
        foreach (Transform child in contentPanel)
        {
            Destroy(child);
        }

        // fill content
        float totalHeight = 0f;
        bool lightBg = true; // for alternating background color
        int i = 1;
        foreach (var player in players)
        {
            var img = Instantiate(row, contentPanel);
            totalHeight += row.GetComponent<RectTransform>().rect.height;

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

        // set height for the scroll view port
        var tf = contentPanel.GetComponent<RectTransform>();
        tf.sizeDelta = new Vector2(tf.sizeDelta.x, totalHeight);
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
