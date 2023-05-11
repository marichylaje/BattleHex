using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Numeros para IDs
    - 0 a 499 normales
    - 500 a 999 highlighted movement
    - 1000 a 1499 attack highlight

    terreno grafico:
    - de 16 unidades, en 16 unidades aumenta en el eje Y
    - de 16.3 unidades, en 16.3 unidades aumenta en el eje X, agregando un 0.1 cuando id % 2 !=
*/

public class GenerateTerrain : MonoBehaviour
{

    public TerrainType[] terrainTypes;
    private GridData[] gridPositions;
    private List<TerrainObjects> terrainObjects = new List<TerrainObjects>();
    private bool flag = true;
    public GameObject terrainsFolder;
    private void Update() {
        if(flag && terrainTypes != null && terrainTypes.Length > 0){
            Initialize();
            flag = false;
        }
        
    }

    public void Initialize()
    {
        gridPositions = Helper.GenerateGridPositions(Constants.terrainHeight);

        // PARA ARRAYS, para Lists, se usa el otro .Count()
        if(terrainTypes != null && terrainTypes.Length > 0){
            Constants.UpdateTerrainType(terrainTypes);

            // Lógica para generar los datos del terreno
            for (int i = 0; i < gridPositions.Length; i++)
            {
                GridData position = gridPositions[i];
                TerrainType randomTerrain = Helper.SelectTerrainType(Helper.GetRandomProbability(terrainTypes));
                TerrainObjects terrain = CreateTerrain(position, randomTerrain.name, false, 0);
                terrainObjects.Add(terrain);
            }
        }

        Constants.UpdateTerrainObjects(terrainObjects);
    }

    public TerrainObjects CreateTerrain(GridData position, string name, bool isHighlighted, int idAddNumber)
    {
        TerrainObjects terrainObjectFinale = null;
        foreach (TerrainType terrainType in Constants.terrainTypes)
        {
            if (terrainType.name == name)
            {
                GameObject newTerrainObject = GameObject.Instantiate(isHighlighted ? terrainType.highlighted : terrainType.type, new Vector3(position.x, position.y, 0), Quaternion.identity);
                newTerrainObject.transform.SetParent(terrainsFolder.transform);

                newTerrainObject.name = "" + (position.id + (isHighlighted ? 500 : 0));
                newTerrainObject.tag = (isHighlighted ? "HighlightTerrain" : "Terrain");

                TerrainObjects terrainObject = newTerrainObject.AddComponent<TerrainObjects>();
                terrainObject.id = position.id + idAddNumber;
                terrainObject.xPos = position.x;
                terrainObject.yPos = position.y;
                terrainObject.terrainType = terrainType;
                terrainObject.name = terrainType.name;

                terrainObjectFinale = terrainObject;
                break;
            }
        }
        return terrainObjectFinale;
    }


    public void HighlighPlayableTerrain(TerrainObjects[] adjacentTerrains)
    {
        foreach (TerrainObjects terrainObject in adjacentTerrains)
        {
            CreateTerrain(new GridData(
                terrainObject.xPos,
                terrainObject.yPos,
                terrainObject.id
            ), terrainObject.name, true, 500);
        }
    }
}
public class GridData
{
    public float x;
    public float y;
    public int id;

    public GridData(float x, float y, int id)
    {
        this.x = x;
        this.y = y;
        this.id = id;
    }
}
[System.Serializable]
public class TerrainType
{
    public string name;
    public GameObject type;
    public GameObject highlighted;
    public float probability;
    public string effect;
    public int effectDmg;
    public float movilityCost;
    public float bonusCost;
}

public class TerrainObjects : MonoBehaviour
{
    public int id;
    public float xPos;
    public float yPos;
    public TerrainType terrainType;
    public string name;
}