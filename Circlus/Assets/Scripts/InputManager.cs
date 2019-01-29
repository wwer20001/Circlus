using UnityEngine;

using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField]
    private float sensitivity = 1.0f;
    public float Sensitivity { get { return sensitivity; } private set { sensitivity = value; } }

    public bool IsTouched { get; private set; }
    public float TouchDeltaSize { get; private set; }

    private Vector3 PrevTouchPosition;

    private UnityAction<float> OnTouchDeltaSizeChange;

    private void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            IsTouched = true;
            PrevTouchPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            TouchDeltaSize = PrevTouchPosition.x - Input.mousePosition.x;
            if(TouchDeltaSize != 0)
            {
                OnTouchDeltaSizeChange(TouchDeltaSize * sensitivity);
            }
            PrevTouchPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            IsTouched = false;
        }
	}

    public void RegistOnTouchDeltaSizeChangeEvent(UnityAction<float> callback)
    {
        OnTouchDeltaSizeChange += callback;
    }
}
