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

        Invoke("DelayedEndTurn", 0.5f);
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.W))
        {
            spellCastManager.isCasting = true;
        }*/

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
            player1Turn = true;
            moveLogic.HideHighlighPlayableTerrain(500, -1);
            float turnSpeed = 1f;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject.FindWithTag("myTurnIcon").GetComponent<Image>().enabled = false;
            GameObject.FindWithTag("enemyTurnIcon").GetComponent<Image>().enabled = true;

            //step2: destroy HighlightTerrain and update flag
            moveLogic.generateHighlightTerrain = false;

            //mover la camara al enemigo
            StartCoroutine(EnemyTurn(turnSpeed));

            
            spellCastManager.isThrowing = false;
            isClickedHighlightedTerrain = false;
            StartCoroutine(DelayedAction((turnSpeed * 1.45f) * enemies.Length, () => {EndTurn();}));
        }
    }

    private IEnumerator EnemyTurn(float turnSpeed){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                // Obtener la posición del enemigo
                Vector3 enemyPosition = enemy.transform.position;

                // Calcular la posición a la cual la cámara debe desplazarse
                Vector3 targetPosition = new Vector3(enemyPosition.x, enemyPosition.y, Camera.main.transform.position.z);

                // Esperar hasta que la cámara termine de moverse antes de pasar al siguiente enemigo
                yield return StartCoroutine(MoveCamera(targetPosition));
            }

            // Esperar un tiempo antes de que el siguiente enemigo realice su acción
            //yield return new WaitForSeconds(turnSpeed);

            // Realizar la acción del enemigo
            EnemyAction(turnSpeed / 3, enemy);
        }
    }
    private void EnemyAction(float turnSpeed, GameObject enemy){
        if(spellCastManager.isThrowing){
            StartCoroutine(DelayedAction(turnSpeed, () => {
                StartCoroutine(DelayedEnemyAction(turnSpeed, enemy));
            }));
        } else {
            StartCoroutine(DelayedEnemyAction(.1f, enemy));
        }
    }

    private IEnumerator DelayedAction(float delay, Action function)
    {
        yield return new WaitForSeconds(delay);
        function();
    }
//TODO agregar gameobject enemy para que afecte solo al enemigo enviado as params
// podemos cambiar MovePlayerToPosition para en vez de tomar el tag, tome el gameobject
    private IEnumerator DelayedEnemyAction(float delay, GameObject enemy)
    {
        moveLogic.HideHighlighPlayableTerrain(500, -1);
        // HERE yield return new WaitForSeconds(delay / 2);

        //step3: enemy IA action
        // TODO: cambiar esto por el mov de cada enemigo
        int enemyWalk = 2;
        TerrainObjects[] pathToPlayer = Helper.GetShorterPathToPlayer(enemy, enemyWalk, Constants.terrainObjects);
        pathToPlayer = pathToPlayer.Where(path => path != null).ToArray();

        // visible path to player
        //createHighlights.HighlightList(pathToPlayer);

        //attacks from enemy
        //EnemyAttack();

        if (pathToPlayer[enemyWalk-1] != null)
        {
            StartCoroutine(Helper.MovePlayerToPosition(pathToPlayer[enemyWalk-1].xPos, pathToPlayer[enemyWalk-1].yPos, enemy, 1.0f));
            
        }
        yield return new WaitForSeconds(delay / 2);
        //EndTurn();
    }

    private IEnumerator MoveCamera(Vector3 targetPosition)
    {
        yield return new WaitForSeconds(.2f);
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
        yield return new WaitForSeconds(.20f);

        // Realizar el resto de las acciones del turno del jugador 2
        // ...
    }

}
