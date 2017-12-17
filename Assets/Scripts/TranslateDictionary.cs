using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class stringCard
{
	public string gameObjectName = "";
	public string englishText = "";
	public string codedText = "";
	public int arcaneNeeded = 5;
}

public class TranslateDictionary : MonoBehaviour {

    public static TranslateDictionary instance;
	public List<stringCard> stringList = new List<stringCard>();

	// Use this for initialization
	void Start () {
        instance = this;
        Populate();
	}

	public void Populate()
	{
		if (stringList.Count > 0)
		{
			Debug.Log("spreadsheet has already been loaded");
			return;
		}

		Debug.Log("spreadsheet data loaded from file");

		TextAsset stringData;

		stringData = Resources.Load("BookStoryText") as TextAsset;

		string[] stringRows = stringData.text.Split(new char[] { '\r' });
        Debug.Log("Reading " + stringRows.Length + " rows");
		for (int i = 1; i < stringRows.Length; i++)
		{//i=1 to skip headers
			if (stringRows[i].Length < 4)
			{
				Debug.Log ("incomplete line found " + stringRows[i].Length);
				break;
			}
			string[] stringCols = stringRows[i].Split(',');

			Debug.Log (stringRows [i]);   // Shows data being loaded

			stringCard nextCard = new stringCard();
            nextCard.gameObjectName = Regex.Replace(stringCols[0], @"\t|\n|\r", "");
            nextCard.englishText = stringCols[1];
            nextCard.codedText = stringCols[2];
            nextCard.arcaneNeeded = int.Parse(stringCols[3]);
            Debug.Log("The object name is " + nextCard.gameObjectName);

			// GameObject.Find for matching name, addcomponent, etc?

			// one of these may be skipped depending how you structure things
			stringList.Add(nextCard);
		}

        StartCoroutine(ApplyDictionary());
	}

    IEnumerator ApplyDictionary() {
        yield return new WaitForSeconds(0.1f);
        foreach(stringCard eachCard in stringList) {
            GameObject matchGO = GameObject.Find(eachCard.gameObjectName);
            if (matchGO) {
                Text matchText = matchGO.GetComponentInChildren<Text>();
                if (matchText) {
                    matchText.text = (PlayerCommon.instance.knowledgeLevel >= eachCard.arcaneNeeded ?
                                      eachCard.englishText : eachCard.codedText);
                    Debug.Log("Comparing knowledge level " + PlayerCommon.instance.knowledgeLevel + " to arcane level " +
                              eachCard.arcaneNeeded + " on object " + matchGO.name);
                } else {
                    Debug.Log("Couldn't find text object for " + matchGO.name);
                }
            } else {
                Debug.Log("No match found for " + eachCard.gameObjectName);
            }
        }
    }
} // end of TranslateDictionary class
