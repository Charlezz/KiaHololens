using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Input;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;

public abstract class BaseController<T> : MonoBehaviour where T : BaseMenuItem
{
    public const int STATE_OUT_OF_BOUND = 0;
    public const int STATE_NONE = 1;
    public const int STATE_FOCUSED = 2;
    

    private T focusedMenu;
    private GestureRecognizer recognizer;
    public Cursor2D cursor;
    public GameObject cameraTransform;

    private bool isWindowFollowing = true;

    private int current_state = 0;


    protected KeywordRecognizer keywordRecognizer = null;
    protected Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    // Use this for initialization
    void Start()
    {
        cameraTransform.transform.position = Vector3.zero;
        cameraTransform.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFollowing(bool enable)
    {
        isWindowFollowing = enable;
    }

    protected void initRecognizer()
    {
        recognizer = new GestureRecognizer();
        recognizer.TappedEvent += (source, tapCount, ray) =>
        {
            if (focusedMenu != null)
            {
                focusedMenu.OnTap();
            }
        };
        recognizer.StartCapturingGestures();
    }

    protected void initCursor()
    {
        cursor.onCursorResultEvent += OnCursorEvent;
    }

    protected void OnCursorEvent(List<RaycastResult> results)
    {

        current_state = results.Count;

        switch (current_state)
        {
            case STATE_OUT_OF_BOUND:
                if (focusedMenu)
                {
                    focusedMenu.SetUIEnable(false);
                    focusedMenu = null;
                }

                if (isWindowFollowing)
                {
                    Vector3 camPos = Camera.main.transform.position;
                    Vector3 camTransPos = cameraTransform.transform.position;

                    cameraTransform.transform.position = Vector3.Lerp(camTransPos, camPos, Time.smoothDeltaTime);
                    cameraTransform.transform.rotation = Quaternion.Slerp(cameraTransform.transform.rotation, Camera.main.transform.rotation, Time.smoothDeltaTime);
                    cameraTransform.transform.eulerAngles = new Vector3(cameraTransform.transform.eulerAngles.x, cameraTransform.transform.eulerAngles.y, 0);
                }

                break;
            case STATE_NONE:
                if (focusedMenu)
                {
                    focusedMenu.SetUIEnable(false);
                    focusedMenu = null;
                }
                break;
            case STATE_FOCUSED:
                T oldFocusedMenu = focusedMenu;

                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.GetComponent<T>() != null)
                    {
                        focusedMenu = results[i].gameObject.GetComponent<T>();
                        break;
                    }
                }


                if (focusedMenu!=oldFocusedMenu)
                {
                    if (oldFocusedMenu != null)
                    {
                        oldFocusedMenu.SetUIEnable(false);
                    }
                    
                    focusedMenu.SetUIEnable(true);
                }
                
                break;
        }

        if (isWindowFollowing)
        {
            float cameraPosZ = Camera.main.transform.position.z;
            float z = Mathf.Lerp(cameraTransform.transform.position.z, cameraPosZ, Time.smoothDeltaTime);

            cameraTransform.transform.position = new Vector3(cameraTransform.transform.position.x, cameraTransform.transform.position.y, z);

        }
    }

    //protected void OnCursorStateEvent(int state)
    //{
    //    switch (state)
    //    {
    //        case Cursor2D.STATE_FOCUSED:

    //            break;
    //        case Cursor2D.STATE_NONE:

    //            break;
    //        case Cursor2D.STATE_OUT_OF_BOUND:
    //            if (isWindowFollowing)
    //            {
    //                Vector3 camPos = Camera.main.transform.position;
    //                Vector3 camTransPos = cameraTransform.transform.position;

    //                cameraTransform.transform.position = Vector3.Lerp(camTransPos, camPos, Time.smoothDeltaTime);
    //                cameraTransform.transform.rotation = Quaternion.Slerp(cameraTransform.transform.rotation, Camera.main.transform.rotation, Time.smoothDeltaTime);
    //            }
    //            if (focusedMenu)
    //            {
    //                focusedMenu.SetUIEnable(false);
    //            }
    //            break;
    //    }

    //    if (isWindowFollowing)
    //    {
    //        float cameraPosZ = Camera.main.transform.position.z;
    //        float z = Mathf.Lerp(cameraTransform.transform.position.z, cameraPosZ, Time.smoothDeltaTime);

    //        cameraTransform.transform.position = new Vector3(cameraTransform.transform.position.x, cameraTransform.transform.position.y, z);

    //    }



    //}

    public void TapWithRightButton()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (focusedMenu != null)
            {
                focusedMenu.OnTap();
            }

        }
    }


    private bool isDragging;
    private Quaternion initCamera;
    private Vector3 initMousePos, initCameraPos;
    protected void ControlCameraWithMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            initCamera = Camera.main.transform.rotation;
            initCameraPos = Camera.main.transform.position;
            initMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (isDragging)
            {
                Vector3 diff = initMousePos - Input.mousePosition;

                Camera.main.transform.position = initCameraPos + diff * 0.005f;

            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    protected void TestWithMouse()
    {
        ControlCameraWithMouse();
        TapWithRightButton();
    }

    void OnDisable()
    {
        VoiceManager.Instance.CancelAll();
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
        }
    }

    protected void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    

}
