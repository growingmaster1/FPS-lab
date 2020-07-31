using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINav : MonoBehaviour,IPointerClickHandler 
{
    public int order;
    //private bool isOn;
    public GameObject onPic;
    private static int onOrder=1;
    private Image navImage;
    public Sprite[] pics;
    private UIMapCollections UIcontroller;
    public  GameObject leftButton;
    public  GameObject rightButton;
    public  GameObject imageIndex;
    private bool isChange;
    private int collectionOn;
    private int getNum;
    

    // Start is called before the first frame update
    void OnEnable()
    {
        isChange = false;
        /*if(order == 1)
        {
            isOn = true;
        }
        else
        {
            isOn = false;
        }*/
        navImage = gameObject.GetComponent<Image>();
        UIcontroller = GameObject.Find("Canvas").GetComponent<UIMapCollections>();
        //onPic = GameObject.Find("OnImage");
        onPic.SetActive(false);
        //leftButton = GameObject.Find("leftSwitchButton");
        leftButton.SetActive(false);
        //rightButton = GameObject.Find("rightSwitchButton");
        rightButton.SetActive(false);
        //imageIndex = GameObject.Find("imageIndex");
        imageIndex.SetActive(false);
        getNum = 0;
        collectionOn = 0;
        show1();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input .GetMouseButtonUp (0))
        {
            Debug.Log("order:" + order + "  onOrder:" + onOrder);
            if (order == onOrder)
            {
                navImage.sprite = pics[0];
            }
            else if (order < onOrder)
            {
                navImage.sprite = pics[1];
                isChange = true;
            }
            else if (order > onOrder)
            {
                navImage.sprite = pics[2];
                isChange = true;
            }
            if (isChange)
            {
                onPic.SetActive(false);
                leftButton.SetActive(false);
                rightButton.SetActive(false);
                imageIndex.SetActive(false);
                if (onOrder == 1)
                {
                    show1();
                    Debug.Log("show1");
                }
                else
                {
                    show2();
                    Debug.Log("show2");
                }
            }
            isChange = false;
        }
        if (onOrder == 2&&getNum >1)
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                leftSwitchFun();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                rightSwitchFun();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onOrder = order;
    }

    void show1()
    {
        if (UIcontroller.mapGet) 
        {
            onPic.SetActive(true);
            onPic.GetComponent<Image>().sprite = UIcontroller.mapImage;
        }
    }

    void show2()
    {
        int i;
        getNum = 0;
        for (i = 0; i < UIcontroller.collctionsGet.Length; i++) 
        {
            if(UIcontroller .collctionsGet [i])
            {
                getNum++;
            }
        }
        if(getNum !=0)
        {
            onPic.SetActive(true);
            for(i=0;i<UIcontroller .collctionsGet .Length;i++)
            {
                if(UIcontroller .collctionsGet [i])
                {
                    onPic.GetComponent<Image>().sprite = UIcontroller.collectionImages[i];
                    break;
                }
            }
            if(getNum >1)
            {
                leftButton.SetActive(true);
                rightButton.SetActive(true);
                imageIndex.SetActive(true);
                imageIndex.GetComponent<Text>().text = "1/" + getNum;
                collectionOn  = 1;
            }
        }
    }

    public void leftSwitchFun()
    {
        int i;
        if (collectionOn != 1 && collectionOn != 0)
        {
            collectionOn--;
        }
        else
        {
            for (i = UIcontroller.collctionsGet.Length - 1; i >= 0; i--) 
            {
                if (UIcontroller.collctionsGet[i]) 
                {
                    collectionOn = i+1;
                    break;
                }
            }
            if (i == -1)
            {
                collectionOn = 0;
            }
        }
        onPic.GetComponent<Image>().sprite = UIcontroller.collectionImages[collectionOn - 1];
        imageIndex.GetComponent<Text>().text = collectionOn +"/" + getNum;
    }
    
    public void rightSwitchFun()
    {
        int i;
        if (collectionOn != getNum)
        {
            collectionOn++;
        }
        else
        {
            collectionOn = 1;
        }
        onPic.GetComponent<Image>().sprite = UIcontroller.collectionImages[collectionOn - 1];
        imageIndex.GetComponent<Text>().text = collectionOn + "/" + getNum;
    }
}
