using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour {
    public static IEnumerator PostDelayed(float time, System.Action success)
    {
        yield return new WaitForSeconds(time);
        success();
    } 
}
