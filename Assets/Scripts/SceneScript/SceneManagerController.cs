using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class SceneManagerController : AbstractBackKeyHandler
{
    private GameObject[] rootGameObjects;
    public void getRootObjects()
    {
        rootGameObjects = gameObject.scene.GetRootGameObjects();
    }

    public void reloadEventSystem()
    {
        getRootObjects();
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            if( rootGameObjects[i].GetComponent<EventSystem>() != null )
            {
                rootGameObjects[i].SetActive( false );
                rootGameObjects[i].SetActive( true );
                return;
            }
        }
    }

    public void pauseSceneEventSystem()
    {
        getRootObjects();

        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            if( rootGameObjects[i] != null )
            {
                if( rootGameObjects[i].GetComponent<EventSystem>() != null )
                {
                    rootGameObjects[i].SetActive( false );
                    return;
                }
            }

        }
    }

    public void pauseSceneObjectsAll()
    {
        getRootObjects();
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            if( rootGameObjects[i] != null )
            {
                if( rootGameObjects[i].name != "SceneManager" )
                {
                    rootGameObjects[i].SetActive( false );
                }
                else
                {
                    rootGameObjects[i].SetActive( true );
                }
            }

        }
    }

    public void pauseSceneObjects(float time)
    {
        getRootObjects();
        List<int> gameObjectList = new List<int>();
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            if( rootGameObjects[i].GetComponent<Canvas>() != null)
            {
                gameObjectList.Add(i);
				StartCoroutine( TimeController.PostDelayed( time, () =>
                    {
                        for(int k = 0; k < gameObjectList.Count; k++)
                        {
                            int number = gameObjectList[k];
                            if( rootGameObjects[number].name != "SceneManager" )
                            {
                                rootGameObjects[number].SetActive(false);
                            }
                        }

                    }
                ) );
            }
            else
            {
                if( rootGameObjects[i].name != "SceneManager" )
                {
                    rootGameObjects[i].SetActive(false);
                }
            }
        }
    }

    public void resumeSceneObjects()
    {
        getRootObjects();

        int eventSystemNumber = 0;
        for (int i = 0; i < rootGameObjects.Length; i++)
        {
            if( rootGameObjects[i].GetComponent<EventSystem>() != null )
            {
                eventSystemNumber = i;
				StartCoroutine( TimeController.PostDelayed( 0.2f, () =>
                        {
                        rootGameObjects[eventSystemNumber].SetActive( false );
                        rootGameObjects[eventSystemNumber].SetActive( true );
                        }
                    ) );
            }
            else
            {
                rootGameObjects[i].SetActive(true);
            }
        }
    }
}
