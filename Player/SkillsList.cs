using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsList : MonoBehaviour
{
    //public TerrainGenerator terrainGenerator;

    public SkillType[] skills;
}

public enum Effects
{
    Fire,
    Cold
}

public enum ElementTypes
{
    Fire,
    Water,
    Earth,
    Wind,
    Poison
}
public enum ThrowTypes
{
    selectedCircle,
    singleLine,
    tripleLine,
    selfCircle,
    tentacle,
}

[System.Serializable]
public class SkillType
{
    public string name;
    public GameObject image;
    public int damage;
    public float probabilityToAppear;
    public Effects effect;
    public int effectAreaSize;
    public int amountOfTurns;
    public ElementTypes elementType;
    public ThrowTypes throwType;

}