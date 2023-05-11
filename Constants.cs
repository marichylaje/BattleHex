using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MAX TERRAIN ID: 179
public static class Constants
{
    public static float playerDesface = 3f;
    public static float enemyDesface = -2f;
    public static int initialTerrainID = 90;
    public static int initialEnemyTerrainID = 173;
    public static float hexWidth = 1;
    public static float hexHeight = 1;
    public static float gap = 1.3f;
    public static int terrainWidth = 20;
    public static int terrainHeight = 9;
    public static float spriteTXSize = 16.3f;
    public static int spriteTYSize = 16;
    public static float spriteTYSizePenalization = 0.1f;

    public static TerrainType[] terrainTypes;

    public static void UpdateTerrainType(TerrainType[] newterrainTypes)
    {
        terrainTypes = newterrainTypes;
    }
    
    public static List<TerrainObjects> terrainObjects = new List<TerrainObjects>();

    public static void UpdateTerrainObjects(List<TerrainObjects> newTerrainObjects)
    {
        terrainObjects.Clear();
        terrainObjects.AddRange(newTerrainObjects);
    }
    public static void AddTerrainObject(TerrainObjects terrainObject)
    {
        terrainObjects.Add(terrainObject);
    }

    public static void ClearTerrainObjects()
    {
        terrainObjects.Clear();
    }

    /*public static List<TerrainObjects> GetTerrainObjects()
    {
        return terrainObjects;
    }*/
}
