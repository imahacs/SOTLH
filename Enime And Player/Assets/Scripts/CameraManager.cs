using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    public Transform Targettransform; //the ojb camera will follow
    public Transform cmeraPivot;
    public Transform cameraTransform;
    private float defaultPosition; //z posiotion 
    public LayerMask collitionLayers;
    public float cameraCollitionOffSet = 0.2f;
    public float cameraCollitionRadius = 0.2f;
    public float minoffset = 0.2f;

    private Vector3 cameraVectorPosition; 
    private Vector3 cameraFollowVelovicty = Vector3.zero;
    public float cameraFollowSpeed = 0.2f;
    public float cameraLookSpeed = 2;
    public float cameraPivotSpeed = 2;

    public float lookAngle;
    public float pivotAngle;
    public float minpivot = -35;
    public float maxpivot = 35;
    

    private void Awake()
    {
        inputManager = FindAnyObjectByType<InputManager>();
        Targettransform = FindAnyObjectByType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FllowPlayer();
        RotateCamera();
        //HandelCameraCollision();
    }

    private void FllowPlayer() 
    {
        Vector3 targetPos = Vector3.SmoothDamp(transform.position, Targettransform.position, ref cameraFollowVelovicty, cameraFollowSpeed);

        transform.position = targetPos;  // called in late update 
            
            
    }

    private void RotateCamera()
    {
        // lookAngle = lookAngle +(joyStickInput) example

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minpivot, maxpivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetrotation = Quaternion.Euler(rotation);
        transform.rotation = targetrotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetrotation = Quaternion.Euler(rotation);
        cmeraPivot.localRotation = targetrotation; // in local rotation means acuall rotation not in the world 


           
    }

    private void HandelCameraCollision()
    {
        float targetPosion = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cmeraPivot.position;
        direction.Normalize();

        if(Physics.SphereCast(cmeraPivot.transform.position, cameraCollitionRadius, direction, out hit, Mathf.Abs(targetPosion), collitionLayers)){
            float distance = Vector3.Distance(cmeraPivot.position, hit.point);
            targetPosion =- (distance - cameraCollitionOffSet);
        }

        if(Mathf.Abs(targetPosion) < minoffset)
        {
            targetPosion = targetPosion - minoffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosion, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
