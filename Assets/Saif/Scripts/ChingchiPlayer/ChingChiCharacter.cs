using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingChiCharacter : MonoBehaviour, ICharacter
{
    [SerializeField]
    private bool isPlayable = false;

    [SerializeField]
    private string playerName = " ";

    public bool isGamePlaying;


    public bool isActive = false;



    public Renderer[] renderers;
    public GameObject deathExplosionVfx;

    public string GetName()
    {
        return playerName;
    }


    public bool IsPlayable()
    {
        return isPlayable;
    }

    public void SetActivation(bool activationState)
    {
        isActive = activationState;
    }


    public virtual void Spawn()
    { 

    }


    public virtual void Die()
    {

    }



}
