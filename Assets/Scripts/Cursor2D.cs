using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Cursor2D : MonoBehaviour
{
    public GraphicRaycaster[] mGraphicRaycaster;
	private Vector2 center;
	private List<RaycastResult> results = new List<RaycastResult> ();


	public delegate void cursorResultHandler (List<RaycastResult> results);

	public event cursorResultHandler onCursorResultEvent;

    public Image cursor;


    // Use this for initialization
    void Start ()
	{
		center = new Vector2 (Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
	}

    private Vector3 newCameraPos;

    private bool isNothing=true;
	// Update is called once per frame
	void Update ()
	{
        
        Vector3 oldCameraPos = newCameraPos;
        newCameraPos = Camera.main.transform.position;

        results.Clear ();
		PointerEventData ped = new PointerEventData (null);
		ped.position = center;

        for(int i = 0; i < mGraphicRaycaster.Length; i++)
        {

            if (mGraphicRaycaster[i].gameObject.activeSelf)
            {
                mGraphicRaycaster[i].Raycast(ped, results);
                if (onCursorResultEvent != null)
                {
                    onCursorResultEvent(results);
                }
            }
           
        }
		


        
        
      
    }
}
