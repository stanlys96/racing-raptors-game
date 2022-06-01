using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("MainPlayer");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.transform.position;
        }
    }
}
