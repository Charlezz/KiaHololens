using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{

		StartCoroutine (
			TimeController.PostDelayed (3.0f, () => {
				SceneManager.LoadScene (SceneName.MAIN);
			})
		);


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


}
