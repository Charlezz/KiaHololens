using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Linq;

public class MainController : BaseController<MainMenuItem>
{
    public Canvas canvas;
    void Start()
    {

        initCursor();
        initRecognizer();
        VoiceManager.Instance.Say(VoiceManager.SELECT_MENU, 1.0f);

        keywords.Add("A R Mode", () =>
        {
            OnARModeClick();
        });

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }
	
	// Update is called once per frame
	void Update ()
	{
        TestWithMouse();

        //cameraTransform.transform.position = Camera.main.transform.position;
	}

	public void OnDiagnosticModeClick ()
	{
		Debug.Log ("OnDiagnosticModeClick");
	}

	public void OnARModeClick ()
	{
		Debug.Log ("OnARModeClick");
        SceneManager.LoadScene(SceneName.DTC_AGREEMENT);
	}

	public void OnServiceInformationClick ()
	{
		Debug.Log ("OnServiceInformationClick");
	}

    
}
