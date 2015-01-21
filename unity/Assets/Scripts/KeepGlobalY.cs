using UnityEngine;
using System.Collections;

/// ######################################################
/// Author: Luigi Garcia
/// - Email: mr.garcialuigi@gmail.com
/// - Linkedin: http://br.linkedin.com/in/garcialuigi
/// - Github:  https://github.com/garcialuigi
/// - Facebook: https://www.facebook.com/mr.garcialuigi
/// ######################################################

public class KeepGlobalY : MonoBehaviour {

    public float yValue;

	void Update () {
        gameObject.transform.position = new Vector3() {
            x = gameObject.transform.position.x,
            y = yValue,
            z = gameObject.transform.position.z
        };
	}
}
