using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    /*private static GameObject terrainsFolder;
    private static List<TerrainObjects> terrainObjects = new List<TerrainObjects>();

    public static void SetTerrainsFolder(GameObject folder)
    {
        terrainsFolder = folder;
    }*/

    public static GridData[] GenerateGridPositions(int terrainHeight)
    {
        GridData[] gridPositions;
        float offsetX = Constants.hexWidth * Constants.spriteTXSize + Constants.gapX;
        float offsetY = Constants.hexHeight * Constants.spriteTYSize + Constants.gapY;
        int id = 0;
        int terrainWidth = Constants.terrainWidth;

        gridPositions = new GridData[Constants.terrainWidth * Constants.terrainHeight];

        for (int y = 0; y < Constants.terrainHeight; y++)
        {
            for (int x = 0; x < Constants.terrainWidth; x++)
            {
                GridData position;
                if (x % 2 != 0)
                    position = new GridData(x * offsetX, y * offsetY + (offsetY / 2), id);
                else
                    position = new GridData(x * offsetX, y * offsetY, id);

                id++;
                gridPositions[x + y * Constants.terrainWidth] = position;
            }
        }
        return gridPositions;
    }

    public static TerrainType FindSpecificTerrainTypeByName(string name)
    {
        foreach (TerrainType terrainType in Constants.terrainTypes)
        {
            string terrainName = terrainType.name;
            if (terrainName == name)
            {
                return terrainType;
            }
        }

        return null;
    }

    public static TerrainObjects FindSpecificTerrainObjectByID(int id, List<TerrainObjects> terrainObjects)
    {
        foreach (TerrainObjects terrainObject in terrainObjects)
        {
            int terrainId = terrainObject.id;
            if (terrainId == id)
            {
                return terrainObject;
            }
        }

        return null;
    }

    // Función que devuelve un valor random entre 0 y el total acumulado del key "probability" del array de objetos existente "terrainTypes" de tipo TerrainType
    public static float GetRandomProbability(TerrainType[] terrainTypes)
    {
        float totalProbability = 0f;
        foreach (TerrainType terrainType in terrainTypes)
        {
            totalProbability += terrainType.probability;
        }

        return Random.Range(0f, totalProbability);
    }

    // Función que consume la anterior, para que compare el numero ingresado "totalProb" con cada "terrainType.probability" en "terrainTypes". De ser mayor a la actual comparación, restar ese numero a nuestra "totalProb", e intentar con el siguiente item en el array
    public static TerrainType SelectTerrainType(float totalProb)
    {
        foreach (TerrainType terrainType in Constants.terrainTypes)
        {
            float terrainProbability = terrainType.probability;
            if (totalProb > terrainProbability)
            {
                totalProb -= terrainProbability;
            }
            else
            {
                return terrainType;
            }
        }
        return null;
    }

    
    public static IEnumerator MovePlayerToPosition(float xPos, float yPos, GameObject player, float movementDuration)
    {
        Vector3 startPosition = player.transform.position;
        Vector3 targetPosition = new Vector3(xPos, yPos + Helper.GetPlayerDesface(player.tag), 0);

        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            // Calcula el progreso del movimiento
            float t = elapsedTime / movementDuration;

            // Aplica una curva de interpolación suave para el movimiento
            t = Mathf.SmoothStep(0f, 1f, t);

            // Calcula la posición actual del jugador mediante la interpolación suave
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Actualiza la posición del jugador
            player.transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de que el jugador esté exactamente en la posición objetivo
        player.transform.position = targetPosition;
    }


    private static float GetPlayerDesface(string tag)
    {
        if (tag == "Player")
        {
            return Constants.playerDesface;
        }
        else if (tag == "Enemy")
        {
            return Constants.enemyDesface;
        }
        else
        {
            return 0f; // Ajusta esto según tus necesidades
        }
    }

    public static TerrainObjects GetTerrainUnderMouse(List<TerrainObjects> terrainObjects){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xPos = mousePosition.x / 15f; // Dividir por el ancho de cada hexágono
        float yPos = mousePosition.y / 15f; // Dividir por la altura de cada hexágono
        int roundedX = Mathf.RoundToInt(xPos);
        int roundedY = Mathf.RoundToInt(yPos);
        float worldX = roundedX * 15f; // Multiplicar por el ancho de cada hexágono
        float worldY = roundedY * 15f; // Multiplicar por la altura de cada hexágono
        TerrainObjects nearestObject = null;
        float nearestDistance = Mathf.Infinity;

        foreach (TerrainObjects obj in terrainObjects) // Reemplaza "yourObjectList" con la lista que contiene los GameObjects de la cuadrícula hexagonal
        {
            float distance = Vector2.Distance(new Vector2(worldX, worldY), obj.transform.position);

            if (distance < nearestDistance)
            {
                nearestObject = obj;
                nearestDistance = distance;
            }
        }

        return nearestObject;
    }


    /*public static void MovePlayerToIDTerrain(string tag, int id, bool enemy, List<TerrainObjects> terrainObjects){
        TerrainObjects terrain = terrainObjects[id];
        MovePlayerToPosition(terrain.xPos, terrain.yPos, tag, enemy);
    }*/

    // funcion generalmente usada para detectar en que casilla se encuentra el jugador o enemigos, si le pasamos su tag
    public static TerrainObjects GetActualPlayerBlockFromGameObject(GameObject character, bool isEnemy, List<TerrainObjects> terrainObjects)
    {
        if (character == null)
        {
            Debug.LogError("Could not find game object with tag: " + character.tag);
            return null;
        }

        float yDesface = isEnemy ? Constants.enemyDesface : Constants.playerDesface;
        Vector2 actualPosition = new Vector2(character.transform.position.x, character.transform.position.y - yDesface);

        foreach (TerrainObjects terrainObject in terrainObjects)
        {
            if (Mathf.Approximately(terrainObject.xPos, actualPosition.x) && Mathf.Approximately(terrainObject.yPos, actualPosition.y))
            {
                return terrainObject;
            }
        }

        Debug.LogError("Could not find terrain object at position: " + actualPosition + "from tag: " + character.tag);
        return null;
    }


    public static TerrainObjects GetTerrainDataFromCoords(float xPos, float yPos, bool onlyHighlighted, List<TerrainObjects> terrainObjects)
    {
        TerrainObjects response;
        foreach (TerrainObjects terrainObject in terrainObjects)
        {
            if (terrainObject.id > 500 && onlyHighlighted)
            {
                if (terrainObject.xPos == xPos && terrainObject.yPos == yPos)
                {
                    return terrainObject;
                }
            }
            if (!onlyHighlighted)
            {
                if ((Mathf.Approximately(terrainObject.xPos, xPos)) && terrainObject.yPos == yPos)
                {
                    return terrainObject;
                }
            }
        }
        //para no moverse, pero deberia de devolver null
        return null; 
    }

    public static TerrainObjects[] GetStraightLine(int distance, int direction, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects)
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

    public static List<Vector2> GetSurroundingCoords(int distance, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects) {        
        float yMove = Constants.spriteTYSize;
        float xMove = Constants.spriteTXSize;
        float yOffset = Constants.spriteTYSizePenalization;
        float x = terrainUnder.xPos;
        float y = terrainUnder.yPos;
        int d = distance;
        int id = terrainUnder.id;
        
        int countFlagOne = 0;

        List<TerrainObjects> terrains = new List<TerrainObjects>();
        List<Vector2> coords = new List<Vector2>();

        // agarra los ejes principales entre nosotros y Distance
        coords.Add(new Vector2(x, Mathf.Round((y + (d * yMove)) * 100f) / 100f)); // top
        coords.Add(new Vector2(x, Mathf.Round((y - (d * yMove)) * 100f) / 100f)); // bot
        coords.Add(new Vector2(x - (d * xMove), Mathf.Round((coords[0].y - ((d * yMove) / 2) + (id % 2 == 0 ? ((d % 2 == 0) ? 0 : yOffset) : ((d % 2 == 0) ? 0 : -yOffset))) * 100f) / 100f)); // topL
        coords.Add(new Vector2(x - (d * xMove), Mathf.Round((coords[1].y + ((d * yMove) / 2) + (id % 2 == 0 ? ((d % 2 == 0) ? 0 : yOffset) : ((d % 2 == 0) ? 0 : -yOffset))) * 100f) / 100f)); // botL
        coords.Add(new Vector2(x + (d * xMove), Mathf.Round((coords[0].y - ((d * yMove) / 2) + (id % 2 == 0 ? ((d % 2 == 0) ? 0 : yOffset) : ((d % 2 == 0) ? 0 : -yOffset))) * 100f) / 100f)); // topR
        coords.Add(new Vector2(x + (d * xMove), Mathf.Round((coords[1].y + ((d * yMove) / 2) + (id % 2 == 0 ? ((d % 2 == 0) ? 0 : yOffset) : ((d % 2 == 0) ? 0 : -yOffset))) * 100f) / 100f)); // botR

        // llena los espacios en blanco entre cada eje principal, creando una linea
        if(d > 1){
            for(int i = 1; i < d; i++){
                coords.Add(new Vector2((coords[0].x - (xMove * i)), Mathf.Round(((coords[0].y - ((yMove / 2) * i)) + (id % 2 == 0 ? ((i % 2 != 0) ? yOffset : 0) : ((i % 2 != 0) ? -yOffset : 0))) * 100f) / 100f)); // topL
                coords.Add(new Vector2((coords[0].x + (xMove * i)), Mathf.Round(((coords[0].y - ((yMove / 2) * i)) + (id % 2 == 0 ? ((i % 2 != 0) ? yOffset : 0) : ((i % 2 != 0) ? -yOffset : 0))) * 100f) / 100f)); // topR
                
                coords.Add(new Vector2(coords[2].x, Mathf.Round((coords[2].y - (yMove * i)) * 100f) / 100f)); // left
                coords.Add(new Vector2(coords[4].x, Mathf.Round((coords[4].y - (yMove * i)) * 100f) / 100f)); // right

                coords.Add(new Vector2((coords[1].x - (xMove * i)), Mathf.Round((coords[1].y + ((yMove / 2) * i) + (id % 2 == 0 ? ((i % 2 != 0) ? yOffset : 0) : ((i % 2 != 0) ? -yOffset : 0))) * 100f) / 100f)); // botL
                coords.Add(new Vector2((coords[1].x + (xMove * i)), Mathf.Round((coords[1].y + ((yMove / 2) * i) + (id % 2 == 0 ? ((i % 2 != 0) ? yOffset : 0) : ((i % 2 != 0) ? -yOffset : 0))) * 100f) / 100f)); // botR
            }
        }

        return coords;
    }

    public static List<TerrainObjects> GetSurroundingTerrainsByCoords(int d, TerrainObjects terrainUnder, List<TerrainObjects> terrainObjects) {        
        List<Vector2> coords = Helper.GetSurroundingCoords(d, terrainUnder, terrainObjects);
        List<TerrainObjects> terrains = new List<TerrainObjects>();

        for (int i = 0; i < coords.Count; i++) {
            TerrainObjects terrainObject = Helper.GetTerrainDataFromCoords(coords[i].x, coords[i].y, false, terrainObjects);
            if (terrainObject != null && !terrains.Contains(terrainObject)) {
                terrains.Add(terrainObject);
            }
        }
        return terrains;
    }

    /*
        DIRECTIONS:
        - top right: 1
        - top: 2
        - top left: 3
        - bot left: 4
        - bot: 5
        - bot right: 6
    */

    /*public static List<TerrainObjects> GetSurroundingTerrainsByTag(string tagName, bool enemy, List<TerrainObjects> terrainObjects){
        List<TerrainObjects> surroundingTerrains = new List<TerrainObjects>();
        TerrainObjects terrainUnderEnemy = Helper.GetActualPlayerBlockFromGameObject(tagName, enemy, terrainObjects);
        float xPos = terrainUnderEnemy.xPos;
        float yPos = terrainUnderEnemy.yPos;

        if(terrainObjects != null){
            TerrainObjects response = Helper.GetTerrainDataFromCoords(xPos, yPos, false, terrainObjects);
            surroundingTerrains = Helper.GetSurroundingTerrainsByCoords(response.xPos, response.yPos, 1, response.id, terrainObjects);
        }
        return surroundingTerrains;
    }*/

