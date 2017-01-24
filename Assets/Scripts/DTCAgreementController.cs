using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Linq;

public class DTCAgreementController : BaseController<DTCAgreementItem> {
    void Start () {
        initCursor();
        initRecognizer(); 
        VoiceManager.Instance.Say(VoiceManager.YOU_SELECT_AR_MODE, 1.0f);
        VoiceManager.Instance.Say(VoiceManager.SEARCH_DTC_FOR_YOUR_VEHICLE, 3.0f);
        VoiceManager.Instance.Say(VoiceManager.BEFORE_STARTING_DTC_SEARCE, 6.5f);

        keywords.Add("OK", () =>
        {
            OnConfirmClick();
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
	
	// Update is called once per frame
	void Update () {
        TestWithMouse();
	}
    
    public void OnConfirmClick()
    {
        SceneManager.LoadScene(SceneName.DTC_SEARCHING);
    }
}
