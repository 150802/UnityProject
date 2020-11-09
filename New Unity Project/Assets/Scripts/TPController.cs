using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class TPController : MonoBehaviour
{
    //Untitled variables
    public bool boarded = true;
    public Transform currentBody;
    public Transform topGoliath;
    public Transform bottomGoliath;
    public Transform body;
    public GameObject bodyGO;
    public GameObject goliath;

    //All the variables for player movement
    public CharacterController controller;
    public Animator animator;
    public Animator playerAnimator;
    public float speed = 5f;
    public float turnSmoothTime = 200f;
    public float gravity = 9.8f;
    public float jumpHeight = 2;
    bool isGrounded = true;
    float turnSmoothVelocity;
    Vector3 velocity;

    //All variables for camera movement
    public Transform cameraLeader;
    public Transform cameraCentre;
    public Transform userCamera;
    public float mouseSensitivity = 0.5f;
    public float maxCamDistance = 10f;
    public float minCamDistance = 1f;
    public float cameraSideOffset = 5f;
    public float camDistance = 5f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Frame variables
        float groundDistance = controller.height / 2 + 1f;
        float HorizontalInput = Input.GetAxisRaw("Horizontal") * mouseSensitivity;
        float VerticalInput = Input.GetAxisRaw("Vertical") * mouseSensitivity;
        RaycastHit hit;
        Vector3 direction = new Vector3(HorizontalInput, 0f, VerticalInput).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraCentre.eulerAngles.y;
        float targetAngleBody = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraCentre.localEulerAngles.y;
        float angle = Mathf.SmoothDampAngle(currentBody.localEulerAngles.y, targetAngleBody, ref turnSmoothVelocity, turnSmoothTime);

        //print(isGrounded);

        if (boarded == true) {
            currentBody = bottomGoliath;
            bodyGO.SetActive(false);
        } else {
            currentBody = body;
            bodyGO.SetActive(true);
        }

        if (Input.GetKeyDown("f")) {
            if (isGrounded == true) {
                boarded = !boarded;
                if (boarded == false)
                {
                    goliath.transform.parent = null;
                    controller.height = 1f;
                    controller.radius = 1f;
                    animator.SetBool("isWalking", false);
                    maxCamDistance = 7f;
                    cameraSideOffset = 2.5f;
                }
                else
                {
                    controller.height = 5f;
                    controller.radius = 1.61f;
                    goliath.transform.SetParent(transform, false);
                    goliath.transform.localPosition = new Vector3(0f, 1.54f, 0f);
                    goliath.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    maxCamDistance = 12f;
                    cameraSideOffset = 5f;
                }
            }
        }

        //Handles user control
        if (direction.magnitude >= 0.1) {
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            if (boarded == true) {
                if (isGrounded == true) {
                    animator.SetBool("isWalking", true);
                } else {
                    animator.SetBool("isWalking", false);
                }
                bottomGoliath.localRotation = Quaternion.Euler(0, angle, 0);
            } else {
                if (isGrounded == true)
                {
                    playerAnimator.SetBool("isWalking", true);
                }
                else
                {
                    playerAnimator.SetBool("isWalking", false);
                }
            }
        } else {
            animator.SetBool("isWalking", false);
            playerAnimator.SetBool("isWalking", false);
        }

        //Handles jumping
        if (Physics.Raycast(transform.position, -transform.up, groundDistance)) {
            if (Input.GetButtonDown("Jump")) {
                velocity.y = jumpHeight;
                isGrounded = false;
            }
            isGrounded = true;
        } else {
            //Puls the player down as gravity.
            velocity.y += -gravity * Time.deltaTime;
            isGrounded = false;
        }
        controller.Move(velocity * Time.deltaTime);

        //Handles camera movement
        if (Physics.Raycast(cameraLeader.position, -cameraLeader.forward, out hit, maxCamDistance)) {
            if (hit.distance < minCamDistance) {
                camDistance = minCamDistance;
            }
            else if (hit.distance > maxCamDistance) {
                camDistance = maxCamDistance;
            }
            else {
                camDistance = hit.distance;
            }
        } else {
            camDistance = maxCamDistance;
        }

        //calculates and processes the mouse/controller input
        float camX = Input.GetAxis("MouseX") * mouseSensitivity * -1 * Time.deltaTime;
        float camY = Input.GetAxis("MouseY") * mouseSensitivity * Time.deltaTime;
        xRotation -= camY;
        yRotation -= camX;
        xRotation = Mathf.Clamp(xRotation, -80f, 90f);

        //Changes the camera distance from the character to not go through other assets
        userCamera.localPosition = new Vector3(0f, 0f, -(camDistance -= 0.1f));

        //rotate the player body to face either the movement direction or the way the camera is pointing
        if (Input.GetButton("Fire2") || Input.GetButton("Fire1")) {
            cameraLeader.localPosition = new Vector3(cameraSideOffset, 0, 0);
            if (boarded == true) {
                topGoliath.localRotation = Quaternion.Euler(0, yRotation, 0);
                if (direction.magnitude >= 0.1) {
                    bottomGoliath.localRotation = Quaternion.Euler(0, angle, 0);
                }
            } else {
                body.localRotation = Quaternion.Euler(0, yRotation, 0);
            }
        } else {
            cameraLeader.localPosition = new Vector3(0, 0, 0);
            if (boarded == true) {
                if (direction.magnitude >= 0.1) {
                    topGoliath.localRotation = Quaternion.Euler(0, angle, 0);
                    bottomGoliath.localRotation = Quaternion.Euler(0, angle, 0);
                }
            } else {
                if (direction.magnitude >= 0.1) {
                    body.localRotation = Quaternion.Euler(0, angle, 0);
                }
            }
        }

        //Rotates the camera by mouse/controller input
        cameraLeader.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        cameraCentre.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}