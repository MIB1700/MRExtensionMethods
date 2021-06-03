using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MR.CustomExtensions;
 
 /*
  *    ADD THIS SCRIPT TO A CUBE IN THE SCENE TO TEST
  */


public class testScript : MonoBehaviour
{
    private Material material;
    // Start is called before the first frame update

    public List<GameObject> listOfGos;
    public GameObject[] arrayOfGos;

    private Vector3 screenPoint;
    private Vector3 offset;

    private Vector3 screenPoin2t;
    private Vector3 offset2;

    //private float speed = 1f;

    //All Coroutines now come with optional Callback function!! 

    void Start()
    {
       

        StartCoroutine(5f.Timer(
            ()=>
            {   //passing in anonymous function as callback
                //could be anything though
                Debug.Log("5 second timer done");
            })
            );

        StartCoroutine(5.5f.Delay(
            () => { Debug.Log("5.5 second timer done"); })
            );

        Vector3 test = Vector3.zero.WithX(99);
        transform.position = Vector3.zero;
        gameObject.RemoveComponent<LineRenderer>(false);

        StartCoroutine(gameObject.MoveTo(Vector3.zero.WithX(15), 10f, () =>
        {
            Debug.Log($"gameObject with tag {gameObject.tag} is done moving");
        }));
        // StartCoroutine(gameObject.RotateTo(new Quaternion(0.87497f, -0.14171f, -0.45028f, -0.107673f), 10f));

        StartCoroutine(gameObject.RotateTo(new Vector3(-60, 0 , 145), 10f));

        StartCoroutine(gameObject.ScaleTo(new Vector3(2, 1, 4), 10f));

        // material = GetComponent<Renderer>().material;
        // StartCoroutine(material.ColorTo(Color.red, 5f));

        StartCoroutine(gameObject.ColorTo(Color.red, 5f, ()=>
        {
            Debug.Log("gambeObject is done changing COLOR");
        }));

        listOfGos = new List<GameObject>();
        arrayOfGos = new GameObject[10];

        for (var go = 0; go < 10; go++)
        {
                GameObject g = GameObject.CreatePrimitive(PrimitiveType.Cube);
                g.transform.localScale = Vector3.one;

                listOfGos.Add(g);

                GameObject gg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                gg.transform.localScale = Vector3.one;

                arrayOfGos[go] = gg;
        }
//----------------------
        listOfGos.ForEach<GameObject>(t => t.transform.position = Vector3.zero.Random(Vector3.one * -5f, Vector3.one * 5f));
        listOfGos.ForEach<GameObject>(t => t.SetActive(false)); //just for demo... set right back to true...

        listOfGos.ForEach<GameObject>(t => t.SetActive(true));

        //multiple calls at once!
        listOfGos.ForEach<GameObject>(t =>
            {
                //initial random color
              //  t.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                //randomly change to this color

                //really should check if we have a Renderer in the first place... after Unity 2019.2 we could do
                 if (t.TryGetComponent<Renderer>(out var curRend)){

                    //curRend.material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    curRend.material.color = curRend.material.color.Randomize();
                 }

                 

                StartCoroutine(t.ColorTo(
                        Color.black,
                        UnityEngine.Random.Range(5f, 10f)
                        )
                ); //over random time))

                StartCoroutine(t.ScaleTo(
                        Vector3.zero.Random(Vector3.one * 2f, Vector3.one * 3f),  //to random size
                        UnityEngine.Random.Range(5f, 10f)//over random time
                        )
                );

                StartCoroutine(t.MoveTo(
                         t.transform.position.AddX(-3),  //to random location
                         5f,
                         UnityEngine.Random.Range(0f, 5f)//over random time
                        )
                );
            }
        );
//----------------------
        arrayOfGos.ForEach<GameObject>(t => t.transform.position = Vector3.zero.Random(Vector3.one, Vector3.one * 10f));

        arrayOfGos.ForEach<GameObject>(t =>
            {
                //initial random color
                t.GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            //randomly change to this color
            StartCoroutine(t.ColorTo(
                    Color.green,
                    UnityEngine.Random.Range(5f, 10f)
                    )
                ); //over random time))

                StartCoroutine(t.ScaleTo(
                        Vector3.zero.Random(Vector3.zero, Vector3.one * 2f),  //to random size
                        UnityEngine.Random.Range(5f, 10f)//over random time
                        )
                );
            }
        );
    }

    void Update()
    { // TODO: need a "smooth look at" that takes framerate into account!! some dampening probably...

        listOfGos.ForEach<GameObject>(go => go.LookAt(gameObject));
        //Debug.Log($"lookAtRotation: {listOfGos[0].GetLookAtRotation(gameObject)}");
        arrayOfGos.ForEach<GameObject>(go => go.transform.rotation = go.GetLookAtRotation(gameObject));
    }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        gameObject.ColorTo(Color.yellow);


        //chaining them! use LINQ to filter the list then act on the filtered content...
        listOfGos.Where(x => x.transform.localScale.x < 2.5f).ForEach(
            t => {
                StartCoroutine(t.MoveTo(Vector3.zero.Random(Vector3.one * -5f, Vector3.one * 5f), 5f));
                StartCoroutine(t.ColorTo(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2.5f));
            }
        );

        listOfGos.Where(x => x.transform.localScale.x >= 4f).ForEach(
            t => {
                StartCoroutine(t.MoveTo(t.transform.position.WithY(UnityEngine.Random.Range(-5f, 10f)), 5f));
                StartCoroutine(t.ColorTo(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 2.5f));
            }
        );
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
    }

     void OnMouseUp()
    {
        gameObject.ColorTo(Color.red);
    }
}
