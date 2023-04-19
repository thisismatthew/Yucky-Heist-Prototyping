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


    public Button Hand_b;
    private int HandImgIndex;

    public Button Apply_b;

    // Start is called before the first frame update
    void Start()
    {


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
        Instantiate(armPrefab);

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
        Canvas.SetActive(false);

    }

    private void Update()
    {
        //keep an up to date record of the values
        values[0] = Stretch_sl.value;
        values[1] = Retract_sl.value;
        values[2] = Bones_sl.value;
        values[3] = Wrist_sl.value;
        values[4] = Amp_sl.value;
        values[5] = Freq_sl.value;
        values[6] = HandImgIndex;


    }
}
