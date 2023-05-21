using System.Collections;
using UnityEngine;

public class DetectClickTerrain : MonoBehaviour
{
    public TurnManager turnManager;
    //private bool isMoving = false; // Variable para controlar si el jugador está en movimiento

    void OnMouseDown()
    {
        //if (isMoving) return; // Si el jugador ya está en movimiento, ignora el clic

        GameObject clickedTerrain = gameObject;
        if (clickedTerrain)
        {
            TerrainObjects terrainObjectsComponent = clickedTerrain.GetComponent<TerrainObjects>();
            if (terrainObjectsComponent != null)
            {
                TerrainObjects terrainUnder = Helper.GetTerrainDataFromCoords(terrainObjectsComponent.xPos, terrainObjectsComponent.yPos, false, Constants.terrainObjects);
                float xPos = terrainUnder.xPos;
                float yPos = terrainUnder.yPos;

                Debug.Log("HERE CLICKED: " + terrainUnder.id + " xPos: " + xPos + " and yPos: " + yPos);
                turnManager.EndTurn();

                StartCoroutine(Helper.MovePlayerToPosition(xPos, yPos, "Player", Constants.movSpeed));
            }
        }
    }
}
