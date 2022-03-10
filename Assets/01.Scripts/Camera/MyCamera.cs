using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MyCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObj = null;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private Vector3 rotate;
    [SerializeField]
    private Vector2 moveVal;
    [SerializeField]
    private float moveSpeed = 10f;
    [SerializeField]
    private bool isJump = false;
    [SerializeField]
    private float jumpPower = 5f;
    private bool _is_rotate_ready = false;

    void Update()
    {
        Vector3 targetPos = targetObj.transform.position;
        Vector3 camDir = targetPos - (targetPos + (Vector3.forward * distance));
        Vector3 camPos = Quaternion.Euler(rotate) * -camDir;
        transform.position = targetPos + camPos;
        transform.LookAt(targetPos);

        if(Vector2.zero != moveVal)
        {
            camDir.Normalize();
            targetObj.transform.position += Quaternion.Euler(rotate) * new Vector3(-moveVal.x,0.0f, -moveVal.y) * moveSpeed *Time.deltaTime; 
        }
        if(isJump)
        {
            targetObj.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, jumpPower, 0.0f));
        }
    }

    // 카메라 회전 처리
    public void RotateCam(InputAction.CallbackContext inContext)
    {
        if (!_is_rotate_ready) return;

        Vector2 inputAxis = inContext.ReadValue<Vector2>();
        float pitch = rotate.x + inputAxis.y;
        rotate.x += Mathf.Min(80.0f, Mathf.Abs(rotate.x)) * Mathf.Sign(pitch);
        rotate.y += inputAxis.x;
    }

    // 카메라 회전 준비 처리
    public void ReadyCameraRotate(InputAction.CallbackContext inContext)
    {
        _is_rotate_ready = inContext.performed;
    }

    // 타겟 이동
    public void MoveTarget(InputAction.CallbackContext inContext)
    {
        moveVal = inContext.ReadValue<Vector2>();
    }

    // 점프
    public void JumpTarget(InputAction.CallbackContext inContext)
    {
        isJump = inContext.ReadValue<bool>();
    }
}
