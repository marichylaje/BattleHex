using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int life = 9;
    public int movement = 3;

    bool onFire = false;
    bool onWater = false;
    bool onPoison = false;
    bool onConfused = false;
    bool onMuted = false;
    bool onParalized = false;
    bool onFreezed = false;

    bool appliedStatsPerTurn = false;

    int getDamage(int currentLife, int damage) {
        return currentLife - damage;
    }

    /*void updateLifeUI(int life){
        GameObject.FindWithTag("LifePlayerUI").GetComponent<Text>().value = life;

    }*/


    // Update is called once per frame
    /*public void UpdatePlayerStats()
    {
        if(Helper.GetActualPlayerBlockFromGameObject("Player").terrainType.effect == "fire"){
            life = getDamage(life, Helper.GetActualPlayerBlockFromGameObject("Player").terrainType.effectDmg);
            updateLifeUI(life);
        }
    }*/
}
