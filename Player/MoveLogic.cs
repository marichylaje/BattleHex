using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLogic : MonoBehaviour
{
    public PlayerStats playerStats;
    public TurnManager turnManager;
    public float movSpeed;
    public bool generateHighlightTerrain = false;
    bool flagInit = false;

    List<TerrainObjects> terrainObjects = Constants.terrainObjects;
    bool player1Turn;
    int idToMove;
    float playerDesface = Constants.playerDesface;
    public int movementDistance = 1;

    void Start()
    {
        Constants.movSpeed = movSpeed;

        // Obtenemos la lista de objetos del terreno
        movementDistance = playerStats.movement;

        // sabemos en que turno se encuentra
        player1Turn = turnManager.player1Turn;

        // para que se ejecute el Highlight luego de ser transportado a su posicion inicial
        generateHighlightTerrain = true;
    }

    void Update()
    {
        // da la posicion inicial a todos
        if(!flagInit && terrainObjects.Count > 0 && terrainObjects[0] != null){
            InitPositions();
            flagInit = true;
        }

        /*if(!player1Turn && !generateHighlightTerrain){
            int maxAttDistanceOnSkillsets = 1; // editar luego por cada enemigo
            // Helper.GetMinDistanceTillEnemy("Enemy", maxAttDistanceOnSkillsets, terrainObjects);
            generateHighlightTerrain = true; // <--- eliminar esto?
        }*/

        // si no es nuestro turno, elimina los Highlights generados
        if(!player1Turn){
            generateHighlightTerrain = true;
        }

        if(!generateHighlightTerrain){
            DestroyHighlighPlayableTerrain(500, -1);
        }
    }

    void InitPositions(){
        float initMovSpeed = 0.1f;

        TerrainObjects initTerrain = terrainObjects[Constants.initialTerrainID];
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Helper.MovePlayerToPosition(initTerrain.xPos, initTerrain.yPos, player, initMovSpeed));

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < Constants.initialEnemyTerrainIDs.Length; i++){
            int enemyTerrainID = Constants.initialEnemyTerrainIDs[i];
            TerrainObjects initEnemyTerrain = terrainObjects[enemyTerrainID];
            StartCoroutine(Helper.MovePlayerToPosition(initEnemyTerrain.xPos, initEnemyTerrain.yPos, enemies[i], initMovSpeed));
        }
    }

    public void HideHighlighPlayableTerrain(int idMax, int idMin)
    {
        GameObject[] highlightTerrain = GameObject.FindGameObjectsWithTag("HighlightTerrain");
        foreach (GameObject terrainObject in highlightTerrain)
        {
            if (int.TryParse(terrainObject.name, out int terrainId))
            {
                terrainId = int.Parse(terrainObject.name);
                if ((terrainId >= idMax && terrainId <= idMax + 499) || terrainId <= idMin)
                {
                    Renderer renderer = terrainObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.enabled = false;  // Desactiva el componente Renderer para ocultar el objeto
                    }
                }
            }
        }
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
