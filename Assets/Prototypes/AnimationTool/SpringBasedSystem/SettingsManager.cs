using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject armPrefab;
    private ArmTrail tm;
    private SpringArmManager sm;

    private int HandImageIndex = 0;
    public List<Sprite> HandImages;
    public Vector3 SetPos;
    private List<float> values = new List<float>();

    public Slider Stretch_sl;
    public Slider Retract_sl;
    public Slider Bones_sl;
    public Slider Wrist_sl;
    public Slider Amp_sl;
    public Slider Freq_sl;

    public TextMeshProUGUI Stretch_txt;
    public TextMeshProUGUI Retract_txt;
    public TextMeshProUGUI Bones_txt;
    public TextMeshProUGUI Wrist_txt;
    public TextMeshProUGUI Amp_txt;
    public TextMeshProUGUI Freq_txt;

    private bool Show = false;

    public Button Hand_b;

    public Button Apply_b;

    // Start is called before the first frame update
    void Start()
    {

        tm = FindObjectOfType<ArmTrail>();
        sm = FindObjectOfType<SpringArmManager>();
        SetPos = tm.gameObject.transform.position;
        DontDestroyOnLoad(this);
        for (int i = 0; i<7; i++)
        {
            values.Add(0);
        }

        Stretch_sl.onValueChanged.AddListener((v)=> {
            Stretch_txt.text = v.ToString("0.000");
        });
        Retract_sl.onValueChanged.AddListener((v) => {
            Retract_txt.text = v.ToString("0.000");
        });
        Bones_sl.onValueChanged.AddListener((v) => {
            Bones_txt.text = v.ToString("0.000");
        });
        Wrist_sl.onValueChanged.AddListener((v) => {
            Wrist_txt.text = v.ToString("0.000");
        });
        Amp_sl.onValueChanged.AddListener((v) => {
            Amp_txt.text = v.ToString("0.0000");
        });
        Freq_sl.onValueChanged.AddListener((v) => {
            Freq_txt.text = v.ToString("0.000");
        });
    }

    public void ApplySettings()
    {
        Destroy(FindObjectOfType<SpringArmManager>().gameObject);
        Instantiate(armPrefab, SetPos, Quaternion.identity);

        tm = FindObjectOfType<ArmTrail>();
        sm = FindObjectOfType<SpringArmManager>();

        // apply the values to the sources
        sm.StretchSpeed = values[0];
        Stretch_sl.value = values[0];
        tm.RetractSpeed = values[1];
        Retract_sl.value = values[1];
        tm.newSegmentDistance = values[2];
        Bones_sl.value = values[2];
        sm.SpringDesiredSpacing = values[3];
        Wrist_sl.value = values[3];
        tm.Amplitude = values[4];
        Amp_sl.value = values[4];
        tm.Frequency = values[5];
        Freq_sl.value = values[5];
        sm.Hand.GetComponentInChildren<SpriteRenderer>().sprite = HandImages[HandImageIndex];
        Canvas.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            sm = FindObjectOfType<SpringArmManager>();
            Canvas.SetActive(!Show);
            sm.Paused = !Show;
            Show = !Show;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            NextHandImage();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetHandPosition();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //keep an up to date record of the values
        values[0] = Stretch_sl.value;
        values[1] = Retract_sl.value;
        values[2] = Bones_sl.value;
        values[3] = Wrist_sl.value;
        values[4] = Amp_sl.value;
        values[5] = Freq_sl.value;
        values[6] = HandImageIndex;


    }

    public void NextHandImage()
    {
        sm = FindObjectOfType<SpringArmManager>();
        HandImageIndex++;
        if (HandImageIndex == HandImages.Count) HandImageIndex = 0;
        Debug.Log(HandImageIndex);
        sm.Hand.GetComponentInChildren<SpriteRenderer>().sprite = HandImages[HandImageIndex];
    }

    public void SetHandPosition()
    {
        sm = FindObjectOfType<SpringArmManager>();
        SetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SetPos.z = 0;

        Destroy(sm.gameObject);
        Instantiate(armPrefab, SetPos, Quaternion.identity);

        tm = FindObjectOfType<ArmTrail>();
        sm = FindObjectOfType<SpringArmManager>();

        // apply the values to the sources
        sm.StretchSpeed = values[0];
        Stretch_sl.value = values[0];
        tm.RetractSpeed = values[1];
        Retract_sl.value = values[1];
        tm.newSegmentDistance = values[2];
        Bones_sl.value = values[2];
        sm.SpringDesiredSpacing = values[3];
        Wrist_sl.value = values[3];
        tm.Amplitude = values[4];
        Amp_sl.value = values[4];
        tm.Frequency = values[5];
        Freq_sl.value = values[5];
        sm.Hand.GetComponentInChildren<SpriteRenderer>().sprite = HandImages[HandImageIndex];
        // find out if we want a topside hand, left or right
    }


}
