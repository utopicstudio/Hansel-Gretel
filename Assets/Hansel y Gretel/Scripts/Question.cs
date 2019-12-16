using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Question : MonoBehaviour {

    public enum QChoice {A=1,B=2,C=3}
	public enum QType {Inferencia, ExtraccionDeInfo, InterpretacionDeSentido}
	public enum QDifficulty {Basico, Intermedio, Avanzado}
    
    [HeaderAttribute("set Question Number to 1 or more")]
    public int questionID;
    public string respuesta;
    [SerializeField] QChoice correctChoice;
    [SerializeField] QType type;
    [SerializeField] QDifficulty difficulty;
    [SerializeField] UnityEngine.UI.Image[] optionImages;
    [SerializeField] VRAutomaticButton[] optionButtons;
    [SerializeField] float hideDelay = 1f;
    [SerializeField] UnityEngine.Events.UnityEvent normalAction;

    // Json variables
    [HideInInspector] public bool userAnsweredCorrectly;
	[HideInInspector] public int realAnswer;
	[HideInInspector] public int userTries;
	[HideInInspector] public float playerPoints;
	[HideInInspector] public int questionType;
	[HideInInspector] public string questionTypeDescription;
	[HideInInspector] public int questionDifficulty;
	[HideInInspector] public string questionDifficultyDescription;
	[HideInInspector] public int questionTotalPoints;
	[HideInInspector] public string questionText;
	[HideInInspector] public string question;
	[HideInInspector] public string[] optionTexts;
	[HideInInspector] public string optionsType;
    

	void Start() {
		Invoke ("addMe", 0.2f);

        Transform questionChild = transform.Find("QuestionOptions");
        /////questionChild.GetChild
        
    }
	void addMe() {
		QuestionManager.Instance.addQuestion (this, this.questionID-1);
	}
    public void Answer(int choice)
    {
        MyQuestionManager.RegisterAnswer(questionID, choice-1);    

        if (choice == (int)correctChoice)
        {
            optionImages[choice - 1].sprite = QuestionManager.Instance.correctSprites[choice - 1];
            QuestionManager.respuestasTotales++;
        }
        else
            optionImages[choice - 1].sprite = QuestionManager.Instance.wrongSprites[choice - 1];
        Invoke("InvokeAction", hideDelay);
        foreach (var item in optionButtons)
        {
            item.enabled = false;
        }
        
        

    }

    public void InvokeAction()
    {
        normalAction.Invoke();
    }
   

	private void updateJsonVariables() {
		QuestionManager q = QuestionManager.Instance;
		userAnsweredCorrectly = q.playerAnswers [questionID - 1];
		realAnswer = q.realAnswers [questionID - 1];
		userTries = q.tries [questionID - 1];
		playerPoints = userAnsweredCorrectly ? questionTotalPoints*(q.maxChancesPerQuestion-userTries+1)/q.maxChancesPerQuestion : 0;
		questionType = (int)type;
		questionTypeDescription = questionType == 0 ? "Inferencia" : questionType == 1 ? "Extraer información" : "Interpretar el sentido";
		questionDifficulty = (int)difficulty;
		questionDifficultyDescription = questionDifficulty == 0 ? "Básico" : questionDifficulty == 1 ? "Intermedio" : "Avanzado";
		questionTotalPoints = q.points [questionID - 1];
		questionText = q.questionTexts1 [questionID - 1];
		question = q.questionTexts2 [questionID - 1];
		//print (optionTexts.Length);
		//print (q.optionTexts.Length);
		//print (q.optionTexts [questionNumber - 1].Split ('/').Length);
		optionTexts = new string[q.optionTexts [questionID - 1].Split ('/').Length];

		if (q.optionTexts [questionID - 1].Trim() != "") {
			string[] split = q.optionTexts [questionID - 1].Split ('/');
			print ( split[0] );
			print ( q.optionTexts [questionID - 1].Split ('/')[0] );

			optionTexts [0] = q.optionTexts [questionID - 1].Split ('/') [0];
			optionTexts [1] = q.optionTexts [questionID - 1].Split ('/') [1];
			optionTexts [2] = q.optionTexts [questionID - 1].Split ('/') [2];
			optionsType = "text";
		} else {
			optionTexts = null;
			optionsType = "image";
		}
		addMe ();
	}

}
