using UnityEngine;

public class EyeScript : MonoBehaviour
{
    Vector3 position = new Vector3();
    float margin = 0;

    private void Start()
    {
        position = transform.position;
    }

    void Update ()
    {
        Vector3 diff = Input.mousePosition - position;
        margin = Mathf.Clamp(Vector3.Distance(Input.mousePosition, position), 0, 15);

        diff.Normalize();


        Vector3 pointToGo = position + diff * margin;

        Debug.DrawLine(position, position + diff * margin);
        
        transform.position = Vector3.MoveTowards(transform.position, pointToGo, 5f);
    }
}