//TODO: cambiar enemyTag de String a ENUM
    public static int GetMinDistanceTillEnemy(GameObject enemy, int attackDistance, List<TerrainObjects> terrainObjects){
        // 1- obtener datos del terreno del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TerrainObjects playerTerrain = Helper.GetActualPlayerBlockFromGameObject(player, false, terrainObjects);
        float playerTXPos = playerTerrain.xPos;
        float playerTYPos = playerTerrain.yPos;
        int ajuste = 0;

        // 2- obtener datos del terreno actual del enemigo
        TerrainObjects enemyTerrain = Helper.GetActualPlayerBlockFromGameObject(enemy, true, terrainObjects);
        float enemyTXPos = enemyTerrain.xPos;
        float enemyTYPos = enemyTerrain.yPos;

        int minDistanceTillEnemy = -1;
        for(int i = 1; i < 100; i++){
            List<TerrainObjects> terrainsRatiusTillEnemy = Helper.GetSurroundingTerrainsByCoords(i, playerTerrain, terrainObjects);
            foreach(TerrainObjects terrain in terrainsRatiusTillEnemy){

                if(terrain.xPos == enemyTXPos && terrain.yPos == enemyTYPos){
                    minDistanceTillEnemy = i;
                    break;
                }
            }
            
            if(minDistanceTillEnemy > 0){
                Debug.Log("MAX DISTANCE TILL ENEMY: " + minDistanceTillEnemy);
                break;
            }
        }

        return minDistanceTillEnemy;
    }

    public static int[] GetEnemiesIDTerrains(List<TerrainObjects> terrainObjects){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int[] enemyTerrain = new int[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject innerEnemy = enemies[i];
            enemyTerrain[i] = Helper.GetActualPlayerBlockFromGameObject(innerEnemy, true, terrainObjects).id;
            Debug.Log("HERE TERRAIN ID ENEMY: " + enemyTerrain[i]);
        }
        return enemyTerrain;
    }

