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
}
