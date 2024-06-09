using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    public class GroundCheck : MonoBehaviour
    {
        public CharacterMovement characterMovement;
        public LayerMask groundMask;
        private void Update()
        {
            characterMovement.groundCheck = Physics.CheckSphere(transform.position, 0.1f, groundMask);
        }
    }
}