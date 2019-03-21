using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace CornellTech
{
    public class BoardManager : MonoBehaviour
    {
        //Enums

        //Structs/classes

        //Readonly/const

        //Serialized
        [SerializeField]
        protected Button SelectTiles;

        [SerializeField]
        protected Button DoneSelect;

        [SerializeField]
        protected Button rollButton;

        [SerializeField]
        protected Button cityPlacer;

        [SerializeField]
        protected Button settlementPlacer;

        [SerializeField]
        protected Button roadPlacer;

        [SerializeField]
        protected Button plusButton;

        [SerializeField]
        protected Button minusButton;

        [SerializeField]
        protected List<BoardTile> allTiles;

        /////Protected/////
        //References
        //Primitives
        //for tile selection
        protected List<BoardTile> myTiles;
        private enum SelectState { Default, Selecting, Robber, Done };
        private SelectState ControlState;
        private bool inArray;

        //for rolling
        public int currentRoll;
        protected GameObject currentDice;

        //for scoreboard (and UI)
        protected TMP_Text rollText;
        protected TMP_Text victoryText;
        protected TMP_Text messageText;
        protected TMP_Text woodText;
        protected TMP_Text brickText;
        protected TMP_Text sheepText;
        protected TMP_Text wheatText;
        protected TMP_Text oreText;
        protected TMP_Text resourceText;

        public int victoryPoints = 0;
        public int totalResources;
        public int totalSettlements;
        public int totalCities;
        public int woodScore;
        public int oreScore;
        public int sheepScore;
        public int wheatScore;
        public int brickScore;
        public string selectedResource;

        //for spawning
        Vector3 spawnPoint;




        //Actions/Funcs

        //Properties

        ///////////////////////////////////////////////////////////////////////////
        //
        // Inherited from MonoBehaviour
        //

        protected void Awake()
        {
            rollText = GameObject.FindWithTag("RollText").GetComponent<TextMeshProUGUI>();
            victoryText = GameObject.FindWithTag("VictoryPoints").GetComponent<TextMeshProUGUI>();
            woodText = GameObject.FindWithTag("WoodScore").GetComponent<TextMeshProUGUI>();
            brickText = GameObject.FindWithTag("BrickScore").GetComponent<TextMeshProUGUI>();
            sheepText = GameObject.FindWithTag("SheepScore").GetComponent<TextMeshProUGUI>();
            wheatText = GameObject.FindWithTag("WheatScore").GetComponent<TextMeshProUGUI>();
            oreText = GameObject.FindWithTag("OreScore").GetComponent<TextMeshProUGUI>();
            messageText = GameObject.FindWithTag("MessageText").GetComponent<TextMeshProUGUI>();
            resourceText = GameObject.FindWithTag("TotalScore").GetComponent<TextMeshProUGUI>();
        }

        protected void Start()
        {
            SetStartScores();
            SelectTiles.onClick.AddListener(TileSelect);
            rollButton.onClick.AddListener(DiceRoll);
            settlementPlacer.onClick.AddListener(SpawnSettlement);
            cityPlacer.onClick.AddListener(SpawnCity);
            plusButton.onClick.AddListener(IncreaseResource);
            minusButton.onClick.AddListener(DecreaseResource);
            ControlState = SelectState.Default;
            myTiles = new List<BoardTile>();

        }

        protected void Update()
        {
            UpdateRaycast();
            UpdateScoreboard();
        }

        ///////////////////////////////////////////////////////////////////////////
        //
        // BoardManager Functions
        //
        // These are functions to control tile selection on the board

        protected void UpdateRaycast()
        {
            float distance = 100f;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                Debug.Log("Mouse position: " + mousePosition.ToString());

                mousePosition.z = distance;

                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                RaycastHit raycastHit;

                if (Physics.Raycast(ray, out raycastHit, distance))
                {
                    string name = raycastHit.collider.gameObject.name;
                    Debug.Log("We hit: " + name);
                    Debug.Log("It was " + raycastHit.distance + " meters away.");

                    BoardTile boardTile = raycastHit.collider.gameObject.GetComponent<BoardTile>();
                    //                  if (boardTile != null)
                    //                  {
                    //                      boardTile.OnClicked();
                    //                  }
                    if (ControlState == SelectState.Selecting)
                    {
                        boardTile.ParticleToggle();
                        if (myTiles.Contains(boardTile))
                        {
                            myTiles.Remove(boardTile);
                        }
                        else
                        {
                            myTiles.Add(boardTile);
                        }
                    }
                    if (ControlState == SelectState.Robber)
                    {
                        foreach (BoardTile checks in allTiles)
                        {
                            if (checks.myThief.activeSelf == true)
                            {
                                checks.myThief.SetActive(false);
                                checks.normalObj.SetActive(true);
                            }
                        }
                        boardTile.myThief.SetActive(true);
                        boardTile.normalObj.SetActive(false);
                        ControlState = SelectState.Default;
                    }
                }
            }
        }

        protected void TileSelect()
        {
            ControlState = SelectState.Selecting;
            DoneSelect.onClick.AddListener(DoneSelecting);
        }

        protected void DoneSelecting()
        {
            ControlState = SelectState.Done;
            foreach (BoardTile element in myTiles)
            {
                element.ParticleToggle();
            }
            foreach (BoardTile tiles in allTiles)
            {
                tiles.textMesh.text = null;
            }
            DoneSelect.onClick.RemoveListener(DefaultSelect);
        }

        protected void DefaultSelect()
        {
            ControlState = SelectState.Default;
        }


        ////////////////////////////////////////
        //
        // Event Functions
        //
        // These are functions for other buttons on the UI

        protected void SetStartScores()
        {
            int startScore = 4;

            woodScore = startScore;
            oreScore = startScore;
            sheepScore = startScore;
            wheatScore = startScore;
            brickScore = startScore;
        }
        protected void DiceRoll()
        {
            RespawnDice();
            GenerateRoll();
            DisplayRollText();
            if (currentRoll == 7)
            {
                MoveRobber();
            }
            else
            {
                HighlightResources();
            }
        }

        protected void RespawnDice()
        {

            if (currentDice)
            {
                Destroy(currentDice);
            }

            currentDice = Instantiate(Resources.Load("Prefabs/Double_Dice") as GameObject);
            currentDice.transform.position = new Vector3(-0.47f, 4.85f, -5.0786f);
            foreach (Rigidbody diebody in currentDice.GetComponentsInChildren<Rigidbody>())
            {
                diebody.AddForce(Vector3.up * 500);
                diebody.AddForce(Vector3.left * 250);
                diebody.AddTorque(Vector3.left);
            }

        }

        protected void GenerateRoll()
        {

            int roll1 = UnityEngine.Random.Range(1, 6);
            int roll2 = UnityEngine.Random.Range(1, 6);

            currentRoll = roll1 + roll2;

        }

        protected void MoveRobber()
        {
            ControlState = SelectState.Robber;
        }

        protected void DisplayRollText()
        {
            rollText.text = currentRoll.ToString();
            StartCoroutine(ClearInSeconds(10));
        }



        protected void HighlightResources()
        {
            foreach (BoardTile selection in myTiles)
            {
                if (currentRoll == selection.number)
                {
                    selection.myParticles.gameObject.SetActive(true);
                    selection.myParticles.Play();
                    selection.textMesh.text = selection.number.ToString();
                }
                else
                {
                    selection.myParticles.gameObject.SetActive(false);
                }
            }
        }

        protected void SpawnSettlement()
        {

            if (woodScore >= 1 && brickScore >= 1 && wheatScore >= 1 && sheepScore >= 1)
            {
                spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
                GameObject settlement = Instantiate(Resources.Load("Prefabs/Medieval_house") as GameObject);
                settlement.transform.position = spawnPoint;
                totalSettlements++;
                woodScore--;
                brickScore--;
                wheatScore--;
                sheepScore--;
            }
            else
            {
                String message = "You don't have enough resources to build a settlement!";
                //notify the player that there is not enough 
                messageText.text = message;
                StartCoroutine(ClearInSeconds(3));
            }

        }

        protected void SpawnCity()
        {
            if (oreScore >= 3 && wheatScore >= 2)
            {
                spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;
                GameObject city = Instantiate(Resources.Load("Prefabs/Medieval_house") as GameObject);
                city.transform.position = spawnPoint;
                totalCities++;
                oreScore -= 3;
                wheatScore -= 2;
            }
            else
            {
                String message = "You don't have enough resources to build a city!";
                //notify the player that there is not enough 
                messageText.text = message;
                StartCoroutine(ClearInSeconds(3));

            }
        }
        protected void UpdateScoreboard()
        {
            victoryPoints = totalSettlements + totalCities * 2;
            totalResources = woodScore + brickScore + wheatScore + sheepScore + oreScore;

            victoryText.text = victoryPoints.ToString();
            resourceText.text = totalResources.ToString();
            woodText.text = woodScore.ToString();
            brickText.text = brickScore.ToString();
            wheatText.text = wheatScore.ToString();
            sheepText.text = sheepScore.ToString();
            oreText.text = oreScore.ToString();

        }

        public void IncreaseResource(){
            Debug.Log("Button Worked!");
            Debug.Log(selectedResource);
            switch (selectedResource)
            {
                case null:
                    break;
                case "Wood":
                    woodScore++;
                    break;
                case "Brick":
                    brickScore++;
                    break;
                case "Wheat":
                    wheatScore++;
                    break;
                case "Sheep":
                    sheepScore++;
                    break;
                case "Ore":
                    oreScore++;
                    break;
            }
        }

        public void DecreaseResource()
        {
            Debug.Log("Button Worked!");
            Debug.Log(selectedResource);
            switch (selectedResource)
            {
                case null:
                    break;
                case "Wood":
                    woodScore--;
                    break;
                case "Brick":
                    brickScore--;
                    break;
                case "Wheat":
                    wheatScore--;
                    break;
                case "Sheep":
                    sheepScore--;
                    break;
                case "Ore":
                    oreScore--;
                    break;
            }
        }

        public void SelectResource(string resource){
            selectedResource = resource;
        }

        protected IEnumerator ClearInSeconds(int seconds)
        {

            yield return new WaitForSeconds(seconds);
            messageText.text = "";
            rollText.text = "";
        }
    }
}