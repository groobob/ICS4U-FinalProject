/*
 * Handles player camera. Has camera shake. Moves with the position of the mouse.
 * 
 * @author Evan
 * @version January 09
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    Vector3 target, mousePos, refVel, shakeOffset;
    [SerializeField] private float cameraDist = 2f;
    private float smoothTime = 0.2f, zStart;
    private float shakeMag, shakeTimeEnd;
    private Vector3 shakeVector;
    private bool shaking;
    private void Start()
    {
        target = _player.position;
        zStart = transform.position.z;
    }
    private void FixedUpdate()
    {
        mousePos = CaptureMousePos();
        shakeOffset = UpdateShake();
        target = UpdateTargetPos();
        UpdateCameraPosition();
    }
    /**
     * Method initiates a shake
     * @param direction Direction to begin the shake
     * @param magnitude How hard to shake
     * @param length Duration of Shake
     * 
     */
    public void Shake(Vector3 direction, float magnitude, float length)
    {
        shaking = true; // Set the value to true. This allows the script to know the camera is shaking
        shakeVector = direction; // Set direction
        shakeMag = magnitude; //Set magnitude
        shakeTimeEnd = Time.time + length; // Set the time where the shake ends
    }

    /**
     * Method for setting the tranform for the camera to follow
     * @param character The Transform of the player camera
     */
    public void setPlayerCamera(Transform charater)
    {
        _player = charater;
    }

    /**
     * Method that returns the current mouse position relative to the center of the screen
     * @return Vector3
     */
    private Vector3 CaptureMousePos() {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition); // Obtain the coordinates of the mouse

        //Following Section makes the coordinates a relative offset to the center of the screen
        ret *= 2;
        ret -= Vector2.one;
        //Does something about preventing the screen from extending a little further when placed at a diagonal
        float max = 0.9f;
        if (Mathf.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }
    /**
     * Method that returns the new position for the camera
     * @return Vector3
     */
    private Vector3 UpdateTargetPos() {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = _player.position + mouseOffset;
        ret += shakeOffset;
        ret.z = zStart;
        return ret;
    }
    /**
     * Moves the camera to the new position on screen.
     */
    private void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }
    
    /**
     * Method that runs every update(). It causes the screen to shake.
     */
    private Vector3 UpdateShake()
    {
        if (!shaking || Time.time > shakeTimeEnd) // If we aren't shaking or if we are done shaking
        {
            shaking = false;
            return Vector3.zero;
        }
        Vector3 tempOffset = shakeVector;
        tempOffset *= shakeMag;
        return tempOffset;
    }
}
