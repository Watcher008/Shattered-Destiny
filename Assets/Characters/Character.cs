using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    public class Character
    {
        public string name;
        public List<Relationship> relationships;

        public Character(string name)
        {
            this.name = name;
            relationships = new List<Relationship>();
        }
    }
}