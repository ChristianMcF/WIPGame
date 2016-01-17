using UnityEngine;
using System.Collections;

public class AndroidMainMenu : MonoBehaviour
{

    public Animation leftAnim;
    public Animation rightAnim;
    public GameObject playerHand;

    public void ClickedScreen()
    {
        Debug.Log("ASHIE");
        leftAnim.Play();
        rightAnim.Play();
        playerHand.SetActive(true);
        gameObject.SetActive(false);
    }
}
