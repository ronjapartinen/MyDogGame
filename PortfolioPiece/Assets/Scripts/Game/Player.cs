using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Threading;
using TMPro;


namespace Game
{
    public class Player : MonoBehaviour
    {
        public Vector3 startPosition = new Vector3(0, 2, 0);
        public Pet pet;

        private float speed = 7;
        private float rotateAmount;

        public Vector3 currentPos;
        
        void Start()
        {
            pet = GameManager.Instance.pet;
           
        }

        void Update()
        {     
            currentPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            // Move player on the forward axis on key press
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * speed * Time.deltaTime;
               
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(pet.OnWhistle());
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                pet.OnStay();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                pet.OnRelease();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(pet.OnSpin());
            }

            // Set rotateAmount which value is defined by which key is pressed
            rotateAmount = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ? rotateAmount + 10 : Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ? rotateAmount - 10 : rotateAmount;
            // Create a target rotation with angleaxis and rotate player with slerp using it
            Quaternion addRotation = Quaternion.AngleAxis(rotateAmount, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, addRotation, 2 * Time.deltaTime);           
        }


    }
}