using UnityEngine;
using TMPro;

public class NodeDisplay : MonoBehaviour
{
    [SerializeField] private PlayerTravelData playerTravelData;

    [SerializeField] private TMP_Text currentNodeText;
    [SerializeField] private TMP_Text hoverNodeText;
    [SerializeField] private GameObject hoverNodePanel;

    private void Update()
    {
        SetCurrentNodeText();
        SetTargetNodeText();
    }

    private void SetCurrentNodeText()
    {
        var node = playerTravelData.CurrentPlayerNode;
        if (node == null) return;
        currentNodeText.text = "At: " + node.terrain.name + " : " + node.x + "," + node.y;
    }

    private void SetTargetNodeText()
    {
        var node = playerTravelData.HoverNode;
        hoverNodePanel.SetActive(node != null);
        if (node == null) return;

        hoverNodeText.text = "Look: " + node.terrain.name + " : " + node.x + "," + node.y;
    }
}
