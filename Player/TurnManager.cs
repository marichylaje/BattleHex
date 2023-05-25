using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

// ASIGNADO A END-TURN BUTTON como gameObject
public class TurnManager : MonoBehaviour
{
    public CreateHighlights createHighlights;
    public MoveLogic moveLogic;
    public SpellCastManager spellCastManager;
    public bool player1Turn = false;
    public bool isClickedHighlightedTerrain = false;
    //public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = false;
        player1Turn = false;
        Debug.Log("TurnMgr Empieza ejecutando primero automaticamente ENDTURN");
        Invoke("DelayedEndTurn", 0.5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            spellCastManager.isCasting = true;
        }

        if (spellCastManager.isCasting)
        {
            spellCastManager.ThrowSkill();
        }

        if(isClickedHighlightedTerrain){
            EndTurn();
        }
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
        float animSecondsLong = 4.0f;
        if (player1Turn && Constants.terrainObjects.Count > 0)
        {
            moveLogic.DestroyHighlighPlayableTerrain(500, -1);
            // Es el turno del jugador 1
            //step0: update turn flag
            player1Turn = false;
            Debug.Log("--------Dentro del IF player1Turn true--------");
            //moveLogic.DestroyHighlighPlayableTerrain(500, -1);

            //step1: change UI
            GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = true;
            GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = false;

            //mover la camara al jugador
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Obtener la posición del enemigo
                Vector3 playerPosition = player.transform.position;

                // Calcular la posición a la cual la cámara debe desplazarse
                Vector3 targetPosition = new Vector3(playerPosition.x, playerPosition.y, Camera.main.transform.position.z);

                // Desplazar la cámara hacia la posición del enemigo
                StartCoroutine(MoveCamera(targetPosition));
            }

            StartCoroutine(DelayedAction(1f, () => {
                createHighlights.WalkHighlight(moveLogic.movementDistance, Constants.terrainObjects);
            }));
        }
        else
        {
            // Es el turno del jugador 2
            //step0: update turn flag
            Debug.Log("--------Dentro del ELSE player1Turn false--------");
            player1Turn = true;

            //step2: destroy HighlightTerrain and update flag
            moveLogic.generateHighlightTerrain = false;

            //mover la camara al enemigo
            GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null)
            {
                // Obtener la posición del enemigo
                Vector3 enemyPosition = enemy.transform.position;

                // Calcular la posición a la cual la cámara debe desplazarse
                Vector3 targetPosition = new Vector3(enemyPosition.x, enemyPosition.y, Camera.main.transform.position.z);

                // Desplazar la cámara hacia la posición del enemigo
                if(spellCastManager.isThrowing){
                    StartCoroutine(DelayedAction(animSecondsLong, () => {
                        StartCoroutine(MoveCamera(targetPosition));
                        GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = false;
                        GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = true;
                    }));
                } else {
                    StartCoroutine(MoveCamera(targetPosition));
                    GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = false;
                    GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = true;
                }
            }            

            //step3: enemy IA action
            // TODO: cambiar esto por el mov de cada enemigo
            if(spellCastManager.isThrowing){
                StartCoroutine(DelayedAction(animSecondsLong, () => {
                    StartCoroutine(DelayedEnemyAction(1f));
                }));
            } else {
                StartCoroutine(DelayedEnemyAction(1f));
            }
            spellCastManager.isThrowing = false;
            isClickedHighlightedTerrain = false;

        }
    }

    private IEnumerator DelayedAction(float delay, Action function)
    {
        yield return new WaitForSeconds(delay);
        function();
    }

    private IEnumerator DelayedEnemyAction(float delay)
    {
        moveLogic.HideHighlighPlayableTerrain(500, -1);
        yield return new WaitForSeconds(delay);

        //step3: enemy IA action
        // TODO: cambiar esto por el mov de cada enemigo
        int enemyWalk = 2;
        TerrainObjects[] pathToPlayer = Helper.GetShorterPathToPlayer("Enemy", enemyWalk, Constants.terrainObjects);
        pathToPlayer = pathToPlayer.Where(path => path != null).ToArray();

        //createHighlights.HighlightList(pathToPlayer);
        if (pathToPlayer[enemyWalk-1] != null)
        {
            StartCoroutine(Helper.MovePlayerToPosition(pathToPlayer[enemyWalk-1].xPos, pathToPlayer[enemyWalk-1].yPos, "Enemy", 1.0f));
        }
        yield return new WaitForSeconds(delay / 2);
        EndTurn();
    }

    private IEnumerator MoveCamera(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(.3f);
        // Obtener la posición actual de la cámara
        Vector3 startPosition = Camera.main.transform.position;

        // Definir la duración del desplazamiento
        float duration = 1.0f;

        // Calcular el tiempo actual de interpolación
        float currentTime = 0.0f;

        // Realizar el desplazamiento suave de la cámara
        while (currentTime < duration)
        {
            // Incrementar el tiempo actual de interpolación
            currentTime += Time.deltaTime;

            // Calcular el valor interpolado entre la posición inicial y la posición objetivo
            float t = Mathf.Clamp01(currentTime / duration);
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Actualizar la posición de la cámara
            Camera.main.transform.position = newPosition;

            // Esperar un frame antes de continuar
            yield return null;
        }

        // Esperar 1 segundo adicional
        yield return new WaitForSeconds(1.0f);

        // Realizar el resto de las acciones del turno del jugador 2
        // ...
    }

}
