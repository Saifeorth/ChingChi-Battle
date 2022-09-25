using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameAnnouncer : MonoBehaviour
{
    public static event Action OnTimerAnnounced;

    [SerializeField]
    private TextMeshProUGUI InGameAnnouncerText;


    [SerializeField]
    private Animator textAnnouncerAnim;
    
    [SerializeField]
    public StartAnnouncer[] startAnnouncers;


    [SerializeField]
    public AudioSource announcerAudioSource;


    private void Awake()
    {
        textAnnouncerAnim = InGameAnnouncerText.GetComponent<Animator>();
        announcerAudioSource = GetComponent<AudioSource>();
    }


    private void OnEnable()
    {
        Spawner.OnPlayeSpawned += OnPlayerSpawned;
    }


    private void OnDisable()
    {
        Spawner.OnPlayeSpawned -= OnPlayerSpawned;
    }

    private void OnPlayerSpawned(ChingChiCharacter player)
    {
        AnnounceTimer();
    }


    private IEnumerator ShowAnnouncerText()
    {
        for (int i = 0; i < startAnnouncers.Length; i++)
        {
            InGameAnnouncerText.text = startAnnouncers[i].startAnnouncerText;
            textAnnouncerAnim.SetTrigger("show");
            announcerAudioSource.PlayOneShot(startAnnouncers[i].startAnnouncerSound);
             yield return new WaitForSeconds(startAnnouncers[i].startAnnouncerSound.length);
        }

        OnTimerAnnounced?.Invoke();
    }


    public void AnnounceTimer()
    {
        StartCoroutine(ShowAnnouncerText());
    }


    [System.Serializable]
    public struct StartAnnouncer
    {
        public AudioClip startAnnouncerSound;
        public String startAnnouncerText;
        //public float announcementDelay;
    }

}
