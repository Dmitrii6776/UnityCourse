using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class MoveObject : MonoBehaviour
{
    [SerializeField] Vector3 movePisition;
    [SerializeField] float moveSpeed;
    [Range(0f, 1f)]
    [SerializeField]  float moveProgress;
    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveProgress = Mathf.PingPong(moveSpeed * Time.time, 1);
        Vector3 offSet = movePisition * moveProgress;
        transform.position = startPosition + offSet;
    }
}
