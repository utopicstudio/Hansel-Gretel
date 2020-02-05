using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using J;

public class JRenderOption_PairsFactory : IRenderOptionFactory
{
    private GameObject Blueprint;

    public JRenderOption_PairsFactory(GameObject InBlueprint)
    {
        if (!InBlueprint.GetComponent<JRenderOption_Pairs>())
        {
            Debug.LogError("Expected RenderOption_Pairs component on Blueprint GameObject, not found");
        }
        else
        {
            Blueprint = InBlueprint;
        }
    }

    public JRenderOption[] BuildRenderOptions(JResource.ContentOption[] Options)
    {
        List<JRenderOption_Pairs> RenderOptions = new List<JRenderOption_Pairs>();

        //First pass, create the RenderOptions and assign the first Port
        foreach (JResource.ContentOption Opt in Options)
        {
            GameObject instantiated = GameObject.Instantiate(Blueprint);
            JRenderOption_Pairs pair = instantiated.GetComponent<JRenderOption_Pairs>();
            RenderOptions.Add(pair);

            pair.Assign(Opt);
            pair.SetupPort(JRenderOption_Pairs.PortLocation.Left, Opt);
        }

        //Second pass, rumble the options and assign the second port 
        System.Random rnd = new System.Random();
        JResource.ContentOption[] RandOpts = Options.OrderBy(x => rnd.Next()).ToArray();

        for (int i = 0; i < RandOpts.Length; i++)
        {
            JResource.ContentOption opt = RandOpts[i];
            JRenderOption_Pairs pair = RenderOptions[i];

            pair.SetupPort(JRenderOption_Pairs.PortLocation.Right, opt);
        }

        //Third Pass, must ask the render options to setup their answers
        JRenderOption_Pairs[] Pairs = RenderOptions.ToArray();
        foreach (JRenderOption_Pairs Pair in Pairs)
        {
            Pair.ReconnectContentAnswer(Pairs);
        }

        return RenderOptions.ToArray();
    }
}

[AddComponentMenu("J/Resources/RenderOptions/RenderOption_Pairs")]
public class JRenderOption_Pairs : JRenderOption {

    enum PortType
    {
        Unknown,
        Text,
        Image
    }

    public enum PortLocation
    {
        Left,
        Right
    }
    
    //Left side
    public UnityEngine.UI.Text LabelLeft;
    public UnityEngine.UI.Image ImageLeft;

    public UnityEngine.UI.Text LabelRight;
    public UnityEngine.UI.Image ImageRight;

    public JSocket LeftSocket;
    public JSocket RightSocket;

    [SerializeField]  private PortType LeftPortType = PortType.Text;
    [SerializeField]  private PortType RightPortType = PortType.Text;

    //Setup any bindings
    private void Awake()
    {
        LeftSocket.OnSocketConnectionChanged.AddListener(OnLeftPortValueChange);
    }

    public override IRenderOptionFactory GetFactory()
    {
        return new JRenderOption_PairsFactory(this.gameObject);
    }

    /// <summary>
    /// Attempts to connect the ports using the pre-existing answers on the content option
    /// </summary>
    /// <param name="Pairs"></param>
    public void ReconnectContentAnswer(JRenderOption_Pairs[] Pairs)
    {
        int Value;
        bool bParsed = int.TryParse(OwningOption.AnswerData, out Value);
        if (bParsed && Value < Pairs.Length)
        {
            //Answers are from Left to Right socket
            LeftSocket.TryConnectSocket(Pairs[Value].RightSocket);
        }
    }

    private void OnLeftPortValueChange()
    {
        //Get the new value
        string str = LeftSocket.OutPort?.CustomData;
        AnswerValueChange(str);
    }
    
    public void SetupPort(PortLocation Location, JResource.ContentOption Opcion)
    {
        PortType type = GetPortTypeForLocation(Location, Opcion);

        UnityEngine.UI.Text Label = null;
        string data = "";
        
        //Obtain the correct label to use and setup custom data for bindings
        switch(Location)
        {
            case PortLocation.Left:
                Label = LabelLeft;
                data = Opcion.GetValueAsString("texto");
                LeftSocket.CustomData = Opcion.Index.ToString();
                break;
            case PortLocation.Right:
                Label = LabelRight;
                data = Opcion.GetValueAsString("texto_secundario");
                RightSocket.CustomData = Opcion.Index.ToString();
                break;
        }

        switch(type)
        {
            case PortType.Text:
                Label.text = data;
                break;
            case PortType.Image:
                Label.text = "Cargando";

                //Clear escaped strings
                data = data.Replace("\\", "");

                //Request a download
                BeginLoadPortImage(data, Location);

                break;
            default:
                break;
        }
        
    }

    private PortType GetPortTypeForLocation(PortLocation location, JResource.ContentOption option)
    {
        return location == PortLocation.Left ? LeftPortType : RightPortType;
    }

    private void BeginLoadPortImage(string url, PortLocation Location)
    {
        StartCoroutine(LoadImage(url, Location));
    }

    private IEnumerator LoadImage(string ImageUrl, PortLocation Location)
    {
        string BaseURL = JResourceManager.Instance.ImageRepositoryURL;
        string URL = BaseURL + "/" + ImageUrl;
        Debug.Log("Starting Load Image from url: " + URL + " on port location: " + Location);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL))
        {
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                Debug.LogError(www.error);
            }
            else
            {
                FinishLoadImage(DownloadHandlerTexture.GetContent(www), Location);
            }
        }
    }

    private void FinishLoadImage(Texture2D Tex, PortLocation Location)
    {
        UnityEngine.UI.Image Img = Location == PortLocation.Left ? ImageLeft : ImageRight;
        Img.overrideSprite = Sprite.Create(Tex, new Rect(0, 0, Tex.width, Tex.height), new Vector2(0, 0));

        //Remember to hide the text showing "Loading"
        UnityEngine.UI.Text Label = Location == PortLocation.Left ? LabelLeft : LabelRight;
        Label.text = "";

        Debug.Log("Image Loaded: " + Tex);
    }
}
