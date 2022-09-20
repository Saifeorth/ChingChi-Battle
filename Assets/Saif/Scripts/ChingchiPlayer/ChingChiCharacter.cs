using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChingChiCharacter : MonoBehaviour, ICharacter
{
    [SerializeField]
    private bool isPlayable = false;

    [SerializeField]
    private string playerName = " ";

    public string GetName()
    {
        return playerName;
    }

    [SerializeField]
    public bool IsPlayable()
    {
        return isPlayable;
    }
}
