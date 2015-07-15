using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

/// <summary>
/// Script that controll the camera movements.
/// It has two possible states: Following the player, and manual controll.
/// You can setup the camera values at the inspector.
/// </summary>
/// ######################################################
/// Author: Luigi Garcia
/// - Email: mr.garcialuigi@gmail.com
/// - Linkedin: http://br.linkedin.com/in/garcialuigi
/// - Github:  https://github.com/garcialuigi
/// - Facebook: https://www.facebook.com/mr.garcialuigi
/// ######################################################
public class MyCamera : MonoBehaviour
{
    #region Inspector

    // references
    public GameObject thePlayer; // the player reference, inspector
    public GameObject focusPoint; // the focus point, inspector
    public GameObject cameraObject; // the camera object reference
    // pan
    public float panSpeed;
    // zoom
    public float zoomSpeed; // the zoom speed multiplier
    public float zoomMaxLimit; // max limit value(distance between camera and focuspoint)
    public float zoomMinLimit; // min limit value(distance between camera and focuspoint)
    // rotate around
    public float rotateAroundSpeed;
    // follow
    public float followSmoothSpeed; // used to lerp the follow

    #endregion

    #region Other Variables

    private MyCameraStatusEnum status; // holds the current status

    #endregion

    #region MonoBehavior Methods

    private void Start()
    {
        status = MyCameraStatusEnum.AT_PLAYER; // the camera must start at AT_PLAYER status
    }

    private void Update()
    {
        switch (status)
        {
            case MyCameraStatusEnum.AT_PLAYER:
                FollowControll();
                RotateAroundControll();
                ZoomControll();
                break;
            case MyCameraStatusEnum.MANUAL:
                ZoomControll();
                RotateAroundControll();
                PanControll();
                break;
            default:
                break;
        }

        // Refresh the focus point position
        RefreshFocusPoint();

        // call this method to refresh the status.
        // this method contains rules to change between states
        UpdateStatus();
    }

    #endregion

    #region Controlls

    /// <summary>
    /// Refresh the focus point position, doing a raycast to put it on the center of the camera view. 
    /// TODO: Optimize this, we dont need to do a raycast each frame.
    /// </summary>
    private void RefreshFocusPoint()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, cameraObject.transform.forward, out hitInfo, 100))
        {
            focusPoint.transform.position = hitInfo.point;
        }
    }

    /// <summary>
    /// Does the follow behavior.
    /// </summary>
    private void FollowControll()
    {
        // calculate the goal
        Vector3 goal = thePlayer.transform.position; // get the player position

        // now we do the reverse engineering. Adding units of vectors to the goal, based on the cameraobject -forward, until it reachs the current
        // y of the camera, to discovery the position to follow
        // ps: this stuff avoid problems with the follow, when zoom is different than default.
        // TODO: Look for a better solution to do this. Maybe use distance from focus point as reference.
        while (goal.y < transform.position.y)
        {
            goal += -cameraObject.transform.forward * 0.5f;
        }

        // do the movement
        transform.position = Vector3.Lerp(transform.position, goal, Time.deltaTime * followSmoothSpeed);
    }

    /// <summary>
    /// Does the pan behavior.
    /// </summary>
    private void PanControll()
    {
        Vector3 movement = Vector3.zero;
        movement.x = InputManager.instance.GetPanAxis().x;
        movement.z = InputManager.instance.GetPanAxis().y;
        transform.Translate(movement * Time.deltaTime * panSpeed, Space.Self); // move based to self space.
    }

    /// <summary>
    /// Does the rotate around behavior based to the focus point.
    /// </summary>
    private void RotateAroundControll()
    {
        if (InputManager.instance.GetRotateAroundRight())
        {
            transform.RotateAround(focusPoint.transform.position, Vector3.up, -rotateAroundSpeed * Time.deltaTime);
        }
        else if (InputManager.instance.GetRotateAroundLeft())
        {
            transform.RotateAround(focusPoint.transform.position, Vector3.up, rotateAroundSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Does the zoom behavior.
    /// </summary>
    private void ZoomControll()
    {
        float axis = InputManager.instance.GetZoomInputAxis();

        if (Vector3.Distance(transform.localPosition, focusPoint.transform.position) > zoomMinLimit && axis > 0 ||
            Vector3.Distance(transform.localPosition, focusPoint.transform.position) < zoomMaxLimit && axis < 0
          )
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, focusPoint.transform.position, axis * Time.deltaTime * zoomSpeed);
        }
    }

    /// <summary>
    /// This method is called at the end of Update.
    /// This method have rules to change between camera states.
    /// </summary>
    private void UpdateStatus()
    {
        // rule 1: AT_PLAYER to MANUAL
        if (status == MyCameraStatusEnum.AT_PLAYER && InputManager.instance.GetPanAxis() != Vector2.zero)
        {
            status = MyCameraStatusEnum.MANUAL;
        }
        // rule 2: MANUAL to AT_PLAYER
        else if (status == MyCameraStatusEnum.MANUAL && InputManager.instance.GetJumpBackToPlayer())
        {
            status = MyCameraStatusEnum.AT_PLAYER;
        }
    }

    #endregion
}