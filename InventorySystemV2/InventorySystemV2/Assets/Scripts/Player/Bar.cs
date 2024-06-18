using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A generic script used for setting the health bar
/// </summary>
public class Bar : MonoBehaviour
{
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    public void UpdateBar(float CurValue, float MaxValue)
    {
        slider.value = (CurValue / MaxValue);
    }
}
