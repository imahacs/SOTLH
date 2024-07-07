using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMAnager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        // to refrance and clean code 
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation , bool isInteracting)
    {// this function is used to play any animation we want 
        // is interacting so we don't get looked in the animation 
        animator.SetBool("isInteracting", isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);

    }

    public void UpdateAniamtorValues(float horivontalMovement , float verticlaMovement ,bool isSprinting)
    {

        //Animation snapping , unsed to round the valuse , looks nice # not needed!!
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horivontalMovement > 0 && horivontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
            
        }
        else if (horivontalMovement > 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if (horivontalMovement < 0 && horivontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horivontalMovement < -0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion

        #region Snapped Vertical
        if (verticlaMovement > 0 && verticlaMovement < 0.55f)
        {
            snappedVertical = 0.5f;

        }
        else if (verticlaMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticlaMovement < 0 && verticlaMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticlaMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isSprinting)
        {
            snappedHorizontal = horivontalMovement;
            snappedVertical = 2;
        }

        // here we use function to pass variables will take from player movment 
        // fist var is int , then what been passed , blend time , time of frame
        animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
