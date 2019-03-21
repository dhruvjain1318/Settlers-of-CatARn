namespace ARVRAssignments
{
    using UnityEngine;
    using UnityEngine.UI;
    using Vuforia;

    public class GameController : MonoBehaviour
    {

        private const float m_MoveSpeed = 0.001f;

        private readonly string r_TouchScreenMessage = "Touch the screen to display my 3D NetID.";

        private readonly string r_MoveNetID3DMessage = "Move your finger on the screen to move my 3D NetID around.";

        [SerializeField]
        public Text m_SystemMessageTextHolder;

        private Vector3 m_NetID3DInitialPstn;

        private TextMesh m_NetID3DTextHolder;

        public GameObject mynetID;

        public GameObject InstantiatedNetID;

        private void Start()
        {
//            m_NetID3DInitialPstn = new Vector3(-0.462f, 0f, -0.411f);
//            m_NetID3DInitialPstn = new Vector3(-200.0f, -200.0f, -150.0f);
            m_NetID3DInitialPstn = new Vector3(0.0f, 0.0f, 0.0f);
            m_NetID3DTextHolder = null;
            m_SystemMessageTextHolder.text = r_TouchScreenMessage;
        }

        private void FixedUpdate()
        {
            if (Input.touchCount > 0)
            {
                if (m_NetID3DTextHolder == null)
                {
                    _SpawnPrefab();
                }
                //TODO: Implement the mini task 5 here.
                Touch continueTouch = Input.GetTouch(0);
                if (continueTouch.phase == TouchPhase.Stationary || continueTouch.phase == TouchPhase.Moved) 
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(continueTouch.position.x, continueTouch.position.y, Vector3.Distance(m_NetID3DInitialPstn, Camera.main.transform.position)));
//                    InstantiatedNetID.transform.position = touchPosition;
                    InstantiatedNetID.transform.position = Vector3.Lerp(InstantiatedNetID.transform.position, touchPosition, Time.deltaTime);
                }
//                Vector3 newpos = continueTouch.position;
//                mynetID.transform.position = newpos;
            }
        }

        private void _SpawnPrefab()
        {
            //TODO: Implement the mini task 4 here.
            m_SystemMessageTextHolder.text = r_MoveNetID3DMessage;
            mynetID = Resources.Load("Forest") as GameObject;
            InstantiatedNetID = Instantiate(mynetID, m_NetID3DInitialPstn, Quaternion.identity);
            m_NetID3DTextHolder = InstantiatedNetID.GetComponent<TextMesh>();
        }
    }
}