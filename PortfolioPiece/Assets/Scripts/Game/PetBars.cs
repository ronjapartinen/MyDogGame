using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



namespace Game
{
    public class PetBars : MonoBehaviour
    {

        private Slider slider;

        void Start()
        {
            slider = GameManager.Instance.slider;
        }


        void Update()
        {
   

        }

        public void UpdateSlider(float value)
        {
            if (slider == null) Debug.Log("hmm");
            slider.value = value;
        }
    }

}
