using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD.PathingSystem;

namespace SD.LocationSystem
{
    public class LocationObject : MonoBehaviour
    {
        [SerializeField] private LocationPreset locationInfo;

        private void Start()
        {
            var node = Pathfinding.instance.GetNode(transform.position);


        }
    }

}