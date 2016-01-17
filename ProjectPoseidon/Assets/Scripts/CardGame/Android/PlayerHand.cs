using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
/* TODO:    Make this feel more tactile
 *              - Sound integration possibly
 *          Optimise for phone
 *          Make program work better
 *              - Set swipe side to side and scroll to be more precise                  (TICK)
 *              - Have swiping be less abuseable if player swipes too quickly           (TICK)
 *          Make it so it is able to be reused later on
 *          Pretty Code up and make it human readable
 *          Comment Code                                                               
 *          Rename script and most variables
 */



public class PlayerHand : MonoBehaviour
{
    #region "Variables"

    //PUBLIC VARIABLES
    #region "Public Variables"
    //The Prefab which will be instantiated for each card
    public GameObject cardPrefab;
    //The Canvas which have all cards parented to it
    public Canvas handCanvas;
    //The Gameobject in the scene which holds the PoolClass Component
    public CardPoolClass cardPoolComponent;

    //Inspector variables for user Interaction
    //Animation Curve which handles the scale of the cards when in proximity of the centre of the screen
    public AnimationCurve enlargeCurve;
    //Determines how many cards will be spawned on Start
    public int numberOfCards;
    //Controls how many pixels between cards.
    public int cardPadding;
    //Controls the speed multiplier for the movement of cards
    public float scrollSpeed;
    //Controls the speed at which the cards are centred
    public float centeringSpeed = 1000;
    //Boolean which checks whether to center the cards with the middle card or the first card
    public bool offsetToZero;
    #endregion

    //PRIVATE VARIABLES
    #region "Private Variables"
    // This stores the finger that's currently dragging this GameObject
    private Lean.LeanFinger playerFinger;
    //Holds all the cards which have been instantiated
    private CardClass[] playerHand;
    //Holds the integer for the closest card to the centre
    private int closestCard;
    //Holds the current rotationspeed of the cards
    private float cardSpinSpeed;
    //Boolean which is used to check if any card has been centerd
    private bool cardCentred;
    //Boolean which is used to check if the user has swiped upwards
    private bool swipedUpwards;

    #endregion

    #endregion

