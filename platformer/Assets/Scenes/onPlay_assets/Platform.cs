using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3 direction = Vector3.right;

    private float currentSpeed = 0f;
    private Vector3 sceneVector = new(17.77778f, 0, 0);
    private bool isMoving = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Moving(5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSpeed == 0f) { isMoving = false; }
        if (isMoving)
        {
            if (transform.position.x - sceneVector.x >= 0f)
            {
                transform.position -= sceneVector * 2;
            }
            else if (transform.position.x + sceneVector.x <= 0f)
            {
                transform.position += sceneVector * 2;
            }
            transform.Translate(direction * currentSpeed * Time.deltaTime * (-1));


        }


    }

    public void Moving(float customSpeed)
    {
        currentSpeed = customSpeed;
        isMoving = true;
    }
}
