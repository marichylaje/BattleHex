using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    /*
        if(alcanceAtaque) break; attack;
        STEPS: 
        1- obtener datos del terreno del jugador
        2- obtener datos del terreno actual del enemigo
        3- comparar a cuantos hexagonos se encuentra en eje X e Y
        4- obtener el coste minimo para llegar al jugador, si todo terrenoMov costara 1 -> usar hexagono formula
        5- tener en cuenta el terrenoMov, e intentar aproximarse a las coords del jugador

        ?- como hacer un "attack range" para que pueda posicionarse en cualquier lugar valido para atacar?
            a- podemos atacar al Player si estamos dentro de un rango de X casillas, dependiendo el skill-set del Enemigo
            usando StarHighlightAttack, WalkHighlight, entre otros ubicados en CreateHighlights.cs
    */


}
