using UnityEngine;
using UnityEngine.UI;

public class StageSelectControl : MonoBehaviour {

    [SerializeField]private bool isHard = false;

	// Use this for initialization
	void Start () {
        foreach(Transform child in transform)
        {
            if (child.name.Contains("Stage"))
            {
                int _nameLength = child.name.Length;
                string _nameNumber = child.name.Substring(5, _nameLength - 5);
                int _number = int.Parse(_nameNumber);

                if (isHard)
                {
                    if (GameManger.Instance.hardLevel < _number)
                    {
                        child.Find("Text").GetComponent<Text>().color = Color.gray;
                        child.GetComponent<Button>().enabled = false;
                    }
                } else
                {
                    if (GameManger.Instance.normalLevel < _number)
                    {
                        child.Find("Text").GetComponent<Text>().color = Color.gray;
                        child.GetComponent<Button>().enabled = false;
                    }
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
