using System.Collections.Generic;
using UnityEngine;

public enum ERotateDirection
{
    Clock = -1,
    ReverseClock = 1,
}

public class RotationMenuManager : MonoBehaviour
{

    [SerializeField]
    private List<RotationMenuItem> rotationMenuItems = new List<RotationMenuItem>();
    public RotationMenuItem CenterItem;// { get; set { } }
    private RotationMenuItem PrevCenterItem;// { get; private set; }
    [Space(1)]

    #region Option
    [Header("Option")]
    [SerializeField]
    private GameObject menuItemPrefab;
    [SerializeField]
    private float centerMenuAngle = 0f;
    private const float centerAngleOffset = -90f;
    [SerializeField]
    private ERotateDirection rotateDirection = ERotateDirection.Clock;
    public Vector3 CenterMenuDirection { get; private set; }
    [Range(1, 10000)]
    [SerializeField]
    private float radius;
    [Range(1, 360)]
    [SerializeField]
    private int menuItemCount;
    [SerializeField]
    private AnimationCurveData meneItemSizeCurve;
    public AnimationCurveData MeneItemSizeCurve { get { return meneItemSizeCurve; } }
    [SerializeField]
    private bool isLoop;
    public bool IsLoop { get { return isLoop; } }
    [SerializeField]
    private int StartPosition;
    [SerializeField]
    private float BetweenItem2ItemAngle;
    #endregion

    #region Property
    public float NowRotateAngle { get { return transform.rotation.eulerAngles.z; } }
    #endregion

    private void Awake()
    {
        var angle = (centerMenuAngle - centerAngleOffset);
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * (float)rotateDirection;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        CenterMenuDirection = new Vector3(x, y);
    }

    // Use this for initialization
    void Start()
    {
        InputManager.Instance.RegistOnTouchDeltaSizeChangeEvent(RotateButtonPannel);
    }

    float timer = 2f;
    float leftAngle;
    float goalAngle;
    void Update()
    {
        if (!InputManager.Instance.IsTouched)
        {
            if (timer > 1.0f)
            {
                return;
            }

            timer += Time.deltaTime * 10f;
            float nowAngle = BetweenItem2ItemAngle * Mathf.Lerp(leftAngle, goalAngle, timer);
            transform.rotation = Quaternion.Euler(0f, 0f, nowAngle);
        }
        else
        {
            leftAngle = transform.rotation.eulerAngles.z / BetweenItem2ItemAngle;
            goalAngle = Mathf.RoundToInt(leftAngle);
            timer = 0f;
        }

    }

    private void RotateButtonPannel(float delta)
    {
        transform.Rotate(Vector3.forward, delta);
    }

    public void SetCenterItem(RotationMenuItem item)
    {
        PrevCenterItem = CenterItem;
        CenterItem = item;
    }


    public void GenerateRotationMenuItems()
    {
        transform.rotation = Quaternion.identity;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        rotationMenuItems.Clear();

        BetweenItem2ItemAngle = 360f / menuItemCount;

        for (int i = 0; i < menuItemCount; i++)
        {
            var angle = (centerMenuAngle - centerAngleOffset) + (BetweenItem2ItemAngle * i);

            var item = Instantiate(menuItemPrefab).GetComponent<RotationMenuItem>();
            item.name = string.Format("SubMenuItem {0:D3}", i);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * (float)rotateDirection;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 newItemDirectionFromCenter = new Vector3(x, y);
            item.transform.position = transform.position + (newItemDirectionFromCenter.normalized * radius);
            item.transform.rotation = Quaternion.identity;
            item.transform.SetParent(transform);
            item.SetManager(this);
            item.Index = i;

            rotationMenuItems.Add(item);
        }

        for (int i = 0; i < rotationMenuItems.Count; i++)
        {
            if (i == 0)
            {
                rotationMenuItems[i].leftItem = rotationMenuItems[rotationMenuItems.Count - 1];
                rotationMenuItems[i].rightItem = rotationMenuItems[i + 1];
            }
            else if (i == rotationMenuItems.Count - 1)
            {
                rotationMenuItems[i].leftItem = rotationMenuItems[i - 1];
                rotationMenuItems[i].rightItem = rotationMenuItems[0];
            }
            else
            {
                rotationMenuItems[i].leftItem = rotationMenuItems[i - 1];
                rotationMenuItems[i].rightItem = rotationMenuItems[i + 1];
            }

            rotationMenuItems[i].leftSetItem = rotationMenuItems[(i - StartPosition) < 0 ? (i - StartPosition) + rotationMenuItems.Count : i - StartPosition];
            rotationMenuItems[i].rightSetItem = rotationMenuItems[(i + StartPosition) >= rotationMenuItems.Count ? (i + StartPosition) - rotationMenuItems.Count : i + StartPosition];
        }

        transform.Rotate(0f, 0f, (StartPosition * (360f / menuItemCount)));
    }

    private void OnDrawGizmosSelected()
    {
        var angle = (centerMenuAngle - centerAngleOffset) ;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * (float)rotateDirection;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (new Vector3(x, y) * radius));
    }
}
