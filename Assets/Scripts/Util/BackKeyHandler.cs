using UnityEngine;
using System.Collections;

public class BackKeyHandler : Singleton<BackKeyHandler>
{
	public System.Action BackKeyAction {
		get;
		set;
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (BackKeyAction != null) {
				int delegateCount = BackKeyAction.GetInvocationList ().Length;
				System.Action action = (System.Action)BackKeyAction.GetInvocationList () [delegateCount - 1];
				BackKeyAction -= action;
				action ();
			}
		}
	}
}
