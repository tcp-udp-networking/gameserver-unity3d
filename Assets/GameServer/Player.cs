using UnityEngine;

namespace GameServer
{
    public class Player : MonoBehaviour
    {
        public int id;
        public string username;
        public CharacterController controller;
        public float gravity = -9.81f;
        public float moveSpeed = 5f;
        public float jumpSpeed = 5f;

        private bool[] inputs;
        private float yVelocity = 0;
        
        //public Texture2D playerTexture2D;
        public WebCamTexture playerTexture2D;
        public byte[] playerTextureBytes;

        private void Start()
        {
            gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
            moveSpeed *= Time.fixedDeltaTime;
            jumpSpeed *= Time.fixedDeltaTime;
        }

        public void Initialize(int _id, string _username)
        {
            id = _id;
            username = _username;
            inputs = new bool[5];
            //playerTexture2D = new Texture2D(300,300, TextureFormat.RGB24, false);
            playerTexture2D = new WebCamTexture();
            playerTextureBytes = new byte[128];
        }

        /// <summary>Processes player input and moves the player.</summary>
        public void FixedUpdate()
        {
            Vector2 _inputDirection = Vector2.zero;
            if (inputs[0])
            {
                _inputDirection.y += 1;
            }
            if (inputs[1])
            {
                _inputDirection.y -= 1;
            }
            if (inputs[2])
            {
                _inputDirection.x -= 1;
            }
            if (inputs[3])
            {
                _inputDirection.x += 1;
            }

            Move(_inputDirection);
            ShowWebcamTexture(playerTextureBytes);
        }
        
        private void ShowWebcamTexture(byte[] texture)
        {
            //playerTexture2D = texture;
            playerTextureBytes = texture;
            ServerSend.PlayerWebcamTexture(id, this);
        }

        /// <summary>Calculates the player's desired movement direction and moves him.</summary>
        /// <param name="_inputDirection"></param>
        private void Move(Vector2 _inputDirection)
        {
            Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
            _moveDirection *= moveSpeed;

            if (controller.isGrounded)
            {
                yVelocity = 0f;
                if (inputs[4])
                {
                    yVelocity = jumpSpeed;
                }
            }
            yVelocity += gravity;

            _moveDirection.y = yVelocity;
            controller.Move(_moveDirection);

            ServerSend.PlayerPosition(this);
            ServerSend.PlayerRotation(this);
        }

        /// <summary>Updates the player input with newly received input.</summary>
        /// <param name="_inputs">The new key inputs.</param>
        /// <param name="_rotation">The new rotation.</param>
        public void SetInput(bool[] _inputs, Quaternion _rotation)
        {
            inputs = _inputs;
            transform.rotation = _rotation;
        }
        
        public void SetTexture(byte[] texture2D)
        {
           // playerTexture2D = texture2D;
           playerTextureBytes = texture2D;
        }
    }
}