using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class WarningController : BaseController<WarningMenuItem> {

    // Use this for initialization
   

    void Start () {
        initCursor();
        initRecognizer();

        VoiceManager.Instance.Say(VoiceManager.Be_sure_to_shut_off_the_high_voltage_system_before_doing_any_work, 0.5f);
        VoiceManager.Instance.Say(VoiceManager.Failure_to_follow_the_safety_instructions_may_result_in_serious_electrical_injuries, 5.0f);


        
        keywords.Add("OK", () =>
        {
            OnOkayClick();
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
    public void OnOkayClick()
    {
        SceneManager.LoadScene(SceneName.BRIEF);
    }
   
}
