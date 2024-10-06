using Shared;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
namespace Client
{
    public class CharacterMovement : MonoBehaviour
    {
        private float gravity;
        public Vector3 gravityPower;
        public bool groundCheck;
        private bool isSprinting;
        private bool isCrouching;
        //Stats
        public PlayerStats stats;
        private PlayerStatManager playerStatManager;
        public Player player;
        private float lesserGravity;

        private CharacterController controller;
        [Header("Body parts")]
        public GameObject character;
        public GameObject body;

        void Start()
        {
            player = MainManager.mainPlayer;
            stats = player.stats;
            groundCheck = false;
            lesserGravity = -9;
            gravity = -27;
            CalculatePlayerStats();
        }

        void CalculatePlayerStats()
        {
            playerStatManager = MainManager.playerStatManager;
            controller = character.GetComponent<CharacterController>();
        }

        void Update()
        {
            UpdatePosition();
            MovementPlayer();
            Gravity();
            //apply last
            ApplyGravity();
            if (isSprinting)
            {
                playerStatManager.AddStamina(-3 * Time.deltaTime);
                if (stats.stamina <= 0)
                {
                    isSprinting = false;
                    playerStatManager.SetMovemendSpeed(stats.baseMovementSpeed);
                }
            }
            else
            {
                if (stats.stamina <= 100)
                {
                    playerStatManager.AddStamina(1 * Time.deltaTime);
                }
            }
        }

        public void SprintingDown()
        {
            playerStatManager.SetMovemendSpeed(stats.movementSpeed * 1.25f);
            isSprinting = true;
        }

        public void SprintingUp()
        {
            playerStatManager.SetMovemendSpeed(stats.baseMovementSpeed);
            isSprinting = false;
            if (isCrouching)
            {
                playerStatManager.SetMovemendSpeed(stats.movementSpeed * 0.5f);
            }
        }

        public void CrouchingDown()
        {
            playerStatManager.SetMovemendSpeed(stats.movementSpeed * 0.5f);
            isCrouching = true;
        }

        public void CrouchingUp()
        {
            playerStatManager.SetMovemendSpeed(stats.baseMovementSpeed);
            isCrouching = false;
            if (isSprinting)
            {
                playerStatManager.SetMovemendSpeed(stats.movementSpeed * 1.25f);
            }
        }

        void MovementPlayer()
        {
            Vector3 movementVector = body.transform.right * Input.GetAxis("Horizontal") + body.transform.forward * Input.GetAxis("Vertical");
            movementVector = stats.movementSpeed * Time.deltaTime * movementVector;
            controller.Move(movementVector);
        }

        public void Jump()
        {
            if (groundCheck)
                gravityPower.y = Mathf.Sqrt(stats.jumpPower * -2f * gravity);
        }

        void Gravity()
        {
            if (groundCheck && gravityPower.y < lesserGravity)
            {
                gravityPower.y = lesserGravity;
            }
            else
            {
                gravityPower.y += gravity * Time.deltaTime;
            }
        }

        void ApplyGravity()
        {
            controller.Move(gravityPower * Time.deltaTime);
        }

        public void HardSyncPlayerMovement() 
        {
            Vector3 currentPos = Converter.Vector3ToUnityVector3(player.stats.currentPosition);
            if (Vector3.Distance(transform.position, currentPos) > 30) 
            {
                transform.position = currentPos;
            }
        }

        public void UpdatePosition()
        {
            player.stats.clientPosition =Converter.Vector3UnityToVector3(transform.position);
        }
    }
}