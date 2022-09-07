using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkStatusTextUpdate : MonoBehaviour
{
    private readonly string connectionStatusMessage = "    Connection Status: ";

    [Header("UI References")]
    public TextMeshProUGUI ConnectionStatusText;

    #region UNITY

    public void Update()
    {
        ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
    }

    #endregion
}
