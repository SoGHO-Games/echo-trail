using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform startPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, startPoint.position);
        RaycastHit hit;
        if (Physics.Raycast(startPoint.position, Vector3.left, out hit))
        {
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.CompareTag("Player"))
            {
                var sceneManager = FindAnyObjectByType<SceneManager>();
                sceneManager.RespawnPlayer(isDeath: true);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, startPoint.position + Vector3.left * 100f);
        }
    }
}
