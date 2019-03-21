namespace ARVRAssignments
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections.Generic;
    using Vuforia;
    using System.Linq;
    public class GameController : MonoBehaviour
    {

        [SerializeField]

        public GameObject board;

        public Canvas canvas;
        public Button b_forest;
        public Button b_grain;
        public Button b_ore;
        public Button b_brick;
        public Button b_pasture;


        // 3D assets

        private GameObject forest;

        private List<GameObject> ForestTiles = new List<GameObject>();
        private List<GameObject> GrainTiles = new List<GameObject>();
        private List<GameObject> PastureTiles = new List<GameObject>();
        private List<GameObject> OreTiles = new List<GameObject>();
        private List<GameObject> BrickTiles = new List<GameObject>();


        private void Start()
        {
            // board = Instantiate(Resources.Load("Prefabs/Board") as GameObject);

            _getforesttiles();
            _getgraintiles();
            _getoretiles();
            _getpasturetiles();
            _getbricktiles();




            //_SwitchTargetByName("Forest");

            /*
// Deprecated. Replaced by _SwitchTargetByName("DatabaseName").
_grainstatus(false);
_orestatus(false);
_foreststatus(false);
_brickstatus(false);
_pasturestatus(false);
*/

        }

        private void FixedUpdate()
        {
            // These buttons activate the different datasets, allowing prioritization of image targets.
            b_grain.onClick.AddListener(delegate { _SwitchTargetByName("Wheat"); });
            b_forest.onClick.AddListener(delegate { _SwitchTargetByName("Forest"); });
            b_ore.onClick.AddListener(delegate { _SwitchTargetByName("Ore"); });
            b_brick.onClick.AddListener(delegate { _SwitchTargetByName("Brick"); });
            b_pasture.onClick.AddListener(delegate { _SwitchTargetByName("Sheep"); });



            /*
            b_grain.onClick.AddListener(delegate { _grainstatus(true); });
            b_forest.onClick.AddListener(delegate { _foreststatus(true); });
            b_ore.onClick.AddListener(delegate { _orestatus(true); });
            b_brick.onClick.AddListener(delegate { _brickstatus(true); });
            b_pasture.onClick.AddListener(delegate { _pasturestatus(true); });
            */

            
            // Raycasting Implementation
            Touch touchInfo = Input.GetTouch(0);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touchInfo.position);

            // When a hit is detected on an object tagged as a 3D tile - change the state to Selected.
            if (Input.touchCount > 0 && Physics.Raycast(ray, out hit) && hit.transform.tag == "Tile3D")
            {
                TileBehaviour hittilebehaviour = hit.collider.GetComponentInChildren<TileBehaviour>();

                if (hittilebehaviour.tilestate != TileBehaviour.MarkerState.Selected)
                {
                    hittilebehaviour.tilestate = TileBehaviour.MarkerState.Selected;
                    Debug.Log(hit.transform.tag + "selected"); 
                }

                if (hittilebehaviour.tilestate == TileBehaviour.MarkerState.Selected)
                {
                    hittilebehaviour.tilestate = TileBehaviour.MarkerState.Deselected;
                    Debug.Log(hit.transform.tag + "deselected");

                }
            }
        }

        private void _SpawnForest()
        {
            forest = Instantiate(Resources.Load("Forest") as GameObject);
            TileBehaviour forestbehaviour = forest.GetComponent<TileBehaviour>();
            forestbehaviour.tilestate = TileBehaviour.MarkerState.Spawned;
        }

        private void _getforesttiles()
        {
            // This function makes a list of the Forest Tiles. To be used when activating image targets.
            foreach (Transform child in board.transform)
            {
                if (child.tag == "Forest")
                {
                    ForestTiles.Add(child.gameObject);
                }
            }

        }

        private void _getgraintiles()
        {
            // This function makes a list of the Grain Tiles. To be used when activating image targets.

            foreach (Transform child in board.transform)
            {
                if (child.tag == "Grain")
                {
                    GrainTiles.Add(child.gameObject);
                }
            }

        }

        private void _getpasturetiles()
        {
            // This function makes a list of the Pasture Tiles. To be used when activating image targets.

            foreach (Transform child in board.transform)
            {
                if (child.tag == "Pasture")
                {
                    PastureTiles.Add(child.gameObject);
                }
            }

        }

        private void _getoretiles()
        {
            // This function makes a list of the Ore Tiles. To be used when activating image targets.

            foreach (Transform child in board.transform)
            {
                if (child.tag == "Ore")
                {
                    OreTiles.Add(child.gameObject);
                }
            }

        }

        private void _getbricktiles()
        {
            // This function makes a list of the Brick Tiles. To be used when activating image targets.

            foreach (Transform child in board.transform)
            {
                if (child.tag == "Brick")
                {
                    BrickTiles.Add(child.gameObject);
                }
            }

        }

        private void _foreststatus(bool value)
        // This function toggles forest tiles on or off (false or true).
        // Deprecated. Replaced by _SwitchTargetByName("DatabaseName").

        {
            foreach (GameObject child in ForestTiles)
            {
                child.SetActive(value);
            }

        }

        private void _grainstatus(bool value)
        // This function toggles grain tiles on or off (false or true).
        // Deprecated. Replaced by _SwitchTargetByName("DatabaseName").


        {
            foreach (GameObject child in GrainTiles)
            {
                child.SetActive(value);
            }

        }

        private void _brickstatus(bool value)
        // This function toggles brick tiles on or off (false or true).
        // Deprecated. Replaced by _SwitchTargetByName("DatabaseName").

        {
            foreach (GameObject child in BrickTiles)
            {
                child.SetActive(value);
            }

        }

        private void _orestatus(bool value)
        // This function toggles ore tiles on or off (false or true).
        // Deprecated. Replaced by _SwitchTargetByName("DatabaseName").

        {
            foreach (GameObject child in OreTiles)
            {
                child.SetActive(value);
            }

        }

        private void _pasturestatus(bool value)
        // This function toggles pasture tiles on or off (false or true).
        // Deprecated. Replaced by _SwitchTargetByName("DatabaseName").

        {
            foreach (GameObject child in PastureTiles)
            {
                child.SetActive(value);
            }

        }

        public void _SwitchTargetByName(string activateThisDataset)
        {
            // This function activates an image target database and switches off all of the 
            // other databases. 
            TrackerManager trackerManager = (TrackerManager)TrackerManager.Instance;
            ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

            IEnumerable<DataSet> datasets = objectTracker.GetDataSets();
            IEnumerable<DataSet> activeDataSets = objectTracker.GetActiveDataSets();
            List<DataSet> activeDataSetsToBeRemoved = activeDataSets.ToList();

            objectTracker.Stop();


            //Loop through all the active datasets and deactivate them.
            //Swapping of the datasets should not be done while the ObjectTracker is working at the same time.
            //So, Stop the tracker first.
            foreach (DataSet ads in activeDataSetsToBeRemoved)
            {
                objectTracker.DeactivateDataSet(ads);

                Debug.Log(ads + " deactivated");
                
            }

            //Then, look up the new dataset and if one exists, activate it.
            foreach (DataSet ds in datasets)
            {
                if (ds.Path.Contains(activateThisDataset))
                {
                    objectTracker.ActivateDataSet(ds);

                }

            }
            //Finally, start the object tracker.
            objectTracker.Start();

            IEnumerable<DataSet> activeDataSetsNew = objectTracker.GetActiveDataSets();
            foreach (DataSet ds in activeDataSetsNew)
            {
                Debug.Log(ds + " activated");
            }
        }

    }
}