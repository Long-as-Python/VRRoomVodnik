using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRayBehaviour : MonoBehaviour
{
    public const float ROT_SPEED_HOR = 5;
    public const float ROT_SPEED_VER = 5;
    private const float MAX_VIEW_ANGLE = 90;
    private const float MIN_VIEW_ANGLE = -90;
    private float horizonSlope = 0.0f; //Needed to save transformation
    private float sideSlope = 0.0f; //Needed to save transformation
    public float range = 5;
    const float MENU_TIME_ELAPSED = 3;
    bool inMenu = false;

    public float holdMenuTime;

    // Start is called before the first frame update
    void ResetMenuHold()
    {
        holdMenuTime = 0;
    }

    bool MenuEntryFulfilled()
    {
        if (holdMenuTime >= MENU_TIME_ELAPSED)
            return true;
        return false;
    }

    bool MenuExitFulfilled()
    {
        if (holdMenuTime <= 0)
            return true;
        return false;
    }

    void Start()
    {
        ResetMenuHold();
    }

    private void FirstViewRotateMouse()
    {
        float currentAngle = Mathf.Clamp(sideSlope - ROT_SPEED_HOR * Input.GetAxis("Mouse Y"), MIN_VIEW_ANGLE,
            MAX_VIEW_ANGLE);
        if (currentAngle > MIN_VIEW_ANGLE && currentAngle < MAX_VIEW_ANGLE)
        {
            horizonSlope += ROT_SPEED_VER * Input.GetAxis("Mouse X");
            sideSlope -= ROT_SPEED_HOR * Input.GetAxis("Mouse Y");
        }

        transform.eulerAngles = new Vector3(sideSlope, horizonSlope, 0);
    }

    bool RaycastPointingSelector(Ray theRay)
    {
        if (Physics.Raycast(theRay, out RaycastHit hit, range) && hit.collider.tag == "Menu-opening")
            return true;
        else if (Physics.Raycast(theRay, out RaycastHit hit2, range) && hit2.collider.GetComponent<VideoController>().IsExitButton)
        {
            inMenu = false;
            hit2.collider.GetComponent<VideoController>().PlayVideo();
            return false;
        }
        else 
            return false;
    }

    GameObject GetCollidedObject(Ray theRay)
    {
        Physics.Raycast(theRay, out RaycastHit hit, range);
        return hit.transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward;
        Vector3 rayLength = direction * range;
        var start = transform.position;
        var finish = transform.TransformDirection(rayLength);
        Ray theRay = new Ray(start, finish);
        Debug.DrawRay(start, finish);

        FirstViewRotateMouse();
        if (!inMenu) //We are not in menu
        {
            if (RaycastPointingSelector(theRay)) //If raycast is hitting menu
            {
                holdMenuTime += Time.deltaTime;
                print("Entering in: " + holdMenuTime);
                if (MenuEntryFulfilled()) //And we fullfill elapse time
                {
                    inMenu = true;
                    print("In menu"); //PlayVideo()
                    GetCollidedObject(theRay).GetComponent<VideoController>().PlayVideo();
                }
            }
            else //If we do not fullfill elapse time still, but are willing to
            {
                holdMenuTime = 0;
                print("Menu entry cancelled");
            }
        }
        else
        {
            if (RaycastPointingSelector(theRay)) //If raycast is hitting menu
            {
                holdMenuTime -= Time.deltaTime;
                print("Leaving in: " + holdMenuTime);
                if (MenuExitFulfilled()) //And we fullfill elapse time
                {
                    inMenu = false;
                    print("Escaped menu");
                }
                else //If we do not fullfill elapse time still, but are willing to
                {
                    holdMenuTime = 0;
                    print("Menu exit cancelled");
                }
            }
        }
    }
}