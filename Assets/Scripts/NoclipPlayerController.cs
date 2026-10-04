using UnityEngine;

public class NoclipPlayerController : MonoBehaviour
{
    private float _speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private float xRotation = 0f;
    private float yRotation = 0f;
    // Update is called once per frame
    void Update()
    {
        // 1. Get Mouse input multiplied by sensitivity
        float mouseX = Input.GetAxis("Mouse X") * 5;
        float mouseY = Input.GetAxis("Mouse Y") * 5;
        // 2. Calculate vertical camera rotation (Look Up/Down)
        xRotation -= mouseX;
        yRotation -= mouseY;

        // 3. Apply vertical rotation directly to this camera
        transform.localRotation = Quaternion.Euler(yRotation, -xRotation, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * 10);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * 10);
        }
        
        
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * Time.deltaTime * 10);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.Translate(Vector3.up * Time.deltaTime * 10);
        }
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            this.transform.Translate(Vector3.down * Time.deltaTime * 10);
        }
    }
}
