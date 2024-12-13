using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using static UnityEngine.GraphicsBuffer;


namespace Game
{
    public class Pet : PetBase
    {

        public Vector3 startPosition = new Vector3(10, 2, 10);
        public float speed = 3;
        public Player player;

        private float moveValue;

        public float startFollowingDistance = 5;
        public float stopFollowingDistance = 40;
        public float maxWhistleDistance = 40;

        private bool stay = false;
        private bool spin;
        public float distance;

        public bool hasBall = false;
        
        private float tailMovement = 10;
        private Transform tail;

        private GameManager manager;

        protected void Start()
        {
            base.Start();
            manager = GameManager.Instance;
            player = manager.player;
            tail = transform.Find("Body/Tail");
        }

        protected void Update()
        {
            base.Update();
           if(player != null && !sleeping)
            {
                Vector3 currentPos = transform.position;
                distance = Vector3.Distance(currentPos, player.currentPos);
                bool tooCloseToPlayer = distance < startFollowingDistance ? true : false;
                bool tooFarToPlayer = distance > stopFollowingDistance ? true : false;

                Vector3 lookPos = player.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);

                moveValue = !tooCloseToPlayer && !tooFarToPlayer ? speed : 0;
                if (!spin)
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);
                if (!stay)
                    transform.position += transform.forward * moveValue * Time.deltaTime;

                //When pet is close to player, move tail 20 degrees to each direction overtime with lerp
                if (distance <= 5)
                {
                    hasBall = false;
                    float value = Mathf.Round(tail.transform.localEulerAngles.z);
                    if (value == 20)
                    {
                        tailMovement = -10;
                    }
                    if (value == 340)
                    {
                        tailMovement = 10;
                    }

                    Vector3 tailAngle = tail.transform.localEulerAngles + new Vector3(0, 0, tailMovement);
                    tail.transform.localEulerAngles = Vector3.Lerp(tail.transform.localEulerAngles, tailAngle, 1);
                }

                if (hasBall)
                {
                    Vector3 ballPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 4);
                    Game.GameManager.Instance.ball.transform.position = ballPos;                  
                }

                if(distance > 20)
                {
                    manager.HideActionButtons();
                }
            }
         
        }

        public IEnumerator OnWhistle()
        {
            if (manager.staying)
            {
                manager.OnStayButton();
            }

            if(distance < maxWhistleDistance && !sleeping)
            {              
                    Vector3 startPosition = transform.position;
                    Vector3 targetPosition = player.currentPos + player.transform.forward * 5;
                    targetPosition.y = transform.position.y;
                
                    float timeElapsed = 0f; 

                 
                    while (timeElapsed < 5)
                    {
                       
                        transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / 5);

                        timeElapsed += Time.deltaTime; 
                        yield return null; 
                    }
                }          
        }

        public IEnumerator OnSpin()
        {
            
            float timeElapsed = 0f;
            while (timeElapsed < 5)
            {
                spin = true;
                transform.Rotate(Vector3.up, 360f * Time.deltaTime / 5);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            spin = false;
        }

        public void OnStay()
        {
            stay = true;
        }

        public void OnRelease()
        {
            stay = false;
        }

        public void OnSleep()
        {
            sleeping = true;
            CallSleep();
        }

        public void OnWakeUp()
        {
            sleeping = false;         
        }

        public void OnFeed(int amount)
        {
            Eat(amount);
        }

        public IEnumerator OnFetch()
        {
            float timeElapsed = 0f;

            while (timeElapsed < 5)
            {
                Vector3 lookPos = manager.ball.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 2);

                Vector3 startPosition = transform.position;
                Vector3 targetPosition = manager.ball.transform.position;
                targetPosition.y = transform.position.y;
                transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / 5);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.GetComponent<Ball>())
            {
                hasBall = true;       
            }
        }
    }

    }


