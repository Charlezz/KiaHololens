
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.VR.WSA.Input;
using UnityEngine.Windows.Speech;

public class DTCSearchingController : BaseController<DTCSearchingItem>
{
    public float waitingTime=8.0f; 
    void Start()
    {
        initCursor();
        initRecognizer();


        VoiceManager.Instance.Say(VoiceManager.NOW_SEARCHING_DTC, 0.5f);
        VoiceManager.Instance.Say(VoiceManager.PLEASE_WAIT_FOR_SECONDS, 2.5f);

        StartCoroutine(TimeController.PostDelayed(waitingTime, () => {
            SceneManager.LoadScene(SceneName.DTC_RESULT);
        }));

        keywords.Add("Cancel", () =>
        {
            OnCancelClick();
        });
        keywords.Add("Main Menu", () =>
        {
            Debug.Log("Said Main Menu");
            SceneManager.LoadScene(SceneName.MAIN);
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

    }
    void Update()
    {
        TestWithMouse();
    }
   


    public void OnCancelClick()
    {
        SceneManager.LoadScene(SceneName.MAIN);
    }

   
}

