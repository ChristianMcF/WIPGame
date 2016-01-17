using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CardClass : MonoBehaviour
{
    //Component Variables
    [HideInInspector]
    public Image imageComponent;
    [HideInInspector]
    public RectTransform transformComponent;
    [HideInInspector]
    public Canvas canvasComponent;

    //Identifier Variables
    public int cardID;
    public int playerNum;

    //Generic Variables
    //Image Variables
    //Generic Card Image
    public Sprite cardFrontImage;
    //Generic Card Back Image
    public Sprite cardBackImage;

    // Use this for initialization
    void Awake()
    {
        InitializeVariables();
    }

    void Update()
    {
        RotationCheck();
    }

    private void InitializeVariables()
    {
        imageComponent = gameObject.GetComponent<Image>();
        transformComponent = gameObject.GetComponent<RectTransform>();
        canvasComponent = gameObject.GetComponent<Canvas>();
    }

    void RotationCheck()
    {
        if (((transformComponent.localRotation.eulerAngles.y >= 90) && (transformComponent.localRotation.eulerAngles.y <= 270)) && imageComponent.sprite != cardBackImage)
        {
            imageComponent.sprite = cardBackImage;
        }
        else if (((transformComponent.localRotation.eulerAngles.y < 90) | (transformComponent.localRotation.eulerAngles.y > 270)) && imageComponent.sprite != cardFrontImage)
        {
            imageComponent.sprite = cardFrontImage;
        }
    }
}
