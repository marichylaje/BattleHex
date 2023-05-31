using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHighlights : MonoBehaviour
{
    public GenerateTerrain generateTerrain;

    public void WalkHighlight(int walkDistance, List<TerrainObjects> terrainObjects)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        TerrainObjects actualTerrainUnderPlayer = Helper.GetActualPlayerBlockFromGameObject(player, false, terrainObjects);
        List<TerrainObjects> terrainToHighlight = new List<TerrainObjects>();
        
        for(int i = 1; i <= walkDistance; i++){
            terrainToHighlight = Helper.GetSurroundingTerrainsByCoords(i, actualTerrainUnderPlayer, terrainObjects);
            terrainToHighlight.RemoveAll(terrain => Helper.GetEnemiesIDTerrains(terrainObjects).Contains(terrain.id));
            generateTerrain.HighlighPlayableTerrain(terrainToHighlight.ToArray());
        }
    }

    /*public void StarHighlightAttack(int attackDistance, int startDistance, List<TerrainObjects> terrainObjects)
    {
        int actualTerrainIDUnderPlayer = Helper.GetActualPlayerBlockFromGameObject("Player", false, terrainObjects).id;

        TerrainObjects[] toHighlightTerrainTop = Helper.GetOnTopBotTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, true, terrainObjects);
        TerrainObjects[] toHighlightTerrainBottom = Helper.GetOnTopBotTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, false, terrainObjects);
        TerrainObjects[] toHighlightTerrainTopRight = Helper.GetOnTopSidesTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, false, terrainObjects);
        TerrainObjects[] toHighlightTerrainTopLeft = Helper.GetOnTopSidesTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, true, terrainObjects);
        TerrainObjects[] toHighlightTerrainBottomRight = Helper.GetOnBottomSidesTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, false, terrainObjects);
        TerrainObjects[] toHighlightTerrainBottomLeft = Helper.GetOnBottomSidesTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, true, terrainObjects);

        TerrainObjects[] toGenerate = toHighlightTerrainTop.Concat(
            toHighlightTerrainBottom.Concat(
                toHighlightTerrainTopRight.Concat(
                    toHighlightTerrainTopLeft.Concat(
                        toHighlightTerrainBottomRight.Concat(toHighlightTerrainBottomLeft).ToArray()).ToArray()).ToArray()).ToArray()).ToArray();


        generateTerrain.HighlighPlayableTerrain(toGenerate);
    }

    

    public void HighlightList(TerrainObjects[] terrainObjects)
    {
        generateTerrain.HighlighPlayableTerrain(terrainObjects);
    }

//TODO: DEVELOP THIS ATTACKS below AND MORE
    public void SingleLineHighlightAttack(int attackDistance, int startDistance, List<TerrainObjects> terrainObjects)
    {
        int actualTerrainIDUnderPlayer = Helper.GetActualPlayerBlockFromGameObject("Player", false, terrainObjects).id;

        TerrainObjects[] toHighlightTerrainTop = Helper.GetOnTopBotTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, true, terrainObjects);

        generateTerrain.HighlighPlayableTerrain(toHighlightTerrainTop);
    }

    public void TripleLineHighlightAttack(int attackDistance, int startDistance, List<TerrainObjects> terrainObjects)
    {
        int actualTerrainIDUnderPlayer = Helper.GetActualPlayerBlockFromGameObject("Player", false, terrainObjects).id;

        TerrainObjects[] toHighlightTerrainTop = Helper.GetOnTopBotTerrains(actualTerrainIDUnderPlayer, attackDistance, startDistance, true, terrainObjects);

        generateTerrain.HighlighPlayableTerrain(toHighlightTerrainTop);
    }
    */
}
