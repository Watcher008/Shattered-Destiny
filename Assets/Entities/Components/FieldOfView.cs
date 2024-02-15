using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.ECS
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private int sightDistance = 5;

        private MapCharacter player;

        private void Start()
        {
            player = GetComponent<MapCharacter>();
            Invoke("UpdateFieldOfView", 0.1f);
        }

        private void UpdateFieldOfView()
        {
            foreach (var actor in GameManager.Actors)
            {
                var character = actor.GetComponent<MapCharacter>();
                if (Pathfinding.instance.GetNodeDistance_Straight(player.Node, character.Node) <= sightDistance)
                {
                    character.SetVisibility(2);
                }
                else
                {
                    character.SetVisibility(0);
                }
            }

            // Also need to do this for all locations, but set already-discovered locations to faded (1)
        }
    }
}