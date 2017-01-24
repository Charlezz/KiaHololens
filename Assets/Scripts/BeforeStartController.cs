using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

public class BeforeStartController : BaseController<WarningMenuItem>{

	// Use this for initialization
	void Start () {

        initCursor();
        initRecognizer();

        VoiceManager.Instance.Say(VoiceManager.Before_start_drag_the_3D_model_onto_the_High_Voltage_Joint_Box, 0.5f);
        VoiceManager.Instance.Say(VoiceManager.If_done_say_OK, 6.0f);

        keywords.Add("OK", () =>
        {
            OnOkayClick();
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
        SceneManager.LoadScene("Brief");
        //gameObject.SetActive(false);
        VoiceManager.Instance.CancelAll();
    }
}
