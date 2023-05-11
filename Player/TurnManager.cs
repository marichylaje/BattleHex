using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// ASIGNADO A END-TURN BUTTON como gameObject
public class TurnManager : MonoBehaviour
{
    public CreateHighlights createHighlights;
    public MoveLogic moveLogic;
    public bool player1Turn = false;
    int controlFlag = 0;
    //public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = false;
        player1Turn = false;
        Debug.Log("TurnMgr Empieza ejecutando primero automaticamente ENDTURN");
        Invoke("DelayedEndTurn", 0.5f);
        Invoke("DelayedEndTurn", 0.5f);
    }

    void OnMouseDown(){
        GameObject turnManagerButton = gameObject;
        if(turnManagerButton){
            Debug.Log("//END TURN btn CLICKED");
            EndTurn();
        }
    }
    void DelayedEndTurn()
    {
        EndTurn();
    }

    public void EndTurn()
    {
        if (player1Turn && Constants.terrainObjects.Count > 0)
        {
            // Es el turno del jugador 1
            //step0: update turn flag
            player1Turn = false;
            Debug.Log("--------Dentro del IF player1Turn true--------");

            //step1: change UI
            GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = true;
            GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = false;

            //step2: highlightTerrain
            createHighlights.WalkHighlight(moveLogic.movementDistance, Constants.terrainObjects);

            //step3: if has clicked terrain, move action (already in DetectClickTerrain.cs)
            //step4: if has clicked skills, attack action
            controlFlag++;
        }
        else
        {
            // Es el turno del jugador 2
            //step0: update turn flag
            player1Turn = true;
            Debug.Log("--------Dentro del ELSE player1Turn false--------");

            //step1: change UI            
            GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = false;
            GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = true;

            //step2: destroy HighlightTerrain and update flag
            moveLogic.generateHighlightTerrain = false;
            moveLogic.DestroyHighlighPlayableTerrain(500, -1);

            //step3: enemy IA action
            int enemyWalk = 2;
            TerrainObjects[] pathToPlayer = Helper.GetShorterPathToPlayer("Enemy", enemyWalk, Constants.terrainObjects);
            Debug.Log("HERE 0------------------------------------------------------------------>: " + Constants.terrainObjects[0].id);
            Debug.Log("HERE 1------------------------------------------------------------------>: " + Constants.terrainObjects[1].id);
            //;
            //Debug.Log("HERE 3------------------------------------------------------------------>: " + pathToPlayer[3].id);
            // createHighlights.HighlightList(pathToPlayer);
            //Helper.MovePlayerToPosition(pathToPlayer[enemyWalk-1].xPos, pathToPlayer[enemyWalk-1].yPos, "Enemy");
        }
    }
}
