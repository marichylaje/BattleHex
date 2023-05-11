using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectClickTerrain : MonoBehaviour
{
    public TurnManager turnManager;

    void OnMouseDown()
    {
        GameObject clickedTerrain = gameObject;
        if(clickedTerrain){
            TerrainObjects terrainObjectsComponent = clickedTerrain.GetComponent<TerrainObjects>();
            if (terrainObjectsComponent != null) {
                Debug.Log("Highlighted Terrain CLICKED");
                float xPos = terrainObjectsComponent.xPos;
                float yPos = terrainObjectsComponent.yPos;
                Helper.MovePlayerToPosition(xPos, yPos, "Player");
                turnManager.EndTurn();
            }
            else {
                Debug.Log("TerrainObjects component not found in CLICKEDTERRAIN.");
            }
        }
    }
}
