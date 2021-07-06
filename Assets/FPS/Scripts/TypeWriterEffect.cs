using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum Puzzle
{
    lockpick,
    tilt,
    door,
    phone,
    sorting,
    news,
    word,
    wires,
    books
}
public enum ScenarionLine
{
    death,
    blood,
    scuffMark,
    stuck
}
public class TypeWriterEffect : MonoBehaviour
{
    //make instance 


    private static TypeWriterEffect _instance;

    public static TypeWriterEffect Instance
    {
        get
        {
            return _instance;
        }
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }




    
    [System.Serializable]
    public class PuzzleLine
    {
        public string line;
        public Puzzle selectPuzzle;
    }
    

    

    [System.Serializable]
    public class PuzzleLineAfterComplete
    {
        public string line;
        public GameObject clue;
        public Puzzle selectPuzzle;
    }

   

    [System.Serializable]
    public class ScenarioLine
    {
        string line;
        public Puzzle selectPuzzle;
    }




    [Header("TypeWriter Parameters")]
    [SerializeField] float textSpeed;
    [SerializeField] float delay;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] bool debugtext;
    string currentText;
    bool erazeText;

    public PuzzleLine[] puzzleLine;
    public PuzzleLineAfterComplete[] afterPuzzle;
    public ScenarioLine[] scenarioLine;

    [Header("scenario Parameters")]
    float idleTimeBeforeClue;


    int tempCount = 0;
    int tempoCount = 0;
   // string[] string;
    List<string> temp = new List<string>();
    List<string> tempo = new List<string>();

    bool writeText;
    private void Start()
    {
        for (int i = 0; i < afterPuzzle.Length; i++)
        {
            if (afterPuzzle[i].clue)
            {
                afterPuzzle[i].clue.SetActive(false);
            }
            else Debug.Log(i);
           
        }
    }
    public void RunningPuzzle(Puzzle puzzle)
    {
        inProgress = false;
        temp.Clear();
        tempCount = 0;
        text.text = "";
        currentText = "";
        switch (puzzle)
        {
            case Puzzle.books:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if(puzzleLine[i].selectPuzzle == Puzzle.books)
                    {
                        tempCount++;
                        //temp[i] = (string)puzzleLine[i].line;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.door:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.door)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.lockpick:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.lockpick)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.news:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.news)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.phone:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.phone)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.sorting:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.sorting)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.tilt:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.tilt)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.wires:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.wires)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;



            case Puzzle.word:
                for (int i = 0; i < puzzleLine.Length; i++)
                {
                    if (puzzleLine[i].selectPuzzle == Puzzle.word)
                    {
                        tempCount++;
                        temp.Add(puzzleLine[i].line);
                    }
                }
                break;
        }
        
        if (temp.Count > 1)
        {
            int pick = Random.Range(0, (temp.Count - 1));
            StartCoroutine(TypeText(temp[pick]));
        }
        else if (temp.Count < 0)
        {
            Debug.Log("mistake here");
        }
        else
        {
            StartCoroutine(TypeText(temp[0]));
        }
        
        //temp = null;
    }
    public void ClosingPuzzle()
    {
        temp.Clear();
        text.text = "";
        tempo.Clear();
        currentText = "";
        erazeText = true;
    }

    public void CompletedPuzzle(Puzzle puzzle)
    {
        inProgress = false;
        tempo.Clear();
        tempoCount = 0;
        text.text = "";
        currentText = "";
        GameObject obj = null;
        switch (puzzle)
        {
            case Puzzle.books:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.books)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.door:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.door)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.lockpick:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.lockpick)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.news:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.news)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.phone:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.phone)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.sorting:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.sorting)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.tilt:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.tilt)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.wires:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.wires)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;



            case Puzzle.word:
                for (int i = 0; i < afterPuzzle.Length; i++)
                {
                    if (afterPuzzle[i].selectPuzzle == Puzzle.word)
                    {
                        tempoCount++;
                        obj = afterPuzzle[i].clue;
                        tempo.Add(afterPuzzle[i].line);
                    }
                }
                break;
        }
        inProgress = false;
        //erazeText = false;
        if (tempo.Count > 1)
        {
           
            int pick = Random.Range(0, (tempoCount - 1));
            StartCoroutine(TypeTextCompeted(tempo[pick], obj));
        }
        else if (tempo.Count < 0)
        {
            Debug.Log("mistake here");
        }
        else
        {
            StartCoroutine(TypeTextCompeted(tempo[0], obj));
        }
        Debug.Log(tempo[0]);
    }

    bool inProgress;
    IEnumerator TypeText(string textToType)
    {
        inProgress = true;
        foreach (char letter in textToType.ToCharArray())
        {
            yield return new WaitForSeconds(textSpeed);
            if (anotherOne)
            {

                //tempo.Clear();
                //tempoCount = 0;
                //text.text = "";
                //textToType = "";
                //currentText = "";
                //inProgress = false;
                Debug.Log("E");
                yield break;

            }
            else
            {
                text.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
            
        }

        inProgress = false;


    }
    bool anotherOne;
    IEnumerator TypeTextCompeted(string textToType, GameObject obj)
    {
        anotherOne = true;
        text.text = "";
        foreach (char letter in textToType.ToCharArray())
        {
            if (!inProgress)
            {
                text.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
        }
        if (!inProgress)
        { 
            yield return new WaitForSeconds(2f);
            text.text = "";
        }
        //if (erazeText)
        //{
        //    erazeText = false;
        //}
        if(obj != null)
        {
            for (int i = 0; i < afterPuzzle.Length; i++)
            {
                if (obj == afterPuzzle[i].clue)
                {
                    afterPuzzle[i].clue.SetActive(true);
                }
            }
        }
        
        
        anotherOne = false;
    }

}
