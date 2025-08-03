using TerrariaClone.Enums;
using TerrariaClone.Items;
using TerrariaClone.Terrain;
using UnityEngine;

namespace TerrariaClone.Controller
{
    public class CamController : MonoBehaviour
    {
        [Header("Breaking & Placing")]
        [Tooltip("The tile class we place when we right click")]
        [SerializeField] private TileClass placeTile;
        [Tooltip("The layer of the tile we break when we left click")]
        [SerializeField] private TileLayer breakLayer;

        [Tooltip("Speed which the camera moves")]
        [SerializeField] float moveSpeed = 5f;

        //references
        Vector3 velocity;
        private TerrainGenerator terrain;
        private Vector3 mousePos;
        private Camera mainCam;

        void Awake() => Init();

        private void Init()
        {
            mainCam = GetComponent<Camera>();
            velocity = transform.position;
            terrain = GameObject.FindObjectOfType<TerrainGenerator>();
        }

        private void LateUpdate() => HandleMovement(); //update the camera movement

        private void Update()
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition); //position of our mouse in the world
            HandlePlacing();
            HandleBreaking();
        }

        private void HandleBreaking()//left click to break tiles
        {
            if (Input.GetMouseButton(0))
            {
                terrain.RemoveTile(breakLayer,
                    Mathf.RoundToInt(mousePos.x),
                    Mathf.RoundToInt(mousePos.y), true);
            }
        }
        private void HandlePlacing()//right click to place tiles
        {
            if (Input.GetMouseButton(1))
            {
                terrain.PlaceTile(placeTile,
                    Mathf.RoundToInt(mousePos.x),
                    Mathf.RoundToInt(mousePos.y), true);
            }
        }
        private void HandleMovement() //movement for the camera
        {
            velocity = new Vector2(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;

            transform.position += velocity;
        }
    }
}