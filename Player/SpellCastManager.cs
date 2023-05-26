using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    DIRECTIONS:
    - top right: 1
    - top: 2
    - top left: 3
    - bot left: 4
    - bot: 5
    - bot right: 6
*/
public class SpellCastManager : MonoBehaviour
{
    public List<ParticleSystem> casting;
    public List<ParticleSystem> skillThrow;
    public List<ParticleSystem> symbols;
    public GenerateTerrain generateTerrain;

    public MoveLogic moveLogic;
    public SpellShapes spellShapes;
    public CreateHighlights createHighlights;
    public DetectClickSkills[] detectClickSkills;
    public string spellNameClicked = null;

    public bool isCasting = false;
    public bool isThrowing = false;
    private Transform throwParentTransform;
    private Transform staffEffectsTransform;
    private SpriteRenderer playerSprite;


    // Cuadrantes en grados
    private float[] cuadranteAngles = { 0, 60f, 120f, 180f, 240f, 300f };

    private void Start()
    {
        throwParentTransform = GameObject.Find("ThrowParent").transform;
        staffEffectsTransform = GameObject.Find("StaffEffects").transform;

        GameObject player = GameObject.Find("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    public void ThrowFireBall(){
        TerrainObjects terrainUnder = Helper.GetActualPlayerBlockFromTag("Player", false, Constants.terrainObjects);
        ThrowSkill(() => {
            generateTerrain.HighlighPlayableTerrain(Helper.GetStraightLine(3, DetectMouseSpace(), terrainUnder, Constants.terrainObjects));
        });
    }

    public void ThrowFireWall(){
        TerrainObjects terrainUnder = Helper.GetActualPlayerBlockFromTag("Player", false, Constants.terrainObjects);
        ThrowSkill(() => {
            generateTerrain.HighlighPlayableTerrain(Helper.GetStraightLine(3, DetectMouseSpace(), terrainUnder, Constants.terrainObjects));
        });
    }

    public void ThrowSkill(Action actionHighlightAttack){
        moveLogic.DestroyHighlighPlayableTerrain(500, -1);
        actionHighlightAttack();
        if (Input.GetMouseButtonDown(0))
        {
            ActivateParticleSystem();
            isCasting = false;
            isThrowing = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape)){
            createHighlights.WalkHighlight(moveLogic.movementDistance, Constants.terrainObjects);
            isCasting = false;
        }
    }

    private void ActivateParticleSystem()
    {
        moveLogic.DestroyHighlighPlayableTerrain(500, -1);
        foreach (ParticleSystem ps in casting)
        {
            ps.Play();
        }

        foreach (ParticleSystem ps in skillThrow)
        {
            ps.Play();
        }
        throwParentTransform.transform.rotation = Quaternion.Euler(0f, 0f, RotateParticles()); // Reemplaza "desiredAngle" con el ángulo deseado en grados
    }
    private float RotateParticles()
    {
        int direction = DetectMouseSpace();
        Vector3 newPosition;
        float newRotation;

        GameObject player = GameObject.FindGameObjectWithTag("Player");


        float xGap = player.transform.position.x;
        float yGap = player.transform.position.y;

        switch (direction)
        {
            case 1:
                newPosition = new Vector3(xGap, yGap, 0f);
                playerSprite.flipX = false;
                newRotation = 0f;
                break;
            case 2:
                newPosition = playerSprite.flipX ? new Vector3((-22.5f + xGap), (yGap), 0f) : new Vector3(xGap, yGap, 0f);
                newRotation = 68.5f;
                break;
            case 3:
                newPosition = new Vector3((-22.5f + xGap), (yGap), 0f);
                playerSprite.flipX = true;
                newRotation = 134.5f;
                break;
            case 4:
                newPosition = new Vector3((-22.5f + xGap), (yGap), 0f);
                playerSprite.flipX = true;
                newRotation = 189.5f;
                break;
            case 5:
                newPosition = playerSprite.flipX ? new Vector3((-22.5f + xGap), (yGap), 0f) : new Vector3(xGap, yGap, 0f);
                newRotation = 249f;
                break;
            default:
                newPosition = new Vector3(xGap, yGap, 0f);
                playerSprite.flipX = false;
                newRotation = 306.5f;
                break;
        }
        staffEffectsTransform.transform.position = newPosition;

        return newRotation;
    }


    private int DetectMouseSpace()
    {
        // Obtener las coordenadas del mouse y el centro de la cámara
        Vector3 mousePosition = Input.mousePosition;
        Vector3 cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));

        // Convertir las coordenadas del mouse y del centro de la cámara a coordenadas de mundo
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        cameraCenter.z = worldMousePosition.z; // Mantener la misma coordenada Z que el mouse

        // Calcular el ángulo entre el centro de la cámara y el mouse
        float angle = Mathf.Atan2(worldMousePosition.y - cameraCenter.y, worldMousePosition.x - cameraCenter.x) * Mathf.Rad2Deg;
        if (angle < 0f)
        {
            angle += 360f; // Asegurarse de que el ángulo sea positivo
        }

        // Verificar en qué cuadrante se encuentra el ángulo
        int cuadrante = 0;
        for (int i = 0; i < cuadranteAngles.Length; i++)
        {
            float startAngle = cuadranteAngles[i];
            float endAngle = cuadranteAngles[(i + 1) % cuadranteAngles.Length];

            // Corregir la condición de detección del cuadrante
            if ((angle >= startAngle && angle < endAngle) || (startAngle > endAngle && (angle >= startAngle || angle < endAngle)))
            {
                cuadrante = i + 1;
                break;
            }
        }
        return cuadrante;
    }

}
