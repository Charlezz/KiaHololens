using UnityEngine;
using System.Collections;

public class StepManager : MonoBehaviour {

    public Animation cable2hole, cable3hole;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Step1();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Step2();
        }
        
	}

    public void Step1()
    {
        cable2hole.Play();
    }
    public void Step2()
    {
        cable3hole.Play();
    }
    public void Step3()
    {

    }
}
