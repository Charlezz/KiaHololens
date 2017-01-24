using UnityEngine;
using System.Collections;

public abstract class AbstractBackKeyHandler : MonoBehaviour
{

	public void SetOnBackEnabled (bool enabled)
	{
		if (enabled) {
			BackKeyHandler.Instance.BackKeyAction += () => {
				OnBackPressed ();
			};
		}
	}

	public virtual void OnBackPressed ()
	{
		SceneStackController.Instance.PopLastAndResume ();
	}
}
