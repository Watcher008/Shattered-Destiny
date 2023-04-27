using UnityEngine;
using TMPro;
using SD.PathingSystem;
using SD.ECS;

public class NodeDisplay : MonoBehaviour
{
    private Pathfinding pathfinding;
    private Position playerPosition;

    [SerializeField] private WorldNodeReference playerTravelData;

    [SerializeField] private TMP_Text currentNodeText;
    [SerializeField] private TMP_Text hoverNodeText;
    [SerializeField] private GameObject hoverNodePanel;

    private void Start()
    {
        pathfinding = Pathfinding.instance;
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Position>();
    }

    private void Update()
    {
        SetCurrentNodeText();
        SetTargetNodeText();
    }

    private void SetCurrentNodeText()
    {
        var node = pathfinding.GetNode(playerPosition.x, playerPosition.y);
        if (node == null) return;
        currentNodeText.text = "At: " + node.terrain.name + " : " + node.x + "," + node.y;
    }

    private void SetTargetNodeText()
    {
        var node = playerTravelData.NodeReference;
        hoverNodePanel.SetActive(node != null);
        if (node == null) return;

        hoverNodeText.text = "Look: " + node.terrain.name + " : " + node.x + "," + node.y + " : " + node.globalPosition;
    }
}
