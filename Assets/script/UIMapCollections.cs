using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.VersionControl;

public class UIMapCollections : MonoBehaviour
{
    public  GameObject UIpanel;
    public Sprite  mapImage;
    public bool mapGet;
    public Sprite[] collectionImages;
    public bool[] collctionsGet;
    private bool UIOn;
    // Start is called before the first frame update
    void Start()
    {
        UIOn = false;
        mapGet = false ;
        for (int i = 0; i < collctionsGet.Length; i++)
        {
            collctionsGet[i] = false;
        }
        UIpanel = GameObject.Find("Panel");
        UIpanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkM())
        {
            if (UIOn)
            {
                UIpanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
            else
            {
                UIpanel.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private bool checkM()
    {
        if(Input .GetKeyUp (KeyCode.M))
        {
            if(UIOn )
            {
                UIOn = false;
            }
            else
            {
                UIOn = true;
            }
            return true;
        }
        return false;
    }
}
