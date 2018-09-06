using UnityEngine;
using unity;

namespace unity 
{
    public class movement : MonoBehaviour
    {

        public float movementSpeed = 5.0f;
        public float clockwise = 1000.0f;
        public float counterClockwise = -5.0f;

        void Start()
        {

        }

        void Update()
        {
            var rigidbody = GetComponent<Rigidbody>();

            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rigidbody.position += Vector3.back * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rigidbody.position += Vector3.left * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rigidbody.position += Vector3.back * Time.deltaTime * movementSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.position += Vector3.up * Time.deltaTime * movementSpeed;
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                transform.position += Vector3.down * Time.deltaTime * movementSpeed;
            }
        }
    }
}