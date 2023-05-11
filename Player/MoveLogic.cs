using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : MonoBehaviour
{
    public CreateHighlights createHighlights;
    public PlayerStats playerStats;
    public TurnManager turnManager;
    public bool generateHighlightTerrain = false;
    bool flagInit = false;

    List<TerrainObjects> terrainObjects = Constants.terrainObjects;
    bool player1Turn;
    int idToMove;
    float playerDesface = Constants.playerDesface;
    public int movementDistance = 1;

    void Start()
    {
        //yield return new WaitForSeconds(0.1f);

        // Obtenemos la lista de objetos del terreno
        movementDistance = playerStats.movement;
        //yield return new WaitForSeconds(0.1f);

        // Obtenemos el objeto de terreno inicial y movemos el jugador a su posición
        
        player1Turn = turnManager.player1Turn;
        Debug.Log("Llamado Endturn desde el inicio de MoveLogic");
        turnManager.EndTurn();

        // para que se ejecute luego de ser transportado a su posicion inicial
        generateHighlightTerrain = true;
    }

    void Update()
    {
        player1Turn = turnManager.player1Turn;

        if(!flagInit && terrainObjects.Count > 0 && terrainObjects[0] != null){
            InitPositions();
            flagInit = true;
        }
        if(!generateHighlightTerrain && player1Turn && terrainObjects.Count > 0 && terrainObjects[0] != null){
            generateHighlightTerrain = false;
        }
        if(!player1Turn && !generateHighlightTerrain){
            int maxAttDistanceOnSkillsets = 1; // editar luego por cada enemigo
            Helper.GetMinDistanceTillEnemy("Enemy", maxAttDistanceOnSkillsets, terrainObjects);
            generateHighlightTerrain = true; // <--- eliminar esto?
        }
        if(!player1Turn){
            generateHighlightTerrain = true;
        }
    }

    void InitPositions(){
        TerrainObjects initTerrain = terrainObjects[Constants.initialTerrainID];
        TerrainObjects initEnemyTerrain = terrainObjects[Constants.initialEnemyTerrainID];
        Helper.MovePlayerToPosition(initTerrain.xPos, initTerrain.yPos, "Player");
        Helper.MovePlayerToPosition(initEnemyTerrain.xPos, initEnemyTerrain.yPos, "Enemy");
    }

    public void DestroyHighlighPlayableTerrain(int idMax, int idMin)
    {
        GameObject[] highlightTerrain = GameObject.FindGameObjectsWithTag("HighlightTerrain");
        foreach (GameObject terrainObject in highlightTerrain)
        {
            if (int.TryParse(terrainObject.name, out int terrainId)){
                terrainId = int.Parse(terrainObject.name);
                if ((terrainId >= idMax && terrainId <= idMax+499) || terrainId <= idMin)
                {
                    GameObject.Destroy(terrainObject.gameObject);                 
                }
            }
        }
    }

    /*
        PASOS IA ENEMIGA:
        0) calcular costo basico para llegar al player
        1) obtener terreno al rededor
        2) calcular cada camino que ayude a igualar la posicion objetivo con la posicion actual
        3) 
    */
}
