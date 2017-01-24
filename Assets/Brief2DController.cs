using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Brief2DController : BaseController<BriefItem> {


    public BriefController briefController;
	// Use this for initialization
	void Start () {
        initCursor();
        initRecognizer();
        SetFollowing(false);
	}
	
	// Update is called once per frame
	void Update () {
        TestWithMouse();
    }

    public void OnNextClick()
    {
        briefController.NextAnimation();
    }
    public void OnPrevClick()
    {
        briefController.PreviousAnimation();
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene(SceneName.WARNING);
    }
}