    #region "Main Methods"
    // Use this for initialization
    void Awake()
    {
        //CreateCards
        InstantiateCards();
        //Initialise the Position
        SetupPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!swipedUpwards)
        {
            ScrollWithoutLoop();
            CentreCard();
            EnlargeCentreCard();
            MoveUpwards();
        }
        else
        {
            if (RotateCard())
            {
                if (MoveCardUpwards())
                {
                    if (PlayDeckReturnAnim())
                    {
                        Debug.Log("Finished Animation");
                    }
                }
            }
        }
    }

    #endregion

    #region "Secondary Methods"

    //Method to instantiate the proper Cards
    void InstantiateCards()
    {
        //Initialise the size of the CardImage array to be the size specified in the inspector
        playerHand = new CardClass[numberOfCards];

        //For everyCard to be instantiated apply correct variables in the script
        for (int i = 0; i < numberOfCards; i++)
        {
            //Get what card to be spawned from CardPoolClass then
            //Instantiate the card object
            GameObject tempCard = Instantiate(cardPoolComponent.GenerateCard(), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            //Set the parent of the object to be the canvas which will hold the cards
            tempCard.transform.SetParent(handCanvas.transform);
            //Set the canvas to have correct settings
            //Set it so it overrides the default screenspace sorting - stops certain cards from appearing over others
            tempCard.GetComponent<Canvas>().overrideSorting = true;
            //Set each card to have the same custom sorting order
            tempCard.GetComponent<Canvas>().sortingOrder = -1;
            //Set the ID of the card to be the order it is instantiated
            tempCard.GetComponent<CardClass>().cardID = i;
            //Add to the players hand array the CardClass component. This class will be used from nowon when referencing cards
            playerHand[i] = tempCard.GetComponent<CardClass>();
        }
    }

    //Method to setup the cards correctly
    void SetupPosition()
    {
        //Set the minSize of the card to be the final value of the animation Curve
        float cardMinSize = enlargeCurve.keys[enlargeCurve.keys.Length - 1].value;
        //Set cardSize to be the first cards modified size, This will be used for setting the size of all cards.
        float cardSize = playerHand[0].transformComponent.sizeDelta.x * cardMinSize;

        //Iterate through each card and setup vars
        for (int i = 0; i < playerHand.Length; i++)
        {
            //Set the scale of the card to be last key on the animation curve
            playerHand[i].transformComponent.localScale = new Vector3(cardMinSize, cardMinSize, cardMinSize);
            //Create a float for the xPosition of the card
            float positionX;
            //If the cards are set to be offset to zero
            if (offsetToZero)
            {
                //Do some math to determine the position of the cards
                positionX = (-((cardSize) + (cardPadding)) * (playerHand.Length / 2)) + ((cardSize * i) + (cardPadding * i));
            }
            else
            {
                //Do some math to determine the position of the cards
                positionX = (cardSize * i) + (cardPadding * i);
            }
            //Set the position of the card, relative to the centre of the screen
            playerHand[i].transformComponent.anchoredPosition = new Vector2(positionX, 0);
        }
    }

    //Accepts player input to move cards along the card X axis
    void ScrollWithoutLoop()
    {
        //Check if the screen is currently being touched, this prevents null reference exceptions
        if (playerFinger != null)
        {
            //Get the distance that the finger moved in the last frame, then multiply it by the scroll speed
            cardSpinSpeed = playerFinger.DeltaScreenPosition.x * scrollSpeed;
        }
        // Check if the current card spin speed is not between -10 and 10;
        //Stops Jittering as the spin slows down
        if (cardSpinSpeed >= 10 | cardSpinSpeed <= -10)
        {
            //Set the card to not be centred
            cardCentred = false;

            //If the x coordinate of the first card is less than 0 (Centre of screen), and the cardSpin speed is going towards the right
            //or the opposite of this, do the next part. This is done so the player can always scroll back towards the card if
            //they are not trying to go past the fist or last cards
            if (((playerHand[0].transformComponent.anchoredPosition.x <= 0) && cardSpinSpeed > 0.0f) | ((playerHand[playerHand.Length - 1].transformComponent.anchoredPosition.x >= 0) && cardSpinSpeed < 0.0f))
            {
                //Do a loop which moves all cards in the specified swipe direction
                for (int i = 0; i < playerHand.Length; i++)
                {
                    //Add speed to the x position without editing the y.
                    playerHand[i].transformComponent.anchoredPosition += new Vector2(cardSpinSpeed, 0);
                }
            }
            //if the spin speed is more than 0
            if (cardSpinSpeed > 0)
            {
                //Decrement the cards spin speed by 0.1 of its absolute value. Looks good for a slowdown elastic effect
                cardSpinSpeed -= (Math.Abs(cardSpinSpeed) * 0.1f);
            }
            //or if the spin speed is less than 0
            else if (cardSpinSpeed < 0)
            {
                //Increment the cards spin speed by 0.1 of its absolute value. Looks good for a slowdown elastic effect
                cardSpinSpeed += (Math.Abs(cardSpinSpeed) * 0.1f);
            }
        }
        //Then call the method to find the current closest card to the centre of the screen.
        GetClosestCardToCentre();
    }

    //Method which finds the closest card to the centre of the screen
    void GetClosestCardToCentre()
    {
        //Set the smallest distance to be infinity for the search loop
        float smallestDistance = Mathf.Infinity;
        //For each card
        for (int i = 0; i < playerHand.Length; i++)
        {
            //Set the objects distance to be the absolute of it's position, as naturally the closer to the centre a card it the closer to zero it becomes
            float objectDistance = Mathf.Abs(playerHand[i].transformComponent.anchoredPosition.x);
            //If this distance is smaller than any of the other checked distances, set the current shortest distance to be this distance
            if (objectDistance < smallestDistance)
            {
                smallestDistance = objectDistance;
                //set the closest card to be the current index of the card
                closestCard = i;
            }
        }
    }

    //Centres the card to the middle of the screen
    void CentreCard()
    {
        //If the screen is not being touched
        if (playerFinger == null)
        {
            //And if the closest card is more than 10
            if (playerHand[closestCard].transformComponent.anchoredPosition.x > 10)
            {
                //For each card in the array
                for (int i = 0; i < playerHand.Length; i++)
                {
                    //Decrement the distance between the posiution and the centre by user defined variable by time
                    playerHand[i].transformComponent.anchoredPosition -= new Vector2(centeringSpeed * Time.deltaTime, 0);
                }
            }
            //Otherwise if the card is less than -10
            else if (playerHand[closestCard].transformComponent.anchoredPosition.x < -10)
            {
                //For each card in the array
                for (int i = 0; i < playerHand.Length; i++)
                {
                    //increment the distance between the posiution and the centre by user defined variable by time
                    playerHand[i].transformComponent.anchoredPosition += new Vector2(centeringSpeed * Time.deltaTime, 0);
                }
            }
            //And if the closest card is between -10 and 10
            else
            {
                //Set that the card is centred to true
                cardCentred = true;
            }
        }
    }

    //Method which enlarges the centre card and its surrounding cards
    void EnlargeCentreCard()
    {
        //Check if the closest card is 0, solves out of range exceptions with for loop
        if (closestCard == 0)
        {
            //For this card and the one to its right
            for (int i = 0; i <= closestCard + 1; i++)
            {
                //Set the scale of the card to associate to the animation curve
                playerHand[i].transformComponent.localScale = new Vector3(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), 0);
                //Set the sorting order of the cards to relate to the animation curve aswell
                playerHand[i].canvasComponent.sortingOrder = Mathf.RoundToInt((float)(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)) - 0.75) * 10);
            }
        }
        //Check if the closest card is the last card, solves out of range exceptions with for loop
        else if (closestCard == playerHand.Length - 1)
        {
            //For this card and the one to its left
            for (int i = closestCard - 1; i <= playerHand.Length - 1; i++)
            {
                //Set the scale of the card to associate to the animation curve
                playerHand[i].transformComponent.localScale = new Vector3(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), 0);
                //Set the sorting order of the cards to relate to the animation curve aswell
                playerHand[i].canvasComponent.sortingOrder = Mathf.RoundToInt((float)(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)) - 0.75) * 10);
            }
        }
        //If the cards are in the middle
        else
        {
            //For this card and the one to its left and right
            for (int i = closestCard - 1; i <= closestCard + 1; i++)
            {
                //Set the scale of the card to associate to the animation curve
                playerHand[i].transformComponent.localScale = new Vector3(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)), 0);
                //Set the sorting order of the cards to relate to the animation curve aswell
                playerHand[i].canvasComponent.sortingOrder = Mathf.RoundToInt((float)(enlargeCurve.Evaluate(Math.Abs(playerHand[i].transformComponent.anchoredPosition.x / 1000)) - 0.75) * 10);
            }
        }
    }

    //Handles when the player submits the card by swiping upwards
    void MoveUpwards()
    {
        //If there is a finger on the screen, solves null reference exceptions
        if (playerFinger != null)
        {
            //If there is a card currently centred
            if (cardCentred)
            {
                //if the swipe upwards is more than the swipe to the x
                if (((playerFinger.SwipeDelta.x > 2f) | (playerFinger.SwipeDelta.x < -2f)) && (playerFinger.SwipeDelta.y > Mathf.Abs(playerFinger.SwipeDelta.x)))
                {
                    //Set swipedUpwards to true
                    swipedUpwards = true;
                }
            }
        }
    }

    //Method to move the card off the screen upwards
    bool MoveCardUpwards()
    {
        //If the card is less than the height of the screen plus the cards height
        if (playerHand[closestCard].transformComponent.anchoredPosition.y < Screen.height + playerHand[closestCard].transformComponent.sizeDelta.y)
        {
            //increment the height of card so it zooms off the screen cleanly. Math stuff
            playerHand[closestCard].transformComponent.anchoredPosition += new Vector2(0, ((50 * (Mathf.Abs(playerHand[closestCard].transformComponent.anchoredPosition.y + 1) / 5)) + 200) * Time.deltaTime);
            return false;
        }
        return true;
    }

    bool RotateCard()
    {
        Vector3 cardRotation = playerHand[closestCard].transformComponent.localRotation.eulerAngles;
        if (cardRotation.y >= 179)
        {
            return true;
        }
        playerHand[closestCard].transformComponent.localRotation = Quaternion.Euler(Vector3.Lerp(cardRotation, new Vector3(cardRotation.x, 180, cardRotation.z), 5 * Time.deltaTime));
        return false;
    }

    bool PlayDeckReturnAnim()
    {
        bool animFinished = true;
        for (int i = 0; i < playerHand.Length; i++)
        {
            if (playerHand[i].transformComponent.anchoredPosition.x > 1)
            {
                playerHand[i].transformComponent.anchoredPosition = Vector2.Lerp(playerHand[i].transformComponent.anchoredPosition, new Vector2(0, 0), 3 * Time.deltaTime);
            }
            else if (playerHand[i].transformComponent.anchoredPosition.x < -1)
            {
                playerHand[i].transformComponent.anchoredPosition = Vector2.Lerp(playerHand[i].transformComponent.anchoredPosition, new Vector2(0, 0), 5 * Time.deltaTime);
            }
            //Check if the animation is finished
            if (playerHand[i].transformComponent.anchoredPosition.x > -1 && playerHand[i].transformComponent.anchoredPosition.x < 1)
            {
                animFinished = false;
            }
        }

        if (animFinished)
        {
            return true;
        }
        return false;
    }

    #endregion

    #region LeanTouchIntegration

    protected virtual void OnEnable()
    {
        // Hook into the OnFingerDown event
        Lean.LeanTouch.OnFingerDown += OnFingerDown;

        // Hook into the OnFingerUp event
        Lean.LeanTouch.OnFingerUp += OnFingerUp;
    }

    protected virtual void OnDisable()
    {
        // Unhook the OnFingerDown event
        Lean.LeanTouch.OnFingerDown -= OnFingerDown;

        // Unhook the OnFingerUp event
        Lean.LeanTouch.OnFingerUp -= OnFingerUp;
    }

    public void OnFingerDown(Lean.LeanFinger finger)
    {
        // Set the current finger to this one
        playerFinger = finger;
    }

    public void OnFingerUp(Lean.LeanFinger finger)
    {
        // Was the current finger lifted from the screen?
        if (finger == playerFinger)
        {
            // Unset the current finger
            playerFinger = null;
        }
    }
    #endregion
}
