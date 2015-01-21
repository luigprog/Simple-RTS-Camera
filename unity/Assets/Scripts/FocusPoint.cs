using UnityEngine;
using System.Collections;

/// <summary>
/// This class is used to refresh the focus point position.
/// This class do a raycast when the mouse left button is pressed, and refreshs the transform position of the object.
/// </summary>
/// ######################################################
/// Author: Luigi Garcia
/// - Email: mr.garcialuigi@gmail.com
/// - Linkedin: http://br.linkedin.com/in/garcialuigi
/// - Github:  https://github.com/garcialuigi
/// - Facebook: https://www.facebook.com/mr.garcialuigi
/// ######################################################
public class FocusPoint : MonoBehaviour {

	void Update () {
	    if(Input.GetMouseButtonDown(0)){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                transform.position = hitInfo.point;
            }
        }
	}
}
