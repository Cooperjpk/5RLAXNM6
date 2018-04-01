using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{
    /// <summary>
    /// To increase the variation between characters, movement will be highly varied between characters. This means that some characters may not have any escape abilities but they have high movement speed, or this could mean an extremely slow character might be given high mobility through their abilities.
    /// </summary>

    public Camera cam;
    Rigidbody rbody;

    public bool canMove = true;
    public bool canAbility = true;

    public Vector3 moveDirection = Vector3.zero;
    public float turnSpeed;
    public float gravity = 0;

    Quaternion targetRotation = Quaternion.identity;

    public float aimSensitivity = 0.25f;

    public float moveSpeed;
    float currentAccel = 1;
    public float maxAccel = 3;
    public float minAccel = 1;
    public float accelRate = 0.01f;

    public bool isInCombat = false;

    public void ToggleMove(bool active)
    {
        canMove = active;
    }

    public void ToggleAbility(bool active)
    {
        canAbility = active;
    }

    public enum InputType
    {
        Keyboard,
        Gamepad,
    }

    public InputType inputType = InputType.Keyboard;

    string horizontalInput = "Horizontal";
    string verticalInput = "Vertical";
    string aimHorizontalInput = "AimHorizontal";
    string aimVerticalInput = "AimVertical";
    string ability1Input = "Fire1";
    string ability2Input = "Fire2";
    string ability3Input = "Fire3";
    string ability4Input = "Fire4";
    string ability5Input = "Fire5";
    string ability6Input = "Fire6";
    string ability7Input = "Fire7";
    string ability8Input = "Fire8";

    public Ability ability1;
    public Ability ability2;
    public Ability ability3;
    public Ability ability4;
    public Ability ability5;
    public Ability ability6;
    public Ability ability7;
    public Ability ability8;

    //Knockback
    public float knockbackForce;
    public float knockbackTime;
    public float knockbackCounter;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    void Update()
    { 
        //Player Movement
        if (canMove)
        {
            //Create movement direction.
            moveDirection = new Vector3(Input.GetAxis(horizontalInput), 0, Input.GetAxis(verticalInput));
            moveDirection *= moveSpeed * currentAccel;
            moveDirection.y -= gravity;
            moveDirection *= Time.deltaTime;

            //If there is movement this frame, turn the character to face that direction of the movement.
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                //rbody.velocity = Vector3.zero;
                targetRotation = Quaternion.Euler(moveDirection);

                //Increase the acceleration while input is being pressed
                currentAccel += accelRate;

                if(currentAccel > maxAccel)
                {
                    currentAccel = maxAccel;
                }
                else if(currentAccel < minAccel)
                {
                    currentAccel = minAccel;
                }
            }
            else
            {
                currentAccel = 1;
            }

            //Apply turn rotation to the character at their turn speed.
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

            //Aim Controls
            if (inputType == InputType.Keyboard)
            {
                //Always look where the mouse is at all times.
                LookAtMousePosition();
            }
            else if (inputType == InputType.Gamepad)
            {
                if (Input.GetAxis(aimHorizontalInput) > aimSensitivity || Input.GetAxis(aimHorizontalInput) < -aimSensitivity || Input.GetAxis(aimVerticalInput) > aimSensitivity || Input.GetAxis(aimVerticalInput) < -aimSensitivity)
                {
                    float screenAngle = Mathf.Atan2(Input.GetAxis(aimHorizontalInput), Input.GetAxis(aimVerticalInput)) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, screenAngle, transform.eulerAngles.z);
                }
                //If there is no looking input, then look at the direction that you are moving
                else if (Input.GetAxis(horizontalInput) > aimSensitivity || Input.GetAxis(horizontalInput) < -aimSensitivity || Input.GetAxis(verticalInput) > aimSensitivity || Input.GetAxis(verticalInput) < -aimSensitivity)
                {
                    float screenAngle = Mathf.Atan2(Input.GetAxis(horizontalInput), Input.GetAxis(verticalInput)) * Mathf.Rad2Deg;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, screenAngle, transform.eulerAngles.z);
                }
            }
        }

        //Apply movement direction.
        rbody.velocity = moveDirection;

        //Player Abilities
        if (canAbility)
        {
            #region Ability 1
            //Use Ability 1
            if (Input.GetButtonDown(ability1Input) && ability1)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability1.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability1Input) && ability1)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability1.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability1Input) && ability1)
            {
                //Debug.Log("Using ability 1 UP!");
                ability1.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 2
            //Use Ability 2
            else if (Input.GetButtonDown(ability2Input) && ability2)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability2.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability2Input) && ability2)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability2.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability2Input) && ability2)
            {
                //Debug.Log("Using ability 1 UP!");
                ability2.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 3
            //Use Ability 3
            else if (Input.GetButtonDown(ability3Input) && ability3)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability3.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability3Input) && ability3)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability3.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability3Input) && ability3)
            {
                //Debug.Log("Using ability 1 UP!");
                ability3.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 4
            //Use Ability 4
            else if (Input.GetButtonDown(ability4Input) && ability4)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability4.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability4Input) && ability4)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability4.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability4Input) && ability4)
            {
                //Debug.Log("Using ability 1 UP!");
                ability4.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 5
            //Use Ability 5
            else if (Input.GetButtonDown(ability5Input) && ability5)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability5.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability5Input) && ability5)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability5.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability5Input) && ability5)
            {
                //Debug.Log("Using ability 1 UP!");
                ability5.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 6
            //Use Ability 6
            else if (Input.GetButtonDown(ability6Input) && ability6)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability6.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability6Input) && ability6)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability6.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability6Input) && ability6)
            {
                //Debug.Log("Using ability 1 UP!");
                ability6.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 7
            //Use Ability 7
            else if (Input.GetButtonDown(ability7Input) && ability7)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability7.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability7Input) && ability7)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability7.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability7Input) && ability7)
            {
                //Debug.Log("Using ability 1 UP!");
                ability7.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion

            #region Ability 8
            //Use Ability 8
            else if (Input.GetButtonDown(ability8Input) && ability8)
            {
                //Debug.Log("Using ability 1 DOWN!");
                ability8.InvokeAbility(Ability.AbilityInput.Down);
            }

            else if (Input.GetButton(ability8Input) && ability8)
            {
                //Debug.Log("Using ability 1 AGAIN!");
                ability8.InvokeAbility(Ability.AbilityInput.Hold);
            }
            else if (Input.GetButtonUp(ability8Input) && ability8)
            {
                //Debug.Log("Using ability 1 UP!");
                ability8.InvokeAbility(Ability.AbilityInput.Up);
            }
            #endregion
        }
    }

    void LookAtMousePosition()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, 1000000))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            transform.rotation = newRotation;
        }
    }

    public void Knockback(Vector3 direction,float force,float time)
    {
        //Restrict a direcction to a fixed position
        direction = direction.normalized;

        //Turn off player movement
        canMove = false;

        //Apply the knockback force in direction
        moveDirection = direction * knockbackForce;
    }
}


