using UnityEngine;
using System.Collections;
using UnityEditor;

public class Controller : MonoBehaviour
{
    //FIX THE RAYCAST FOR IS GROUNDED BEFORE YOU DO ANYTHING ELSE
    //ALSO currently the jump height is pretty much dependant on how long the player presses the button which is good but the player can only press when the character is grounded. So the longer time its grounded the higher it jumps.
    //Currently the controller has issues with the get button down and up and stuff. Its because its taking in the axis in the input axis. But I just want it to take the A button.

    public Camera cam;
    Rigidbody rbody;

    public bool canMove = true;
    public bool canJump = true;
    public bool canAbility = true;

    public Vector3 moveDirection = Vector3.zero;
    public float turnSpeed; //Make this private and set it.
    public float gravity = 0;

    Quaternion targetRotation = Quaternion.identity;

    public float aimSensitivity = 0.25f;

    public float moveSpeed;
    public float horiAirSpeed;
    public float vertAirSpeed = 1;

    public float jumpForce = 10;

    private float jumpTime = 2;
    private float jumpTimeCounter;
    private bool stoppedJumping;

    float currentAccel = 1;
    public float maxAccel = 3;
    public float minAccel = 1;
    public float accelRate = 0.01f;

    private bool grounded;
    private float distanceToCheck = 0.55f;
    private Vector3 startPosition = new Vector3(0, 0.5f, 0);

    private bool jumpDown;
    private bool jumpUp;

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
    string jumpInput = "Jump";
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
        jumpTimeCounter = jumpTime;
    }

    void Update()
    {
        grounded = IsGrounded();
        if (canJump && grounded)
        {
            jumpTimeCounter = jumpTime;
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxis(jumpInput) <= 0 && !jumpDown)
        {
            jumpDown = true;
        }

        if (Input.GetAxis(jumpInput) > 0 && !jumpUp)
        {
            jumpUp = true;
        }

        //Character Jumping
        //If the character is touching the ground, can jump and is pressing the jump input, then add to their jump value by jump gain.
        if (Input.GetAxis(jumpInput) > 0 && jumpDown)
        {
            if (canJump && grounded)
            {
                //moveDirection.y = jumpForce;
                rbody.AddForce(new Vector3(0, jumpForce, 0));
                stoppedJumping = false;
                jumpDown = false;
            }
        }
        //if you keep holding down the mouse button...
        if ((Input.GetAxis(jumpInput) > 0) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if (jumpTimeCounter > 0)
            {
                //keep jumping!
                //moveDirection.y = jumpForce;
                rbody.AddForce(new Vector3(0,jumpForce,0));
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        if (Input.GetAxis(jumpInput) <= 0 && jumpUp)
        {
            //stop jumping and set your counter to zero.  The timer will reset once we touch the ground again in the update function.
            jumpTimeCounter = 0;
            stoppedJumping = true;
            jumpUp = false;
        }

        //Character Movement
        if (canMove)
        {
            //Create movement direction.
            moveDirection = new Vector3(Input.GetAxis(horizontalInput), 0, Input.GetAxis(verticalInput));
            if (grounded)
            {
                moveDirection *= moveSpeed * currentAccel;
            }
            else
            {
                moveDirection *= horiAirSpeed * currentAccel;
            }
            //moveDirection.y = jumpForce;
            moveDirection *= Time.deltaTime;

            //If there is movement this frame, turn the character to face that direction of the movement and increase the acceleration.
            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                //rbody.velocity = Vector3.zero;
                targetRotation = Quaternion.Euler(moveDirection);

                //Increase the acceleration while input is being pressed
                currentAccel += accelRate;

                if (currentAccel > maxAccel)
                {
                    currentAccel = maxAccel;
                }
                else if (currentAccel < minAccel)
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

            //Apply Movement Direction.
            rbody.velocity = moveDirection;

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

        //Character Abilities
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

    public void Knockback(Vector3 knockbackDirection, float knockbackforce, float knockbackTime)
    {
        //Restrict a direcction to a fixed position
        knockbackDirection = knockbackDirection.normalized;

        //Turn off player movement
        StartCoroutine(StopMovementForTime(knockbackTime));

        //Apply the knockback force in direction
        moveDirection = knockbackDirection * knockbackForce;
    }

    public IEnumerator StopMovementForTime(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    public IEnumerator StopAbilityForTime(float time)
    {
        canAbility = false;
        yield return new WaitForSeconds(time);
        canAbility = true;
    }

    public bool IsGrounded()
    {
        //return Physics.Raycast(transform.position, -Vector3.up, distanceToCheck);
        return Physics.CheckSphere(transform.position - startPosition, distanceToCheck);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - distanceToCheck, transform.position.z));
        Gizmos.DrawWireSphere(transform.position - startPosition, distanceToCheck);
    }
}

/*
#region Editor
[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Controller con = (Controller)target;
        Vector3 dir = new Vector3(0, 0, 0);
        float theforce = 1;
        float thetime = 1;

        //DrawDefaultInspector();


        if (GUILayout.Button("Knockback", EditorStyles.miniButtonMid))
        {
            con.Knockback(dir,theforce,thetime);
        }

        /*
            showCurrent = EditorGUILayout.ToggleLeft("Show Current Health", showCurrent);
            EditorGUILayout.Space();
            stats.totalArmor = EditorGUILayout.IntField("Total Armor", stats.totalArmor);
            if (showCurrent)
            {
                stats.currentArmor = EditorGUILayout.IntSlider("Current Armor", stats.currentArmor, 0, stats.totalArmor);
            }
            stats.totalShield = EditorGUILayout.IntField("Total Shield", stats.totalShield);
            if (showCurrent)
            {
                stats.currentShield = EditorGUILayout.IntSlider("Current Shield", stats.currentShield, 0, stats.totalShield);
            }
            stats.totalHealth = EditorGUILayout.IntField("Total Health", stats.totalHealth);
            if (showCurrent)
            {
                stats.currentHealth = EditorGUILayout.IntSlider("Current Health", stats.currentHealth, 0, stats.totalHealth);
            }
            EditorGUILayout.Space();
            stats.damageMultiplier = EditorGUILayout.Slider("Damage Multiplier", stats.damageMultiplier, 0.1f, 2);
            stats.healingMultiplier = EditorGUILayout.Slider("Healing Multiplier", stats.healingMultiplier, 0.1f, 2);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Armor", EditorStyles.boldLabel);
            stats.armorDecayAmount = EditorGUILayout.IntField("Armor Decay Amount", stats.armorDecayAmount);
            stats.armorDecayTime = EditorGUILayout.FloatField("Armor Decay Time", stats.armorDecayTime);
            stats.armorDamageFraction = EditorGUILayout.FloatField("Armor Damage Fraction", stats.armorDamageFraction);
            stats.armorRecoverFraction = EditorGUILayout.FloatField("Armor Recover Fraction", stats.armorRecoverFraction);
            stats.currentRecoverArmor = EditorGUILayout.IntSlider("Recover Armor", stats.currentRecoverArmor, 0, stats.totalArmor);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);
            healthValue = EditorGUILayout.IntField("Health Value", healthValue);
            damageDealt = EditorGUILayout.IntField("Damage Value", damageDealt);
            if (GUILayout.Button("Do Damage", EditorStyles.miniButtonMid))
            {
                stats.RecoveryDamage(damageDealt);
            }
            
    }
}
#endregion
*/
