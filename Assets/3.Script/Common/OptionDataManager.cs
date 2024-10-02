using UnityEngine;

public class OptionDataManager : MonoBehaviour {
    [SerializeField] private OptionData optionDataObject;
    public OptionData OptionData => optionDataObject;
}
