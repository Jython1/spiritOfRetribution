using UnityEngine;

[CreateAssetMenu(fileName = "NewScriptableObjectScript", menuName = "Factions/Character Side")]
public class CharacterSide : ScriptableObject
{
    public string sideName;

    [Tooltip("Отношения к другим сторонам")]
    public SideRelation[] relations;

}

[System.Serializable]
public class SideRelation
{
    public CharacterSide otherSide;

    public RelationType relation;
    public enum RelationType
    {
        Enemy = -1,
        Neutral = 0,
        Friendly = 1
    }

    
}
