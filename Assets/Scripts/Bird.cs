using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    [SerializeField] float maxDragDistance = 4;
    [SerializeField] float lunchPower = 150;
    LineRenderer lineRenderer;
    Vector3 startingPosition;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.enabled = false;
        startingPosition = transform.position;
    }
    void OnMouseUp()
    {
        Vector3 directionAndMagnitude = startingPosition - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionAndMagnitude * lunchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        lineRenderer.enabled = false;
    }
    void OnMouseDrag()
    {
        var destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destination.z = 0;
        if (Vector2.Distance(destination, startingPosition) > maxDragDistance)
            destination = Vector3.MoveTowards(startingPosition, destination, maxDragDistance);
        transform.position = destination;
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GetComponent<Rigidbody2D>().gravityScale = 1;

        if (FindAnyObjectByType<Enemy>(FindObjectsInactive.Exclude) == null)
        {
            Debug.Log("GameOver");
            int levelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(levelToLoad);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke("ReloadLevel", 5);
    }
    void ReloadLevel()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
