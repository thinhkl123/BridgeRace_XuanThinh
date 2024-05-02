using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    [SerializeField] private float speed = 5f;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Animator animator;

    private float moveHorizontal;
    private float moveVertical;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CalculateMove();
    }

    private void CalculateMove()
    {
        if (GameMainManager.instance.state != GameMainManager.GameState.Playing)
        {
            return;
        }
        //Keyboard
        moveHorizontal = Input.GetAxis(HORIZONTAL);
        moveVertical = Input.GetAxis(VERTICAL);

        //JoyStick
        moveHorizontal = joystick.Horizontal;
        moveVertical = joystick.Vertical;

        //Debug.Log(moveVertical + " " + moveVertical);

        Vector3 direction = new Vector3(moveHorizontal, 0f, moveVertical).normalized;
        Vector3 nextPos = TF.position + direction * speed * Time.deltaTime + Vector3.up * 2f;

        if (CanMove(nextPos))
        {
            TF.position = CheckNextPosition(nextPos);
        }

        if (direction != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(direction * speed * Time.deltaTime);
            visual.forward = direction;
        }

        animator.SetFloat("Speed", direction.magnitude);
    }
}
