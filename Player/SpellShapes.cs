using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapes : MonoBehaviour
{
    public static TerrainObjects[] GetStraightLine(int distance, int direction, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects)
    {
        Vector2[] coords = new Vector2[distance];
        List<TerrainObjects> response = new List<TerrainObjects>(); // Crear un arreglo de tamaño 1
        
        bool IsWithinMapBounds(Vector2 position){
            return position.x >= 0 && position.x < Constants.maxXsizeMap && position.y >= 0 && position.y < Constants.maxYsizeMap;
        }

        for(int i = 1; i <= distance; i++){
            Vector2[] data = Helper.GetSurroundingCoords(i, terrainUnder, terrainObjects).ToArray();

            if(direction == 1 /*topR*/ && IsWithinMapBounds(data[4])){
                coords[i - 1] = data[4];
            } else if(direction == 2 /*top*/ && IsWithinMapBounds(data[0])){
                coords[i - 1] = data[0];
            } else if(direction == 3 /*topL*/ && IsWithinMapBounds(data[2])){
                coords[i - 1] = data[2];
            } else if(direction == 4 /*botL*/ && IsWithinMapBounds(data[3])){
                coords[i - 1] = data[3];
            } else if(direction == 5 /*bot*/ && IsWithinMapBounds(data[1])){
                coords[i - 1] = data[1];
            } else if(direction == 6 /*botR*/ && IsWithinMapBounds(data[5])){
                coords[i - 1] = data[5];            
            }
        }

        for (int i = 0; i < coords.Length; i++) {
            TerrainObjects terrainObject = Helper.GetTerrainDataFromCoords(coords[i].x, coords[i].y, false, terrainObjects);
            if (terrainObject != null && !response.Contains(terrainObject)) {
                response.Add(terrainObject);
            }
        }
        return response.ToArray();
    }
}
