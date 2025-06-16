using System.Linq;


public static class FactionRelations
{

    public static SideRelation.RelationType GetRelation(CharacterSide sideA, CharacterSide sideB)
    {
        if (!sideA || !sideB)
            return SideRelation.RelationType.Neutral;

        var rel = sideA.relations?.FirstOrDefault(r => r.otherSide == sideB);
        return rel?.relation ?? SideRelation.RelationType.Neutral;

    }

    public static bool IsEnemy(CharacterSide sideA, CharacterSide sideB) =>
        GetRelation(sideA, sideB) == SideRelation.RelationType.Enemy;

    public static bool IsFriendly(CharacterSide sideA, CharacterSide sideB) =>
        GetRelation(sideA, sideB) == SideRelation.RelationType.Friendly;

    public static bool IsNeutral(CharacterSide sideA, CharacterSide sideB) =>
        GetRelation(sideA, sideB) == SideRelation.RelationType.Neutral;

}
