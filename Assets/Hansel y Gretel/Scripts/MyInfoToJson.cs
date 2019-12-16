using UnityEngine;
using System.Collections;
//using UnityEngine.Experimental.Networking;

public class MyInfoToJson : MonoBehaviour {

	private QuestionManager qMan;



	// Use this for initialization
	IEnumerator Start () {
		//qMan = QuestionManager.Instance;


		//Invoke ("printJson", 1f);

		// Post usando WWW antiguo
		/*
		string jsonString = "{\n\t\"questions\":[\n\t\t{\n\t\t\t\"userAnsweredCorrectly\":false,\n\t\t\t\"realAnswer\":1,\n\t\t\t\"userTries\":0,\n\t\t\t\"playerPoints\":0,\n\t\t\t\"questionType\":0,\n\t\t\t\"questionTypeDescription\":\"Inferencia\",\n\t\t\t\"questionDifficulty\":0,\n\t\t\t\"questionDifficultyDescription\":\"Básico\",\n\t\t\t\"questionTotalPoints\":3,\n\t\t\t\"questionText\":\"q1\",\n\t\t\t\"question\":\"preg1\",\n\t\t\t\"optionTexts\":[\n\t\t\t\t\"op1_0\",\n\t\t\t\t\"op2_0\",\n\t\t\t\t\"op3_0\"\n\t\t\t],\n\t\t\t\"optionsType\":\"text\"\n\t\t},\n\t\t{\n\t\t\t\"userAnsweredCorrectly\":false,\n\t\t\t\"realAnswer\":3,\n\t\t\t\"userTries\":0,\n\t\t\t\"playerPoints\":0,\n\t\t\t\"questionType\":0,\n\t\t\t\"questionTypeDescription\":\"Inferencia\",\n\t\t\t\"questionDifficulty\":0,\n\t\t\t\"questionDifficultyDescription\":\"Básico\",\n\t\t\t\"questionTotalPoints\":3,\n\t\t\t\"questionText\":\"q2\",\n\t\t\t\"question\":\"preg2\",\n\t\t\t\"optionTexts\":[],\n\t\t\t\"optionsType\":\"image\"\n\t\t},\n\t\t{\n\t\t\t\"userAnsweredCorrectly\":false,\n\t\t\t\"realAnswer\":3,\n\t\t\t\"userTries\":0,\n\t\t\t\"playerPoints\":0,\n\t\t\t\"questionType\":0,\n\t\t\t\"questionTypeDescription\":\"Inferencia\",\n\t\t\t\"questionDifficulty\":0,\n\t\t\t\"questionDifficultyDescription\":\"Básico\",\n\t\t\t\"questionTotalPoints\":3,\n\t\t\t\"questionText\":\"q3\",\n\t\t\t\"question\":\"preg3\",\n\t\t\t\"optionTexts\":[\n\t\t\t\t\"op1\",\n\t\t\t\t\"op2\",\n\t\t\t\t\"op3\"\n\t\t\t],\n\t\t\t\"optionsType\":\"text\"\n\t\t},\n\t\t{\n\t\t\t\"userAnsweredCorrectly\":false,\n\t\t\t\"realAnswer\":2,\n\t\t\t\"userTries\":0,\n\t\t\t\"playerPoints\":0,\n\t\t\t\"questionType\":0,\n\t\t\t\"questionTypeDescription\":\"Inferencia\",\n\t\t\t\"questionDifficulty\":0,\n\t\t\t\"questionDifficultyDescription\":\"Básico\",\n\t\t\t\"questionTotalPoints\":3,\n\t\t\t\"questionText\":\"q4\",\n\t\t\t\"question\":\"preg4\",\n\t\t\t\"optionTexts\":[\n\t\t\t\t\"op1\",\n\t\t\t\t\"op2\",\n\t\t\t\t\"op3\"\n\t\t\t],\n\t\t\t\"optionsType\":\"text\"\n\t\t}\n\t],\n\t\"numCorrect\":3,\n\t\"numTotal\":4,\n\t\"type1Percentage\":1,\n\t\"type2Percentage\":1,\n\t\"type3Percentage\":1,\n\t\"teacherEmail\":\"teacher@email.com\",\n\t\"examName\" : \"TESTING INTELLIGENCE\",\n\t\"studentUser\":\"mystudentuser\",\n}";
		string url = "200";
		byte[] data = System.Text.Encoding.ASCII.GetBytes(jsonString.ToCharArray());
		Hashtable headers = new Hashtable ();
		headers.Add ("Content-Type", "application/json");
		WWW w3 = new WWW (url, data, headers);
		*/

		// Post usando UnityWebRequest nuevo
		/*UnityWebRequest www = UnityWebRequest.Get("http://strategywiki.org/wiki/File:SWMeeting20110226.txt");
		yield return www.Send ();

		if (www.isError) {
			Debug.Log (www.error);
		} else {
			print (www.downloadHandler.text);
		}*/
		yield return new WaitForSeconds (1f);
	}


	public void printJson() {
		JSONObject j = getJson ();
		print (j.Print (true));
	}

	public JSONObject getJson() {
		JSONObject j2 = new JSONObject (JSONObject.Type.ARRAY);


		//hacer un for aca:

		for (int i = 0; i < qMan.questions.Length; i++) {

			QuestionManager.QuestionJson q = qMan.questions [i];
			if (q==null) {
				break;
			}

			print (q);
			print (qMan.questions.Length);
			print (q.userAnsweredCorrectly);

			JSONObject j3 = new JSONObject (JSONObject.Type.OBJECT);
			j3.AddField ("userAnsweredCorrectly", q.userAnsweredCorrectly);
			j3.AddField ("realAnswer", q.realAnswer);
			j3.AddField ("userTries", q.userTries);
			j3.AddField ("playerPoints", q.playerPoints);
			j3.AddField ("questionType", q.questionType);
			j3.AddField ("questionTypeDescription", q.questionTypeDescription);
			j3.AddField ("questionDifficulty", q.questionDifficulty);
			j3.AddField ("questionDifficultyDescription", q.questionDifficultyDescription);
			j3.AddField ("questionTotalPoints", q.questionTotalPoints);
			j3.AddField ("questionText", q.questionText);
			j3.AddField ("question", q.question);
			JSONObject j4 = new JSONObject (JSONObject.Type.ARRAY);
			if (q.optionTexts != null) {
				for (int k = 0; k < q.optionTexts.Length; k++) {
				
                    //DESCOMENTAR DESPUES Y REVISAR EL ERROR
					//j4.Add (q.optionTexts [k]);
				}
			}

			j3.AddField ("optionTexts", j4);
			j3.AddField ("optionsType", q.optionsType);

			j2.Add (j3);
		}
			

		JSONObject j = new JSONObject (JSONObject.Type.OBJECT);
		j.AddField ("questions", j2);
		j.AddField ("numCorrect", qMan.numCorrect);
		j.AddField ("numTotal", qMan.numTotal);
		j.AddField ("type1Percentage", qMan.type1Percentage);
		j.AddField ("type2Percentage", qMan.type2Percentage);
		j.AddField ("type3Percentage", qMan.type3Percentage);

		return j;

	}
}
