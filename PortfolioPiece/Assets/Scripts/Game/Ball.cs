using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Game
{
    public class Ball : MonoBehaviour
    {
        private bool holdingBall = false;

        public Rigidbody rb;
        private float startTime, endTime;
        private Vector3 startPos, endPos;
        private float swipeDistance, swipeTime;
        private float speed;
        private float maxSpeed = 40;
        private float velocity;
        private Vector3 angle;

        private void ResetBall()
        {
            angle = Vector3.zero;
            startPos = Vector3.zero;
            endPos = Vector3.zero;
            speed = 0;
            startTime = 0;
            endTime = 0;
            swipeDistance = 0;
            rb.velocity = Vector3.zero;
        }
        public void PickUpBall()
        {          
            GameManager manager = GameManager.Instance;
            rb = gameObject.GetComponent<Rigidbody>();
            // Set position for the ball infront of the player
            transform.position = GameManager.Instance.player.currentPos + manager.player.transform.forward * 3 + manager.player.transform.up * 2;
            rb.useGravity = false;
            holdingBall = true;                 
        }
        private void CallAngle()
        {
            // Create angle vector based on mouse position when released the mouse button
            angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x, endPos.y + 50, (Camera.main.nearClipPlane + 5)));
        }
        private void CallSpeed()
        {
            // Set speed with velocity. Velocity using swipe distance and time
            if(swipeTime > 0)
            {
                velocity = swipeDistance / (swipeDistance / swipeTime);
                speed = velocity * 40;
                if(speed >= maxSpeed)
                {
                    speed = maxSpeed;
                }
                if(speed < maxSpeed)
                {
                    speed += 40;
                }
                swipeTime = 0;
            }
        }
        void Update()
        {
            //Set start time and position, when mouse is down on the ball
            if (holdingBall)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 1000))
                    {
                        // check if ray hit is this ball
                        if (hit.transform == transform)
                        {
                            //Set startTime to current time and startPos to mouse position
                            startTime = Time.time;
                            startPos = Input.mousePosition;
                        }
                    }
                }
                //Get swipe time between mouse down and mouseup and distance of mouse moving while down
                else if (Input.GetMouseButtonUp(0))
                {
                    endTime = Time.time;
                    endPos = Input.mousePosition;
                    swipeDistance = (endPos - startPos).magnitude;
                    swipeTime = endTime - startTime;

                    //Move the balls rigidbody with AddForce using the angle and the speed
                    if (swipeTime < 5 && swipeDistance > 30)
                    {
                        CallSpeed();
                        CallAngle();
                        rb.AddForce(new Vector3((angle.x * speed), (angle.y * speed / 3), (angle.z * speed) * 2));
                        rb.useGravity = true;
                        Invoke("ResetBall", 4);
                        StartCoroutine(GameManager.Instance.pet.OnFetch());
                    }
                    else 
                    {
                        ResetBall();
                    }
                }
            }
        }
    }

}
