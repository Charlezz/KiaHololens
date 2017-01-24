using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using System.Linq;

public class DTCResultController : BaseController<DTCResultItem> {

    

    // Use this for initialization
    void Start () {
        initCursor();
        initRecognizer();

        VoiceManager.Instance.Say(VoiceManager.Here_your_DTC_Result, 0.5f);
        VoiceManager.Instance.Say(VoiceManager.Please_select_the_number_that_you_want_to_see_AR_repair_procedures, 2.7f);

        keywords.Add("Select One", () =>
        {
            OnOBCClick();
        });
        keywords.Add("Select Two", () =>
        {
            OnInverterClick();
        });
        keywords.Add("Search Again", () =>
        {
            OnReSearchClick();
        });
        keywords.Add("Main Menu", () =>
        {
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

    public void OnOBCClick()
    {
        Debug.Log("OnOBCClick");
        SceneManager.LoadScene(SceneName.WARNING);
    }

    public void OnInverterClick()
    {
        SceneManager.LoadScene(SceneName.WARNING);
        Debug.Log("OnInverterClick");
    }

    public void OnEPCUClick()
    {
        Debug.Log("OnEPCUClick");
        SceneManager.LoadScene(SceneName.WARNING);
    }
    public void OnReSearchClick()
    {
        Debug.Log("re-search");
        SceneManager.LoadScene(SceneName.DTC_SEARCHING);
    }
}
