using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class BaseMenuItem : MonoBehaviour
{
    protected bool isEnable;
    public Image image;
    public Text text;
    public AudioSource mAudioSource;
    public UnityEvent mEvent;
    public Sprite normal, over;



    public virtual void SetUIEnable(bool enable)
    {
       
        if (enable)
        {
            image.sprite = over;
            gameObject.transform.localScale = MaxstSize.HIGHLIGHT;
            mAudioSource.Play();
        }
        else
        {
            image.sprite = normal;
            gameObject.transform.localScale = MaxstSize.NORMAL;
        }
    }

    public void OnTap()
    {
        if (mEvent != null) { mEvent.Invoke(); }
    }

}
