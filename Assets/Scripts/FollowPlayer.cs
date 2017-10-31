using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {


    void LateUpdate() {
        transform.position = PlayerCommon.instance.transform.position;
    }
}