//TODO: despues ejecutar al reves, y hacer el camino mas corto
//      priorizar cuando se avanza en diagonal que cuando se avanza en linea recta, cuando en diagonal ambos disminuyen
//      eliminar la impresion de highlights para enemigo
    public static TerrainObjects[] GetShorterPathToPlayer(GameObject enemy, int attackDistance, List<TerrainObjects> terrainObjects){
        /*
            1- obtener datos del jugador y enemigo a mover
            2- GetSurrounderTerrain del enemigo, d = 1
            3- if ayuda a disminuir la diferencia entre enemigo o jugador en X o Y, priorizar camino
            4- loop hasta llegar o que el enemigo llegue a su maxima movilidad
            5- considerar espacio para calc distancia de skills
            +
        */
        // 1- obtener datos del terreno del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TerrainObjects playerTerrain = Helper.GetActualPlayerBlockFromGameObject(player, false, terrainObjects);
        float playerTXPos = playerTerrain.xPos;
        float playerTYPos = playerTerrain.yPos;
        int ajuste = 0;

        // 2- obtener datos del terreno actual del enemigo
        TerrainObjects enemyTerrain = Helper.GetActualPlayerBlockFromGameObject(enemy, true, terrainObjects);
        float enemyTXPos = enemyTerrain.xPos;
        float enemyTYPos = enemyTerrain.yPos;

        /*GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int[] otherEnemyTerrain = new int[enemies.Length - 1];
        for (int i = 0; i >= enemies.Length; i++)
        {
            GameObject innerEnemy = enemies[i];
            if (innerEnemy != enemy){
                otherEnemyTerrain[i] = Helper.GetActualPlayerBlockFromGameObject(innerEnemy, true, terrainObjects).id;
                Debug.Log("HERE TERRAIN ID ENEMY: " + otherEnemyTerrain[i]);
            }
        }*/

        // 3- comparar a cuantos hexagonos se encuentra en eje X e Y
        float xDistanceBetween(TerrainObjects terrainObj){
            return Mathf.Round(ajuste + (terrainObj.xPos / Constants.spriteTXSize) - (playerTXPos / Constants.spriteTXSize));
        }
        float yDistanceBetween(TerrainObjects terrainObj){
            return Mathf.Round(ajuste + ((terrainObj.yPos - 
            (enemyTerrain.id % 2 != 0 ? Constants.spriteTYSizePenalization : 0)) / Constants.spriteTYSize) - 
            ((playerTYPos - (playerTerrain.id % 2 != 0 ? Constants.spriteTYSizePenalization : 0)) / Constants.spriteTYSize));
        }

        TerrainObjects[] acumTerrains = new TerrainObjects[20];
        bool createPathFlag = false;
        int loopCounter = -1;
        float pathCounter = 0;

        // entran 6 terrenos que rodean a otro terreno/enemy
        void CreatePath(List<TerrainObjects> surrTerrains){
            if(createPathFlag){
                Debug.Log("------------------------- EXIT createPathFlag EXIT: " + createPathFlag + ", loopCount: " + loopCounter);
                return;
            }
            // lleva contador de cuantas iteraciones, para poder modificar Index de la respuesta
            loopCounter++;
            // acomodamos la data priorizando a quienes están mas cerca del Player
            surrTerrains.Sort((a, b) =>
            {
                float xDistanceA = xDistanceBetween(a);
                float xDistanceB = xDistanceBetween(b);
                float yDistanceA = yDistanceBetween(a);
                float yDistanceB = yDistanceBetween(b);

                float distanceA = Mathf.Abs(xDistanceA) + Mathf.Abs(yDistanceA) + a.terrainType.movilityCost - a.terrainType.bonusCost;
                float distanceB = Mathf.Abs(xDistanceB) + Mathf.Abs(yDistanceB) + b.terrainType.movilityCost - a.terrainType.bonusCost;

                return distanceA.CompareTo(distanceB);
            });
            /*Debug.Log("////////////////////////////////////////////////////////");

            Debug.Log("loopCounter :" + loopCounter);
            foreach(TerrainObjects surrTerrain in surrTerrains){
                Debug.Log("xDistanceBetween :" + xDistanceBetween(surrTerrain) + ", yDistanceBetween :" + yDistanceBetween(surrTerrain) + ", ID :" + surrTerrain.id);
            }*/

            foreach(TerrainObjects surrTerrain in surrTerrains){
                if(createPathFlag){
                    break;
                }
                bool alreadyExists = false;
                for (int i = 0; i < loopCounter; i++)
                {
                    if (acumTerrains[i] == surrTerrain)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                // Si el surrTerrain ya existe en acumTerrains, continuar con la siguiente iteración
                if (alreadyExists || Helper.GetEnemiesIDTerrains(terrainObjects).Contains(surrTerrain.id))
                {
                    continue;
                }


                // si encuentra un terreno que tenga diferencia 0 con X e Y, hemos llegado a Player
                if(xDistanceBetween(surrTerrain) == 0){
                    if(yDistanceBetween(surrTerrain) == 0){
                        createPathFlag = true;
                        break;
                    }
                }

                if((loopCounter < acumTerrains.Length) && surrTerrain != null){
                    acumTerrains[loopCounter] = surrTerrain;
                } else {
                    Debug.Log("BREAK FORCED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
                }
                CreatePath(Helper.GetSurroundingTerrainsByCoords(1, surrTerrain, terrainObjects));
            }
        }
        //empieza llamandolo desde la posicion inicial del enemigo
        CreatePath(Helper.GetSurroundingTerrainsByCoords(1, enemyTerrain, terrainObjects));

        return acumTerrains;
    }


    public static TerrainObjects[] GetOnTopBotTerrains(int centralID, int maxRow, int initRow, bool isTop, List<TerrainObjects> terrainObjects)
    {
        List<TerrainObjects> terrains = new List<TerrainObjects>();
        int valorFijo = Constants.terrainWidth;

        for (int i = 1; i <= maxRow; i++)
        {
            foreach (TerrainObjects terrainObject in terrainObjects)
            {
                if(isTop){
                    if (terrainObject.id == centralID + valorFijo * i && initRow <= i)
                    {
                        terrains.Add(terrainObject);
                    }
                } else {
                    if (terrainObject.id == centralID - valorFijo * i && initRow <= i)
                    {
                        terrains.Add(terrainObject);
                    }
                }
            }
        }

        return terrains.ToArray();
    }

    public static TerrainObjects[] GetOnTopSidesTerrains(int centralID, int maxRow, int initRow, bool isLeft, List<TerrainObjects> terrainObjects)
    {
        List<TerrainObjects> terrains = new List<TerrainObjects>();
        int valorFijo = Constants.terrainWidth;
        int contadorA = 1;
        int contadorB = 1;
        bool isHittedLimit = false;

        for (int i = 0; i <= maxRow - 1; i++)
        {
            if(isHittedLimit){
                 break;
            }
            foreach (TerrainObjects terrainObject in terrainObjects)
            {
                if(isHittedLimit){
                    break;
                }
                if(centralID% 2 == 0){
                    if(isLeft){
                        if (i == 0 && terrainObject.id == centralID + 1 && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID + 1 + ((valorFijo + 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos > 280){
                                isHittedLimit = true;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + ((valorFijo + 2) * contadorB) && initRow <= i + 1)
                        {
                            contadorB++;
                            terrains.Add(terrainObject);
                            
                            break;
                        }
                    } else {
                        if (i == 0 && terrainObject.id == centralID - 1 && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID + ((valorFijo - 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos < 1){
                                isHittedLimit = true;
                                break;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + ((valorFijo - 2) * contadorB) && initRow <= i + 1)
                        {
                            if(terrainObject.xPos < 280){
                                contadorB++;
                                terrains.Add(terrainObject);
                                break;
                            }
                            break;
                        }
                    }
                } else {
                    if(isLeft){
                        if (i == 0 && terrainObject.id == centralID + 1 + valorFijo && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID + 1 + valorFijo + ((valorFijo + 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos > 280){
                                isHittedLimit = true;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + ((valorFijo + 2) * contadorB) && initRow <= i + 1)
                        {
                            contadorB++;
                            terrains.Add(terrainObject);

                            
                            break;
                        }
                    } else {
                        if (i == 0 && terrainObject.id == centralID - 1 + valorFijo && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID + ((valorFijo - 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos < 1){
                                isHittedLimit = true;
                                break;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + valorFijo - 2 + ((valorFijo - 2) * contadorB) && initRow <= i + 1)
                        {
                            if(terrainObject.xPos < 270){
                                contadorB++;
                                terrains.Add(terrainObject);
                                break;
                            }
                            break;
                        }
                    }
                }

            }
        }

        return terrains.ToArray();
    }

    public static TerrainObjects[] GetOnBottomSidesTerrains(int centralID, int maxRow, int initRow, bool isLeft, List<TerrainObjects> terrainObjects)
    {
        List<TerrainObjects> terrains = new List<TerrainObjects>();
        int valorFijo = Constants.terrainWidth;
        int contadorA = 1;
        int contadorB = 1;
        bool isHittedLimit = false;

        for (int i = 0; i <= maxRow - 1; i++)
        {
            if(isHittedLimit){
                 break;
            }
            foreach (TerrainObjects terrainObject in terrainObjects)
            {
                if(isHittedLimit){
                    break;
                }
                if(centralID% 2 == 0){
                    if(!isLeft){
                        if (i == 0 && terrainObject.id == centralID + 1 - valorFijo && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID - valorFijo + 1 + ((-valorFijo + 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);
                            if(terrainObject.xPos > 280){
                                isHittedLimit = true;
                            }
                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + ((-valorFijo + 2) * contadorB) && initRow <= i + 1)
                        {
                            if(terrainObject.xPos < 1){
                                break;
                            }
                            contadorB++;
                            terrains.Add(terrainObject);
                            break;
                        }
                    } else {
                        if (i == 0 && terrainObject.id == centralID - 1 - valorFijo && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID - valorFijo - 1 + ((-valorFijo - 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);
                            if(terrainObject.xPos < 1){
                                isHittedLimit = true;
                                break;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + ((-valorFijo - 2) * contadorB) && initRow <= i + 1)
                        {
                            if(terrainObject.xPos < 280){
                                contadorB++;
                                terrains.Add(terrainObject);
                                break;
                            }
                            break;
                        }
                    }
                } else {
                    if(isLeft){
                        if (i == 0 && terrainObject.id == centralID - 1 && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID - (1) - ((valorFijo + 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos > 280){
                                isHittedLimit = true;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID - ((valorFijo + 2) * contadorB) && initRow <= i + 1)
                        {
                            contadorB++;
                            terrains.Add(terrainObject);

                            
                            break;
                        }
                    } else {
                        if (i == 0 && terrainObject.id == centralID + 1 && initRow <= i + 1)
                        {
                            terrains.Add(terrainObject);
                            break;
                        } else if (i % 2 == 0 && i != 0 && terrainObject.id == centralID - ((valorFijo - 2) * contadorA) && initRow <= i + 1)
                        {
                            contadorA++;
                            terrains.Add(terrainObject);

                            if(terrainObject.xPos < 1){
                                isHittedLimit = true;
                                break;
                            }

                            break;
                        } else if (i % 2 != 0 && i != 0 && terrainObject.id == centralID + valorFijo - 1 - ((valorFijo - 2) * contadorB) && initRow <= i + 1)
                        {
                            if(terrainObject.xPos < 270){
                                contadorB++;
                                terrains.Add(terrainObject);
                                break;
                            }
                            break;
                        }
                    }
                }

            }
        }

        return terrains.ToArray();
    }

    public static TerrainObjects[] GetSecondLevelTerrainUnreachables(int centralID, List<TerrainObjects> terrainObjects)
    {
        List<TerrainObjects> terrains = new List<TerrainObjects>();
        int valorFijo = Constants.terrainWidth;

        foreach (TerrainObjects terrainObject in terrainObjects)
        {
            if (
                terrainObject.id == centralID - valorFijo - (centralID% 2 != 0 ? 0 : valorFijo) + 1 || 
                terrainObject.id == centralID - valorFijo - (centralID% 2 != 0 ? 0 : valorFijo) - 1 || 
                terrainObject.id == centralID + 2 ||
                terrainObject.id == centralID - 2 ||
                terrainObject.id == centralID + valorFijo + (centralID% 2 == 0 ? 0 : valorFijo) + 1 ||
                terrainObject.id == centralID + valorFijo + (centralID% 2 == 0 ? 0 : valorFijo) - 1
            )
                {
                    terrains.Add(terrainObject);        
                }

        }

        return terrains.ToArray();
    }

    public static TerrainObjects[] GetThirdLevelTerrainUnreachables(int centralID, List<TerrainObjects> terrainObjects)
    {
        List<TerrainObjects> terrains = new List<TerrainObjects>();
        int valorFijo = Constants.terrainWidth;

        foreach (TerrainObjects terrainObject in terrainObjects)
        {
            if (
                /*41_61*/terrainObject.id == centralID + valorFijo + valorFijo + (centralID% 2 == 0 ? 0 : valorFijo) + 1 || 
                /*39_59*/terrainObject.id == centralID + valorFijo + valorFijo + (centralID% 2 == 0 ? 0 : valorFijo) - 1 || 
                /*+3*/terrainObject.id == centralID + 3 ||
                /*-3*/terrainObject.id == centralID - 3 ||
                /*42*/terrainObject.id == centralID + valorFijo + valorFijo + 2 ||
                /*38*/terrainObject.id == centralID + valorFijo + valorFijo - 2 ||
                /*-39_-59*/terrainObject.id == centralID - valorFijo - valorFijo - (centralID% 2 != 0 ? 0 : valorFijo) + 1 || 
                /*-41_-61*/terrainObject.id == centralID - valorFijo - valorFijo - (centralID% 2 != 0 ? 0 : valorFijo) - 1 || 
                /*23_-17*/terrainObject.id == centralID + (centralID% 2 != 0 ? valorFijo : -valorFijo) + 3 ||
                /*17_-23*/terrainObject.id == centralID + (centralID% 2 != 0 ? valorFijo : -valorFijo) - 3 ||
                /*-38*/terrainObject.id == centralID - valorFijo - valorFijo + 2 ||
                /*-42*/terrainObject.id == centralID - valorFijo - valorFijo - 2
            )
                {
                    terrains.Add(terrainObject);        
                }

        }

        return terrains.ToArray();
    }

}
