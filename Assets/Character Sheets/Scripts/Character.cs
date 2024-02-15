using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD.CharacterSystem
{
    public class Character
    {
        public string Name { get; private set; }
        public Professions Profession { get; private set; }

        public Character(string name, Professions profession)
        {
            this.Name = name;
            this.Profession = profession;
        }
    }
}

public enum Professions
{
    Armorer,
    Artisan,
    Artist,
    Baker,
    Barrister,
    Blacksmith,
    Carpenter,
    Elder,
    Farmer,
    Fisherman,
    Guard,
    Miller,
    Minstrel,
    Noble,
    Performer,
    Roofer,
    Shoemaker,
    Soldier,
    Stonemason,
    Unemployed,
    Urchin,
    Weaver,
}