using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePanel : MonoBehaviour
{
 
    private Coroutine defaultCoroutine;


    public void CloseAndOpenPanel(GameObject closePanel, GameObject openPanel, bool shouldPause = false)
    {
        if (defaultCoroutine != null)
        {
            StopCoroutine(defaultCoroutine); 
        }
        defaultCoroutine = StartCoroutine(CloseOpenPanelCoroutine(closePanel, openPanel, shouldPause));
       
    }


    public IEnumerator CloseOpenPanelCoroutine(GameObject panelToClose, GameObject panelToOpen, bool shouldPause = false)
    {

         if(!shouldPause)
        {
            Time.timeScale = 1.0f;
        }

        Animator anim;
        float duration = 0.0f;

        if (panelToClose != null)
        {
            anim = panelToClose.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("show");
                duration = anim.GetCurrentAnimatorStateInfo(0).length/2;
              //  AudioController.instance.PlayAudio(AudioType.SFX_Panel_SlideOut, true, 0.0f, 0.5f);
            }

 
        }

        yield return new WaitForSeconds(duration);


        duration = 0.0f;

        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }


        if (panelToOpen != null)
        {
            if (!panelToOpen.activeSelf)
            {
                panelToOpen.SetActive(true);
            }

            if (shouldPause)
            {
                Time.timeScale = 0.0f;
            }
          

            anim = panelToOpen.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("show");
                //anim.speed = 1.0f;
                duration = anim.GetCurrentAnimatorStateInfo(0).length;
            }
        }

       
        yield return new WaitForSeconds(duration);
        defaultCoroutine = null;

     


    }




}
