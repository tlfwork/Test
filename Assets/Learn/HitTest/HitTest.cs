using UnityEngine;

public class HitTest : MonoBehaviour
{
    bool choosen = false;

    float choosen_y = 0;

    Vector3 offset;

    GameObject choosengameobject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);

        //Debug.LogWarning(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Cube")
                {
                    choosen = true;

                    choosengameobject = hit.collider.gameObject;

                    choosen_y = choosengameobject.transform.position.y;

                    Vector3 Cube_W_Pos_Before = choosengameobject.transform.position;

                    float deepth = Camera.main.WorldToScreenPoint(Cube_W_Pos_Before).z;

                    Vector3 Cube_W_Pos_After = Camera.main.ScreenToWorldPoint
                        (
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, deepth)
                        );

                    offset = Cube_W_Pos_After - Cube_W_Pos_Before;
                }
            }
        }

        if (choosen)
        {
            Vector3 Cube_W_Pos_Before = choosengameobject.transform.position;

            float deepth = Camera.main.WorldToScreenPoint(Cube_W_Pos_Before).z;

            Vector3 Cube_W_Pos_After = Camera.main.ScreenToWorldPoint
                (
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, deepth)
                );

            choosengameobject.transform.position = Cube_W_Pos_After - offset;

            choosengameobject.transform.position = new Vector3
                (
                choosengameobject.transform.position.x,choosen_y, choosengameobject.transform.position.z
                );
        }

        if (Input.GetMouseButtonUp(0) && choosen)
        {
            choosen = false;
        }
    }
}
