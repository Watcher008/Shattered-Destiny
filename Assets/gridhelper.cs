using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridhelper : MonoBehaviour
{
    [SerializeField] private float tileSpacing = 4.1f;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                transform.GetChild(x * 10 + z).position = new Vector3(x * tileSpacing, 0, z * tileSpacing);
            }
        }
    }
}
