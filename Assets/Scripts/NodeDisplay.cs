using UnityEngine;
using TMPro;
using SD.PathingSystem;
using SD.ECS;

public class NodeDisplay : MonoBehaviour
{
    private Pathfinding pathfinding;
    private MapCharacter playerPosition;

    [SerializeField] private WorldNodeReference playerTravelData;

    [SerializeField] private TMP_Text currentNodeText;
    [SerializeField] private TMP_Text hoverNodeText;
    [SerializeField] private GameObject hoverNodePanel;

    private void Start()
    {
        pathfinding = Pathfinding.instance;
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<MapCharacter>();
    }

    private void Update()
    {
        SetCurrentNodeText();
        SetTargetNodeText();
    }

    private void SetCurrentNodeText()
    {
        if (playerPosition.Node == null) return;
        currentNodeText.text = "At: " + playerPosition.Node.Terrain.name + " : " + playerPosition.Node.X + "," + playerPosition.Node.Y;
    }

    private void SetTargetNodeText()
    {
        var node = playerTravelData.NodeReference;
        hoverNodePanel.SetActive(node != null);
        if (node == null) return;

        hoverNodeText.text = "Look: " + node.Terrain.name + " : " + node.X + "," + node.Y + " : " + node.WorldPosition;
    }
}
