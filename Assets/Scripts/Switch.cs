using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject targetObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Echo"))
        {
            ToggleSwitch(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Echo"))
        {
            ToggleSwitch(false);
        }
    }

    private void ToggleSwitch(bool activate)
    {
        targetObject.SetActive(!activate);
    }

    void FixedUpdate()
    {
        if (GetComponent<Collider>().enabled)
        {
            foreach (Collider col in Physics.OverlapBox(transform.position, GetComponent<Collider>().bounds.extents))
            {
                if (col.CompareTag("Player") || col.CompareTag("Echo"))
                {
                    ToggleSwitch(true);
                    return;
                }
            }
            ToggleSwitch(false);
        }
    }
}
