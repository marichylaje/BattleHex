using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapes : MonoBehaviour
{
    public TerrainObjects[] GetStraightLine(int distance, int direction, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects)
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

    public TerrainObjects[] GetWallLine(int direction, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects)
    {
        Vector2[] coords = new Vector2[4];
        List<TerrainObjects> response = new List<TerrainObjects>(); // Crear un arreglo de tamaño 1
        
        bool IsWithinMapBounds(Vector2 position){
            return position.x >= 0 && position.x < Constants.maxXsizeMap && position.y >= 0 && position.y < Constants.maxYsizeMap;
        }

        Vector2[] data = Helper.GetSurroundingCoords(3, terrainUnder, terrainObjects).ToArray();

        if(/*topR*/ direction == 1 && IsWithinMapBounds(data[4])){
            coords[1] = data[0]; //top
            coords[2] = data[4]; //topR
            coords[3] = data[7]; //topRL
            coords[0] = data[13]; //topRR
        } else if(/*topL*/ direction == 2 && IsWithinMapBounds(data[0])){
            coords[1] = data[0]; //top
            coords[2] = data[2]; //topL
            coords[3] = data[6]; //topLR
            coords[0] = data[12]; //topLL
        } else if(/*Left*/ direction == 3 && IsWithinMapBounds(data[2])){
            coords[1] = data[2]; //topL
            coords[2] = data[8]; //leftT
            coords[3] = data[14]; //leftM
            coords[0] = data[3];  //leftB
        } else if(/*botL*/ direction == 4 && IsWithinMapBounds(data[3])){
            coords[1] = data[3]; //botL
            coords[2] = data[1]; // bot
            coords[3] = data[10]; //botR
            coords[0] = data[16]; //botL
        } else if(/*botR*/ direction == 5 && IsWithinMapBounds(data[1])){
            coords[1] = data[1]; //bot
            coords[2] = data[5]; // botR
            coords[3] = data[17]; //botM
            coords[0] = data[11]; //botL
        } else if(/*Right*/ direction == 6 && IsWithinMapBounds(data[5])){
            coords[1] = data[4]; // topR
            coords[2] = data[5]; // botR
            coords[3] = data[9]; // rightT
            coords[0] = data[15]; //RightB         
        }

        for (int i = 0; i < coords.Length; i++) {
            TerrainObjects terrainObject = Helper.GetTerrainDataFromCoords(coords[i].x, coords[i].y, false, terrainObjects);
            if (terrainObject != null && !response.Contains(terrainObject)) {
                response.Add(terrainObject);
            }
        }
        return response.ToArray();
    }

    public TerrainObjects[] GetMouseCircle(bool isRing, int distance, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects)
    {
        Vector2[] coords = new Vector2[4];
        List<TerrainObjects> response = new List<TerrainObjects>(); // Crear un arreglo de tamaño 1
        
        bool IsWithinMapBounds(Vector2 position){
            return position.x >= 0 && position.x < Constants.maxXsizeMap && position.y >= 0 && position.y < Constants.maxYsizeMap;
        }

        Vector2[] data = Helper.GetSurroundingCoords(1, terrainUnder, terrainObjects).ToArray();
        
        for (int i = 0; i < data.Length; i++) {
            TerrainObjects terrainObject = Helper.GetTerrainDataFromCoords(data[i].x, data[i].y, false, terrainObjects);
            if (terrainObject != null && !response.Contains(terrainObject)) {
                response.Add(terrainObject);
            }
        }
        if(!isRing){
            response.Add(terrainUnder);
        }
        /*
        for (int i = 0; i < coords.Length; i++) {
            TerrainObjects terrainObject = Helper.GetTerrainDataFromCoords(coords[i].x, coords[i].y, false, terrainObjects);
            if (terrainObject != null && !response.Contains(terrainObject)) {
                response.Add(terrainObject);
            }
        }*/
        return response.ToArray();
    }
}
