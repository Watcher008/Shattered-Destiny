using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    public class Caravan : MonoBehaviour
    {
        public List<Character> charactersInCaravarn;

        public Caravan()
        {
            charactersInCaravarn = new List<Character>();
        }
    }
}