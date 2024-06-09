using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private PlayerStatManager playerStatManager;
        private PlayerStatManager.Stats playerStats;
        private float lesserGravity;

        private CharacterController controller;
        [Header("Body parts")]
        public GameObject character;
        public GameObject body;

        void Start()
        {
            groundCheck = false;
            lesserGravity = -9;
            gravity = -27;
            CalculatePlayerStats();
        }

        void CalculatePlayerStats()
        {
            playerStatManager = MainManager.playerStatManager;
            playerStats = playerStatManager.stats;
            controller = character.GetComponent<CharacterController>();
        }

        void Update()
        {
            MovementPlayer();
            Gravity();
            //apply last
            ApplyGravity();
            if (isSprinting)
            {
                playerStatManager.AddStamina(-3 * Time.deltaTime);
                if (playerStats.stamina <= 0)
                {
                    isSprinting = false;
                    playerStatManager.SetMovemendSpeed(playerStats.baseMovementSpeed);
                }
            }
            else
            {
                //Shit does work
                if (playerStats.stamina <= 100)
                {
                    playerStatManager.AddStamina(1 * Time.deltaTime);
                }
            }
        }

        public void SprintingDown()
        {
            playerStatManager.SetMovemendSpeed(playerStats.movementSpeed * 1.25f);
            isSprinting = true;
        }

        public void SprintingUp()
        {
            playerStatManager.SetMovemendSpeed(playerStats.baseMovementSpeed);
            isSprinting = false;
            if (isCrouching)
            {
                playerStatManager.SetMovemendSpeed(playerStats.movementSpeed * 0.5f);
            }
        }

        public void CrouchingDown()
        {
            playerStatManager.SetMovemendSpeed(playerStats.movementSpeed * 0.5f);
            isCrouching = true;
        }

        public void CrouchingUp()
        {
            playerStatManager.SetMovemendSpeed(playerStats.baseMovementSpeed);
            isCrouching = false;
            if (isSprinting)
            {
                playerStatManager.SetMovemendSpeed(playerStats.movementSpeed * 1.25f);
            }
        }

        void MovementPlayer()
        {
            Vector3 movementVector = body.transform.right * Input.GetAxis("Horizontal") + body.transform.forward * Input.GetAxis("Vertical");
            movementVector = playerStats.movementSpeed * Time.deltaTime * movementVector;
            controller.Move(movementVector);
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

        public void Jump()
        {
            if (groundCheck)
                gravityPower.y = Mathf.Sqrt(playerStats.jumpPower * -2f * gravity);
        }

        void ApplyGravity()
        {
            controller.Move(gravityPower * Time.deltaTime);
        }
    }
}