using UnityEngine;
using System.Collections;

/// <summary>
/// This script move one object randomly at x and z.
/// </summary>
/// ######################################################
/// Author: Luigi Garcia
/// - Email: mr.garcialuigi@gmail.com
/// - Linkedin: http://br.linkedin.com/in/garcialuigi
/// - Github:  https://github.com/garcialuigi
/// - Facebook: https://www.facebook.com/mr.garcialuigi
/// ######################################################
public class WanderMovement : MonoBehaviour {

    public float speed; // inspector

    private float timeOut = 0;
    private Vector3 movement;

	void Update () {
        if (timeOut <= 0)
        {
            timeOut = 2;
            movement.x = Random.Range(0, 2) == 0 ? 1 : -1;
            movement.z = Random.Range(0, 2) == 0 ? 1 : -1;
            movement.y = 0;
        }
        else {
            timeOut -= Time.deltaTime;
            transform.Translate(movement * speed, Space.World);
        }
	}
}
