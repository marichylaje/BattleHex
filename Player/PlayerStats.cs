using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int life = 100;
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

    void updateLifeUI(int life){
        //GameObject.FindWithTag("lifeUI").GetComponent<Text>().value = life;

    }


    // Update is called once per frame
    /*public void UpdatePlayerStats()
    {
        if(Helper.GetActualPlayerBlockFromTag("Player").terrainType.effect == "fire"){
            life = getDamage(life, Helper.GetActualPlayerBlockFromTag("Player").terrainType.effectDmg);
            updateLifeUI(life);
        }
    }*/
}
