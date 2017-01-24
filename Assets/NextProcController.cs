using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Linq;

public class NextProcController : BaseController<BasicMenuItem> {

	// Use this for initialization
	void Start () {
        initCursor();
        initRecognizer();

        VoiceManager.Instance.Say(VoiceManager.the_removal_procedure_finished, 0.5f);
        VoiceManager.Instance.Say(VoiceManager.select_the_next_step_and_say_the_number, 3.0f);

        keywords.Add("Select One", () =>
        {
            OnStartInstallationClick();
        });
        keywords.Add("Select Two", () =>
        {
            OnGoToMainClick();
        });
        keywords.Add("Back", () =>
        {
            OnBackClick();
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

    public void OnStartInstallationClick()
    {
        Debug.Log("Start_Installation");
    }
    public void OnGoToMainClick()
    {
        SceneManager.LoadScene(SceneName.MAIN);
    }

    public void OnBackClick()
    {
        SceneManager.LoadScene(SceneName.BRIEF);
    }
}
