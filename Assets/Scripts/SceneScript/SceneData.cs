using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneData 
{
	public string PreviousSceneName
	{
		get;
		set;
	}

	public string CurrentSceneName
	{
		get;
		set;
	}

	public SceneManagerController Controller
	{
		get;
		set;
	}

	public LoadSceneMode LoadingMode
	{
		get;
		set;
	}

	public SceneData(string previousSceneName, string currentSceneName) :
		this(previousSceneName, currentSceneName, null)
	{

	}

	public SceneData(string previousSceneName, string currentSceneName, SceneManagerController controller)
	{
		PreviousSceneName = previousSceneName;
		CurrentSceneName = currentSceneName;
		Controller = controller;
		if (controller != null)
		{
			LoadingMode = LoadSceneMode.Additive;
		}
		else
		{
			LoadingMode = LoadSceneMode.Single;
		}
	}
}
