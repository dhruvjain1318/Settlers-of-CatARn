using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CornellTech
{
    public class DiceScript : MonoBehaviour
    {
        private Vector3 initPos;
        private object initXpose;
        private int diceCount;

        [SerializeField]
        protected GameObject diceObject;



        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {


            if (Input.GetMouseButtonDown(0))
            {
                //initial click to roll a dice
                initPos = Input.mousePosition;

                //return x component of dice from screen to view point
                initXpose = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
            }

            //current position of mouse
            Vector3 currentPos = Input.mousePosition;

            //get all position along with mouse pointer movement
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(currentPos.x, currentPos.y, Mathf.Clamp(currentPos.y / 10, 10, 50)));

            //translate from screen to world coordinates  
            newPos = Camera.main.ScreenToWorldPoint(currentPos);

            if (Input.GetMouseButtonUp(0))
            {
                initPos = Camera.main.ScreenToWorldPoint(initPos);

                //Method use to roll the dice
                RollTheDice(newPos);
                //use identify face value on dice
                GetDiceCount();
            }


        }
        //Method Roll the Dice
        private void RollTheDice(Vector3 lastPos)
        {
            Vector3 rand_torque = new Vector3(Random.Range(100f, 1000f), Random.Range(100f, 1000f), Random.Range(100f, 1000f));
            diceObject.GetComponent<Rigidbody>().AddTorque(rand_torque, ForceMode.Impulse);
            lastPos.y += 12;
            diceObject.GetComponent<Rigidbody>().AddForce(((lastPos - initPos).normalized) * (Vector3.Distance(lastPos, initPos)) * 25 * diceObject.GetComponent<Rigidbody>().mass);
        }

        //Coroutine to get dice count
        private void GetDiceCount()
        {
            if (Vector3.Dot(transform.forward, Vector3.up) > 1)
                diceCount = 5;
            if (Vector3.Dot(-transform.forward, Vector3.up) > 1)
                diceCount = 2;
            if (Vector3.Dot(transform.up, Vector3.up) > 1)
                diceCount = 3;
            if (Vector3.Dot(-transform.up, Vector3.up) > 1)
                diceCount = 4;
            if (Vector3.Dot(transform.right, Vector3.up) > 1)
                diceCount = 6;
            if (Vector3.Dot(-transform.right, Vector3.up) > 1)
                diceCount = 1;
            Debug.Log("diceCount :" + diceCount);
        }


    }

}
