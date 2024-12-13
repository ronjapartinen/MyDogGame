using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;


namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private GameObject playerPrefab;
        private GameObject petPrefab;
        public  Player player;
        public  Pet pet;

        public PetBars petBar;
        public Slider slider;

        public Button playButton;
        public Button whistleUpButton;

        public Button spinButton;
        public Button stayButton;
        public Button sleepButton;      
        public Button feedButton;
        public Button throwBallButton;

        private List<Button> actionButtons;
        public GameObject dogButtons;
        public GameObject startmenuCanvas;

        public GameObject ballPrefab;
        public Ball ball;

        public bool staying = false;
        public bool sleeping = false;

        public bool hideButtons = false;

        public static GameManager Instance;

        [SerializeField] public TextMeshPro petName;

        void Awake()
        {
            // Ensure there is only one instance of the GameManager
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }

        void Start()
        {
            actionButtons = new List<Button>() { stayButton, sleepButton, spinButton, throwBallButton, feedButton };

            playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
            player = playerPrefab.GetComponent<Player>();

            petPrefab = Resources.Load<GameObject>("Prefabs/Dog");
            pet = petPrefab.GetComponent<Pet>();

            ballPrefab = Resources.Load<GameObject>("Prefabs/Ball");
            ball = ballPrefab.GetComponent<Ball>();

            playButton.onClick.AddListener(OnPlayButton);
            spinButton.onClick.AddListener(OnSpinButton);
            stayButton.onClick.AddListener(OnStayButton);           
            sleepButton.onClick.AddListener(OnSleepButton);       
            feedButton.onClick.AddListener(OnFeedButton);
            throwBallButton.onClick.AddListener(OnThrowBallButton);
            whistleUpButton.onClick.AddListener(OnWhistleButton);

            HideActionButtons();
        }

   

        void Update()
        {
            // Set dogButtons canvas active on mouse down on the pet
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Camera.main != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        //Check if ray hit is our pet
                        if (hit.collider.tag == "Dog")
                        {
                            ShowActionButtons();
                        }
                    }
                }
            }           
        }

        public void OnWhistleButton()
        {
            if (pet != null)
            {
                StartCoroutine(pet.OnWhistle());
            }
        }

        public void OnSpinButton()
        {
            if(pet != null) StartCoroutine(pet.OnSpin());
        }

        public void OnStayButton()
        {
            if (pet != null && !staying)
            {
                staying = true;
                stayButton.GetComponentInChildren<TextMeshProUGUI>().text = "Release";
                pet.OnStay();
            }
            else if (pet != null && staying)
            {
                staying = false;
                stayButton.GetComponentInChildren<TextMeshProUGUI>().text = "Stay";
                pet.OnRelease();
            }
        }

  

        public void OnPlayButton()
        {          
            if(startmenuCanvas != null) startmenuCanvas.SetActive(false);

            //Instantiate player and pet, set reference to petBar and slider
            player = Instantiate(player, player.startPosition, Quaternion.identity);
            pet = Instantiate(pet, pet.startPosition, Quaternion.identity);          

            petBar = pet.GetComponent<PetBars>();
            slider = pet.GetComponentInChildren<Slider>();         
        }

        public void OnSleepButton()
        {

            if (pet != null && !sleeping)
            {
                sleeping = true;
                sleepButton.GetComponentInChildren<TextMeshProUGUI>().text = "Wake Up";
                pet.OnSleep();
            }

            else if (pet != null && sleeping)
            {
                sleeping = false;
                sleepButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sleep";
                pet.OnWakeUp();
            }
           
        }

        public void OnFeedButton()
        {
            if (pet != null) pet.OnFeed(10);
        }

        public void OnThrowBallButton()
        {
            //If not already ball in game, instantiate one infront of the player and call PickUpBall
            if (!GameObject.FindAnyObjectByType<Ball>())
            {
                ball = Instantiate(ball, player.currentPos + player.transform.forward * 3 + player.transform.up * 2, player.transform.rotation);
                ball.PickUpBall();
            }
            else ball.PickUpBall();
        }

        public void ShowActionButtons()
        {
            foreach(Button button in actionButtons)
            {
                button.gameObject.SetActive(true);
            }
        }

        public void HideActionButtons()
        {
            foreach (Button button in actionButtons)
            {
                button.gameObject.SetActive(false);
            }             
        }
    }
}
