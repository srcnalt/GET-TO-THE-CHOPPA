using UnityEngine;

public class EyeScript : MonoBehaviour
{
    public Transform eyeCenter;

    float margin = 0;

    private void Start()
    {
        eyeCenter.position = transform.position;
    }

    void Update ()
    {

        Vector3 diff = Input.mousePosition - eyeCenter.position;
        margin = Mathf.Clamp(Vector3.Distance(Input.mousePosition, eyeCenter.position), 0, 15);

        diff.Normalize();


        Vector3 pointToGo = eyeCenter.position + diff * margin;

        Debug.DrawLine(eyeCenter.position, eyeCenter.position + diff * margin);
        
        transform.position = Vector3.MoveTowards(transform.position, pointToGo, 5f);
    }
}
