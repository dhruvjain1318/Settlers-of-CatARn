using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CornellTech
{
	public class BoardTile : MonoBehaviour
	{
		//Enums

		//Structs/classes

		//Readonly/const

		//Serialized
		[SerializeField]
		public TextMesh textMesh;

        [SerializeField]
        public ParticleSystem myParticles;

        [SerializeField]
        public GameObject myThief;

        [SerializeField]
        public GameObject normalObj;
		
		/////Protected/////
		//References
		//Primitives
		protected bool hasGeneratedNumber = false;

        public int number;
		
		//Actions/Funcs

		//Properties

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//
		
		protected void Awake ()
		{
		}
		
		protected void Start ()
		{
            GenerateNumber();
		}
		
		protected void Update ()
		{	
            if(number == 7)
            {
                GenerateNumber();
            }
		}
		
		///////////////////////////////////////////////////////////////////////////
		//
		// BoardTile Functions
		//

		protected void GenerateNumber()
		{
			//TODO: Realistic number generation
			number = UnityEngine.Random.Range (3, 12);

			textMesh.text = number.ToString ();

			hasGeneratedNumber = true;
		}

		////////////////////////////////////////
		//
		// Event Functions

//		public void OnClicked()
//		{
//			if (!hasGeneratedNumber)
//			{
//				GenerateNumber ();
//			}

//		}

        public void ParticleToggle()
        {
            if (myParticles.isPlaying == true)
            {
                myParticles.gameObject.SetActive(false);
            }
            else
            {
                myParticles.gameObject.SetActive(true);
                myParticles.Play();
            }
        }
	}
}