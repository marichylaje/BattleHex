using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastManager : MonoBehaviour
{
    public List<ParticleSystem> particleSystems;
    public MoveLogic moveLogic;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HERE START PART SYSTEM");
        // Desactivar todos los Particle Systems al inicio
        foreach (ParticleSystem ps in particleSystems)
        {
            Debug.Log("HERE PART SYSTEMs");
            ps.Stop();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            Debug.Log("W TOCADA");
            ActivateParticleSystem();
        }
    }

    // Update is called once per frame
    void ActivateParticleSystem()
    {
        moveLogic.DestroyHighlighPlayableTerrain(500, -1);
        foreach(ParticleSystem ps in particleSystems){
            ps.Play();
        }
    }
}
