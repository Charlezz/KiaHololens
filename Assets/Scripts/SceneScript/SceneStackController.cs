using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneStackController : Singleton<SceneStackController>
{

	protected SceneStackController ()
	{
//		sceneStack.Push (new SceneData (SceneName.MAIN, SceneManager.GetActiveScene ().name));
	}


	private Stack<SceneData> sceneStack = new Stack<SceneData> ();

	public int GetCount ()
	{
		return sceneStack.Count;
	}

	///<summary>
	///Return last scene name for scene stack tracing
	///</summary>
	public string GetLastSceneName ()
	{

		if (GetCount () == 0) {
			return null;
		}
		SceneData sceneData = sceneStack.Peek ();
		if (sceneData != null) {
			return sceneData.CurrentSceneName;
		} else {
			return null;
		}
	}

	///<summary>
	///Return scene data instance of top
	///</summary>
	public SceneData GetCurrentSceneData ()
	{
		return sceneStack.Peek ();
	}

	///<summary>
	///Clear scene stack and load new scene
	///</summary>
	public void LoadScene (string sceneName)
	{
		Clear ();
		PushPreviousAndLoadCurrent (new SceneData (null, sceneName));

	}

	public void PushPreviousAndLoadCurrent (SceneData currentSceneData)
	{
		if (currentSceneData.LoadingMode == LoadSceneMode.Additive) {
			currentSceneData.Controller.pauseSceneObjects (0.2f);
		}
		sceneStack.Push (currentSceneData);
		SceneManager.LoadScene (currentSceneData.CurrentSceneName, currentSceneData.LoadingMode);

	}

	public void PushPreviousAndLoadCurrentFix (SceneData currentSceneData)
	{
		sceneStack.Push (currentSceneData);
		if (currentSceneData.Controller != null) {
			currentSceneData.Controller.pauseSceneEventSystem ();
		}

		SceneManager.LoadScene (currentSceneData.CurrentSceneName, currentSceneData.LoadingMode);

	}

	public void PushPreviousAndLoadCurrentAndTime (SceneData currentSceneData, float time)
	{
		if (currentSceneData.LoadingMode == LoadSceneMode.Additive) {
			currentSceneData.Controller.pauseSceneObjects (time);
		}
		sceneStack.Push (currentSceneData);
		SceneManager.LoadScene (currentSceneData.CurrentSceneName, currentSceneData.LoadingMode);
	}

	public void PausePrevious ()
	{
		if (sceneStack.Count == 0) {
			return;
		}

		//SceneManagerController sceneManagerController = (SceneManagerController)sceneStack.Peek();
		//sceneManagerController.pauseSceneObjects();
		SceneManagerController sceneManagerController = sceneStack.Peek ().Controller;
		if (sceneManagerController) {
			sceneManagerController.pauseSceneObjectsAll ();
		}

	}

	public void PausePreviousAndTime (float time)
	{
		if (sceneStack.Count == 0) {
			return;
		}

		//SceneManagerController sceneManagerController = (SceneManagerController)sceneStack.Peek();
		//sceneManagerController.pauseSceneObjects();
		SceneManagerController sceneManagerController = sceneStack.Peek ().Controller;
		if (sceneManagerController) {
			sceneManagerController.pauseSceneObjects (time);
		}

	}

	public void PopLastAndResume ()
	{
		if (sceneStack.Count == 0) {
			return;
		}

		SceneData sceneData = sceneStack.Pop ();

		if (sceneData.Controller) {
			sceneData.Controller.resumeSceneObjects ();
			SceneManager.UnloadScene (sceneData.CurrentSceneName);
		} else {
			SceneManager.LoadScene (sceneData.PreviousSceneName);
		}

	}

	public void PopWithCount (int backCount)
	{
		if (sceneStack.Count == 0) {
			return;
		}

		SceneData sceneData = null;
		for (int i = 0; i < backCount; i++) {
			sceneData = sceneStack.Pop ();
			SceneManager.UnloadScene (sceneData.CurrentSceneName);
		}

		if (sceneData.Controller) {
			sceneData.Controller.resumeSceneObjects ();
		} else {
			SceneManager.LoadScene (sceneData.PreviousSceneName);
		}

	}

	public void Clear ()
	{
		foreach (SceneData _sceneData in sceneStack) {
			if (_sceneData.Controller) {
				SceneManager.UnloadScene (_sceneData.PreviousSceneName);
			}
		}
		sceneStack.Clear ();

	}
}