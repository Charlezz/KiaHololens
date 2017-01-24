using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Linq;
using UnityEngine.Windows.Speech;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class BriefController : MonoBehaviour {

    public Brief2DController brief2D;

    public GameObject refPoint;
    public MeshRenderer videoRenderer;
    public GameObject jointBox;
    public Animation cable2,cable3;
    public GameObject cableChild2, cableChild3;
    public Text guideText;

    private KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    Texture2D[] textures = null;

    public GameObject canvas,b4start;
    void Start () {
        //VoiceManager.Instance.Say(VoiceManager.Drag_the_3D_model_onto_the_High_Voltage_Joint_Box__If_done_say_OK, 0.5f);
        NextAnimation();
        initRecognizer();

        textures = Resources.LoadAll<Texture2D>("kia_alpha") ;
        initKeyword();

        //QualitySettings.SetQualityLevel(0, true);
    }

    void OnDestroy()
    {
        //QualitySettings.SetQualityLevel(6, true);
    }


    private bool isVideoPlayed;
    
    void initKeyword()
    {
        keywords.Add("OK", () =>
        {
            NextAnimation();
            b4start.SetActive(false);
        });
        keywords.Add("Next", () =>
        {
            Debug.Log("Said Next");
            NextAnimation();
        });
        keywords.Add("Back", () =>
        {
            Debug.Log("back");
            brief2D.OnBackClick();
        });
        keywords.Add("Previous", () =>
        {
            Debug.Log("Said Previous");
            PreviousAnimation();
        });
        keywords.Add("Main Menu", () =>
        {
            Debug.Log("Said Main Menu");
            SceneManager.LoadScene(SceneName.MAIN);
        });
        keywords.Add("High Quality", () =>
        {
            QualitySettings.SetQualityLevel(5, true);
        });
        keywords.Add("Low Quality", () =>
        {
            QualitySettings.SetQualityLevel(0, true);
        });



        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        
        keywordRecognizer.Start();
        

    }

   

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }


    GameObject focusedObject;
	// Update is called once per frame
	void Update () {


        TestWithMouse();

        KeyInput();


        Animator anim = jointBox.GetComponent<Animator>();

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Loosen 4bolts"))
        {
            currentAnimation = 1;
        } else if (anim.GetCurrentAnimatorStateInfo(0).IsName("remove box"))
        {
            currentAnimation = 2;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("idle(cable)"))
        {
            currentAnimation = 0;
        }

        if (canvas != null)
        {
            canvas.transform.LookAt(Camera.main.transform);
            canvas.transform.eulerAngles = new Vector3(0, canvas.transform.eulerAngles.y+180f, 0);
        }

        if(b4start!=null && b4start.activeSelf)
        {
            b4start.transform.LookAt(Camera.main.transform);
            b4start.transform.eulerAngles = new Vector3(0, b4start.transform.eulerAngles.y + 180f, 0);


            jointBox.transform.LookAt(Camera.main.transform);
            jointBox.transform.eulerAngles = new Vector3(0, jointBox.transform.eulerAngles.y, 0);

        }

    }


    private GestureRecognizer recognizer;

    private bool isManipulating;
    private Vector3 manipulationPreviousPosition;

    protected void initRecognizer()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);

      

        recognizer.ManipulationCanceledEvent += (source, position, ray) =>
        {
            Debug.Log("ManipulationCanceledEvent");
            isManipulating = false;
        };

        recognizer.ManipulationCompletedEvent +=(source, position, ray) =>
        {
            Debug.Log("ManipulationCompletedEvent");
            isManipulating = false;
        };

        recognizer.ManipulationStartedEvent += (source, position, ray) =>
        {
            Debug.Log("ManipulationStartedEvent");
            isManipulating = true;
                manipulationPreviousPosition = position;
        };
        recognizer.ManipulationUpdatedEvent+= (source, position, ray) =>
        {
            Debug.Log("ManipulationUpdatedEvent");
            isManipulating = true;
                Vector3 moveVector = Vector3.zero;
                moveVector = position - manipulationPreviousPosition;
                manipulationPreviousPosition = position;
                refPoint.transform.position += moveVector;
        };



        
        recognizer.StartCapturingGestures();
    }

    public void KeyInput()
    {

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            NextAnimation();
        }else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            PreviousAnimation();
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
        KeyInput();
    }


    public int currentAnimation=0;
    public int cableState = 0;
    

    public IEnumerator PlayVideo()
    {
        videoRenderer.gameObject.SetActive(true);
        videoRenderer.transform.SetParent(refPoint.transform, true);
        jointBox.SetActive(false);
        
        for (int i = 0; i < textures.Length; i++)
        {
            Debug.Log("video renderer " + i + " frame");
            videoRenderer.material.mainTexture = textures[i] as Texture2D;
            yield return new WaitForSeconds(0.0025f);
        }
        videoRenderer.gameObject.SetActive(false);
        videoRenderer.transform.SetParent(jointBox.transform, true);
        jointBox.SetActive(true);

        jointBox.GetComponent<Animator>().SetInteger("WhereToGo", 1);
        VoiceManager.Instance.Say(VoiceManager.If_finished_say_Next, 3.5f);
    }
    public void NextAnimation()
    {
        Debug.Log("next animation");
        switch (currentAnimation)
        {
            case 0:
                switch (cableState)
                {
                    case 0:
                        Debug.Log("cable3 play");
                        VoiceManager.Instance.Say(VoiceManager.Disconnet_the_quick_charge_high_voltage_cable, 0.5f);
                        VoiceManager.Instance.Say(VoiceManager.If_finished_say_Next, 3.5f);
                        cable3.Play();
                        cableState = 1;
                        StartCoroutine(TimeController.PostDelayed(3.2f, () =>
                        {
                            cableChild3.gameObject.SetActive(false);
                        }));
                        
                        //VoiceManager.Instance.Say(VoiceManager.If_finished_say_Next, 3.5f);
                        break;
                    case 1:
                        Debug.Log("cable2 play");
                        VoiceManager.Instance.Say(VoiceManager.Disconnect_the_high_voltage_battery_cable);
                        VoiceManager.Instance.Say(VoiceManager.If_finished_say_Next, 3.5f);

                        guideText.text = "Disconnect the high voltage battery cable.";
                        cable2.Play();
                        StartCoroutine(TimeController.PostDelayed(3.2f, () =>
                        {
                            //cable2.gameObject.SetActive(false);
                            cableChild2.gameObject.SetActive(false);
                        }));
                        cableState = 2;
                        break;
                    case 2:
                        guideText.text = "Loosen four bolts(4EA).";
                        VoiceManager.Instance.Say(VoiceManager.Loosen_four_bolts);
                        VoiceManager.Instance.Say(VoiceManager.If_finished_say_Next, 1.5f);
                        jointBox.GetComponent<Animator>().SetInteger("WhereToGo", 1);
                        cableState = -1;
                        cable2.Rewind();
                        cable2.Play();
                        cable2.Sample();
                        cable2.Stop();
                        cable3.Rewind();
                        cable3.Play();
                        cable3.Sample();
                        cable3.Stop();

                        cableChild2.gameObject.SetActive(false);
                        cableChild3.gameObject.SetActive(false);
                        break;
                }
                break;
            case 1:
                guideText.text = "Remove the High voltage joint box.";
                VoiceManager.Instance.Say(VoiceManager.This_is_the_last_step);
                VoiceManager.Instance.Say(VoiceManager.Remove_the_high_voltage_joint_box, 1.5f);
                jointBox.GetComponent<Animator>().SetInteger("WhereToGo", 2);


                StartCoroutine(TimeController.PostDelayed(5.0f, () => { NextAnimation(); }));
                break;
            case 2:
                refPoint.SetActive(false);
                SceneManager.LoadScene(SceneName.NEXT_PROCEDURE);
                break;
        }
    }

    public void StartVideo()
    {
        Debug.Log("StartVideo");
        b4start.SetActive(false);
        StartCoroutine(PlayVideo());
    }
    public void PreviousAnimation()
    {
        StopAllCoroutines();
        switch (currentAnimation)
        {
            case 0:
                switch (cableState)
                {
                    case 0:
                        break;
                    case 1:
                        cableState = 0;
                        cable3.Rewind();
                        cable3.Play();
                        cable3.Sample();
                        cable3.Stop();

                        cableChild3.gameObject.SetActive(true);

                        guideText.text = "Disconnect the quick charge high voltage cable.";
                        break;
                    case 2:
                        cableChild2.gameObject.SetActive(true);
                        cableState = 1;
                        cable2.Rewind();
                        cable2.Play();
                        cable2.Sample();
                        cable2.Stop();

                        

                        guideText.text = "Disconnect the high voltage battery cable.";
                        break;
                }
                break;
            case 1:
                guideText.text = "Disconnect the quick charge high voltage cable.";
                cableState = 0;
                cable2.Rewind();
                cable2.Play();
                cable2.Sample();
                cable2.Stop();
                cable3.Rewind();
                cable3.Play();
                cable3.Sample();
                cable3.Stop();
                cableChild2.gameObject.SetActive(true);
                cableChild3.gameObject.SetActive(true);
                jointBox.GetComponent<Animator>().SetInteger("WhereToGo", 4);
                
                break;
            case 2:
                guideText.text = "Loosen four bolts(4EA).";
                VoiceManager.Instance.Say(VoiceManager.Loosen_four_bolts, 0.5f);
                jointBox.GetComponent<Animator>().SetInteger("WhereToGo", 3);
                break;

        }
    }


}
