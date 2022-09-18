using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI positionText;
    [SerializeField]
    private TextMeshProUGUI displayNameText;
    [SerializeField]
    private TextMeshProUGUI mmrText;


    public void OnValuesUpdate(string pos, string displayName, string mmr)
    {
        positionText.text = pos;
        displayNameText.text = displayName;
        mmrText.text = mmr;
    }

}
