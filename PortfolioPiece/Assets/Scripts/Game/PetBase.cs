using StateMachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public abstract class PetBase : MonoBehaviour
    {

        private float maxEnergy = 100;
        private float maxFood = 100;

        protected float foodLevel;
        protected float energyLevel = 100;

        public bool sleeping = false;

        protected PetBars petBar;

        protected virtual void Start()
        {
            petBar = GameManager.Instance.petBar;
        }

        protected virtual void Update()
        {            
            //Set sliders screen position on top of the pet
            if (GameManager.Instance.slider != null)
            {
                Vector3 petPos = Camera.main.WorldToScreenPoint(GameManager.Instance.pet.transform.position);
                petPos.y += 200;
                GameManager.Instance.slider.transform.position = petPos;
            }         
            if (energyLevel >= 1)
            {
                energyLevel -= 0.1f;
            }

            petBar.UpdateSlider(energyLevel);
        }

        public void Eat(int food)
        {
            //Add energy if its not full and update slider value with it
            if(energyLevel < maxEnergy)
            {
                energyLevel += food;
            }       
            petBar.UpdateSlider(energyLevel);
        }
        public void CallSleep()
        {
            StartCoroutine(Sleep());
        }

        public IEnumerator Sleep()
        {
            float time = 0;
            //Add energy overtime while sleeping and update slider value with it
            while (sleeping || time > 1000)
            {
                if(energyLevel < maxEnergy)
                {
                    energyLevel += 1;
                    time += Time.deltaTime;
                    petBar.UpdateSlider(energyLevel);
                }
                yield return null;
            }
           
        }
    }
}
