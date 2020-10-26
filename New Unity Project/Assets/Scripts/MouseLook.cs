using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //central variables
    public float mouseSensitivity = 200f;
    public float maxCamDistance = 10f;
    public float minCamDistance = 1f;
    public float cameraSideOffset = 5f;
    public Transform cameraLeader;
    public Transform cameraCentre;
    public Transform userCamera;
    public Transform topPlayerBody;
    public Transform bottomPlayerBody;
    float xRotation = 0f;
    float yRotation = 0f;
    public float camDistance = 5f;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //frame variables
        RaycastHit hit;
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraLeader.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(bottomPlayerBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        //changes camera distance from the player to stop if from passing through walls
        if (Physics.Raycast(cameraLeader.position, -cameraLeader.forward, out hit, maxCamDistance))
        {

            if (hit.distance < minCamDistance)
            {
                camDistance = minCamDistance;
            }
            else if (hit.distance > maxCamDistance)
            {
                camDistance = maxCamDistance;
            }
            else
            {
                camDistance = hit.distance;
            }
        }
        else
        {
            camDistance = maxCamDistance;
        }

        //calculates and processes the mosue/controller input
        float camX = Input.GetAxis("MouseX") * mouseSensitivity * -1 * Time.deltaTime;
        float camY = Input.GetAxis("MouseY") * mouseSensitivity * Time.deltaTime;
        xRotation -= camY;
        yRotation -= camX;
        xRotation = Mathf.Clamp(xRotation, -80f, 90f);

        //Changes the camera distance from the character to not go through other assets
        userCamera.localPosition = new Vector3(0f, 0f, -(camDistance -= 0.1f));

        //rotate the player body to face either the movement direction or the way the camera is pointing
        if (Input.GetButton("Fire2") || Input.GetButton("Fire1"))
        {
            cameraLeader.localPosition = new Vector3(cameraSideOffset, 0, 0);
            topPlayerBody.localRotation = Quaternion.Euler(0, yRotation, 0);
        }
        else
        {
            if (direction.magnitude >= 0.1f)
            {
                topPlayerBody.localRotation = Quaternion.Euler(0, angle, 0);
            }
            cameraLeader.localPosition = new Vector3(0, 0, 0);
        }

        if (direction.magnitude >= 0.1f)
        {
            bottomPlayerBody.localRotation = Quaternion.Euler(0, angle, 0);
        }

        //rotates the camera by mouse/controller input
        cameraLeader.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cameraCentre.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
