using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.WSA.Input;

public class PlayerControll : MonoBehaviour
{
    [SerializeField] GameObject CameraHolder = null;
    [SerializeField] GameObject VrRig = null;
    [SerializeField] float Speed = 15;
    [SerializeField] XRController LeftHand = null;
    [SerializeField] XRController RightHand = null;
    Animator animator = null;
    float player_rotationX = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.Euler(0, CameraHolder.transform.eulerAngles.y, 0);
    }

    private void Update()
    {
        if (GameManager.Instance.GamePaused)
        {
            return;
        }
        MovePlayer();
        MoveCamera();
        //GetMenuClick();
    }

    private void GetMenuClick()
    {
        if (LeftHand.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool presses))
        {
            Debug.Log("click");
            //GameManager.Instance.OnGamePaused();
        }
    }

    void MovePlayer()
    {
        var inputY = Input.GetAxis("Vertical");
        var inputX = Input.GetAxis("Horizontal");
        if (inputX != 0 || inputY != 0)
        {
            WalkAnim(true);
        }
        else
        {
            WalkAnim(false);
        }
        transform.position += transform.forward * Speed * inputY * Time.deltaTime;
        transform.position += transform.right * Speed * inputX * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, CameraHolder.transform.eulerAngles.y, 0);
    }

    void MoveCamera()
    {
        VrRig.transform.position = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        var objName = collision.gameObject.tag;
        if (objName.Contains("Zombie"))
        {
            GameManager.Instance.HitEnemy();
        }
        else if (objName.Contains("Keys"))
        {
            PickUpAnim();
            GameManager.Instance.HitKeys(collision);
        }
        else if (objName.Contains("Door"))
        {
            GameManager.Instance.HitDoors(collision);
        }
        else if (objName.Contains("Saw"))
        {
            GameManager.Instance.HitSaw();
        }
        else if (objName.Contains("Score"))
        {
            PickUpAnim();
            GameManager.Instance.HitScore(collision);
        }
        else if (objName.Contains("Finish"))
        {
            PickUpAnim();
            GameManager.Instance.HitFinish(collision);
        }
    }

    void PickUpAnim()
    {
        animator.Play("pickup");
    }

    void WalkAnim(bool play)
    {
        if (play)
        {
            animator.Play("walk");
        }
        else
        {
            animator.Play("idle");
        }
    }
}