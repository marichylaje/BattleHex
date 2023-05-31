using System.Collections;
using UnityEngine;

public class DetectClickTerrain : MonoBehaviour
{
    public TurnManager turnManager;
    public SpellCastManager spellCastManager;

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
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                StartCoroutine(Helper.MovePlayerToPosition(xPos, yPos, player, Constants.movSpeed));

                turnManager.isClickedHighlightedTerrain = true;
                if(spellCastManager.isCasting){
                    spellCastManager.isChangingSpell = false;
                }
            }
        }
    }
}
