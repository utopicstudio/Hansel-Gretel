using UnityEngine;
using System.Collections;

// Call functions of this class when the player answeres a question.
// example: QuestionManager.Instance.addAnswer(5,2);
// public: addAnswer(int,int), printAnswers()
public class QuestionManager : Singleton<QuestionManager> {
	protected QuestionManager () {}
    public static int respuestasTotales;
	public const int numOfOptions = 3;
	public int maxChancesPerQuestion = 3;
	[HeaderAttribute("Fill in the real answers in order:")]

	public int[] realAnswers;
	public int[] points;
	public bool[] playerAnswers;
	public int[] tries;

	public string[] questionTexts1;
	public string[] questionTexts2;
	public string[] optionTexts;

	public int numCorrect;
	public int numTotal;//TODO:asignar 6 o 7
	public float type1Percentage;
	public float type2Percentage;
	public float type3Percentage;

    public UnityEngine.Sprite[] correctSprites;
    public UnityEngine.Sprite[] wrongSprites;

    [HideInInspector] public QuestionJson[] questions;

	private int index_questions = 0;

	PlayerChoices[] playerChoices; //[answer to question 1, answer to question 2, ...]
	// where 1==option A , etc.


	void Awake () {
		questions = new QuestionJson[10]; //TODO. maximo 10 preguntas por ahora;
	}

	void Start () {
		DontDestroyOnLoad (this);
		Application.targetFrameRate = 60;

		playerChoices = new PlayerChoices[realAnswers.Length];
		playerAnswers = new bool[realAnswers.Length];
		tries = new int[realAnswers.Length];

		for (int i = 0; i<realAnswers.Length; i++) {
			playerChoices[i] = new PlayerChoices(maxChancesPerQuestion);
		}
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.P))
			QuestionManager.Instance.printAnswers ();
	}






	public void addQuestion(Question q, int q_index) {
		if (q_index < questions.Length) {
			if (questions[q_index] == null) {
				questions [q_index] = new QuestionJson ();
				questions[q_index].userAnsweredCorrectly = q.userAnsweredCorrectly;
				questions[q_index].realAnswer = q.realAnswer;
				questions[q_index].userTries = q.userTries;
				questions[q_index].playerPoints = q.playerPoints;
				questions[q_index].questionType = q.questionType;
				questions[q_index].questionTypeDescription = q.questionTypeDescription;
				questions[q_index].questionDifficulty = q.questionDifficulty;
				questions[q_index].questionDifficultyDescription = q.questionDifficultyDescription;
				questions[q_index].questionTotalPoints = q.questionTotalPoints;
				questions[q_index].questionText = q.questionText;
				questions[q_index].question = q.question;
				if (q.optionTexts != null && q.optionTexts.Length > 0) {
					questions [q_index].optionTexts = new string[3];
					questions [q_index].optionTexts [0] = q.optionTexts [0];
					questions [q_index].optionTexts [1] = q.optionTexts [1];
					questions [q_index].optionTexts [2] = q.optionTexts [2];
				} else {
					questions [q_index].optionTexts = null;
				}
				questions[q_index].optionsType = q.optionsType;
			}
		}
	}

	public void addAnswer(int questionID, int value) {
		tries [questionID - 1] += 1;

		playerChoices [questionID - 1].addAnswer (value);
		if (!playerAnswers [questionID - 1])
			playerAnswers [questionID - 1] = (value == realAnswers [questionID - 1]);
		this.questions [questionID - 1].userAnsweredCorrectly = (value == realAnswers [questionID - 1]);
		this.questions [questionID - 1].userTries = tries [questionID - 1];
		this.questions [questionID - 1].playerPoints = calculatePoints (tries [questionID - 1], this.points [questionID - 1], playerAnswers[questionID - 1]);
	}
	float calculatePoints(int tries, int maxPoints, bool answeredCorrectly) {
		float ret = maxPoints * (maxChancesPerQuestion - tries + 1) * 1.0f / maxChancesPerQuestion;
		return answeredCorrectly ? ret : 0f;
	}

	public bool isQuestionCorrectlyAnswered(int answerIndex) {
		return playerAnswers [answerIndex - 1];
	}
	public bool isQuestionWithTriesLeft(int answerIndex) {
		return tries [answerIndex - 1] < maxChancesPerQuestion;
	}

	public int getNumberOfCorrectAnswers() {
		int numCorrect = 0;
		for (int i = 0; i < playerAnswers.Length; i++) {
			if (playerAnswers [i])
				numCorrect++;
		}
		return numCorrect;
	}
	public int getNumberOfWrongAnswers() {
		return playerAnswers.Length - getNumberOfCorrectAnswers ();
	}

	public int getNumberOfTotalQuestions() {
		return realAnswers.Length;
	}

	public void printAnswers () {
		string s = "playerAnswers: ";
		foreach (var ans in playerAnswers) {
			string val = ans ? "true" : "-";
			s += val + " ";
		}
		s += "\n" + "realAnswers: ";
		foreach (var ans in realAnswers) {
			s += ans + " ";
		}
		Debug.Log (s);
	}


	private class PlayerChoices
	{
		public int[] answers;

		private int index;

		public PlayerChoices(int maxAnswers)
		{
			answers = new int[maxAnswers];
			index = 0;
		}
		public void addAnswer(int answer) {
			answers [index] = answer;
			index++;
		}
	}

	public class QuestionJson {
		public bool userAnsweredCorrectly;
		public int realAnswer;
		public int userTries;
		public float playerPoints;
		public int questionType;
		public string questionTypeDescription;
		public int questionDifficulty;
		public string questionDifficultyDescription;
		public int questionTotalPoints;
		public string questionText;
		public string question;
		public string[] optionTexts;
		public string optionsType;

		public QuestionJson() {
			this.optionTexts = new string[3];
		}
	}

}
