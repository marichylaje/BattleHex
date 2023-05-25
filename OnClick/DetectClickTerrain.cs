using System.Collections;
using UnityEngine;

public class DetectClickTerrain : MonoBehaviour
{
    public TurnManager turnManager;

    void OnMouseDown()
    {
        GameObject clickedTerrain = gameObject;
        if (clickedTerrain)
        {
            TerrainObjects terrainObjectsComponent = clickedTerrain.GetComponent<TerrainObjects>();
            if (terrainObjectsComponent != null)
            {
                TerrainObjects terrainUnder = Helper.GetTerrainDataFromCoords(terrainObjectsComponent.xPos, terrainObjectsComponent.yPos, false, Constants.terrainObjects);
                float xPos = terrainUnder.xPos;
                float yPos = terrainUnder.yPos;
                StartCoroutine(Helper.MovePlayerToPosition(xPos, yPos, "Player", Constants.movSpeed));

                //TODO: esto deberia estar en un doc especial que controle los turnos, y no ser llamado aca
                Debug.Log("1");
                turnManager.isClickedHighlightedTerrain = true;
            }
        }
    }
}
