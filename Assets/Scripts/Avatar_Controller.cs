using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar_Controller : MonoBehaviour
{
    private bool isLeftSide;
    public GameObject rest_avatar;
    public GameObject active_avatar;

    public void setLeftSide(bool isLeft)
    {
        isLeftSide = isLeft;

        if (isLeftSide)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        rest_avatar.GetComponent<Rest_Movement>().setLeftSide(isLeftSide);
        active_avatar.GetComponent<playerAnimationController>().setLeftSide(isLeftSide);

    }
}
