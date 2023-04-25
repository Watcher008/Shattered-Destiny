namespace SD.CharacterSystem
{
    public class Relationship
    {
        public Character character;
        public RelationshipType type;
        public int disposition;

        public Relationship(Character other, RelationshipType type, int disp)
        {
            character = other;
            this.type = type;
            disposition = disp;
        }
    }
}

public enum RelationshipType
{
    //Familial
    Spouse,
    Lover,
    FormerLover,
    Child,
    Parent,
    Grandparent,
    Sibling,
    Aunt_Uncle,
    Niece_Nephew,
    Cousin,

    //Professional
    Apprentice,
    Master,
    FormerApprentice,
    FormerMaster,
    Partner,
    Associate,

    //Other
    Companion,
    CloseFriend,
    Friend,
    Acquaintance,

    //Negative
    Rival,
    Grudge
}