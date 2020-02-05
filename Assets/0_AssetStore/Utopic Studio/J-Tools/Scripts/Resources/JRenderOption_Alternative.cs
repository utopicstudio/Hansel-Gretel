using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    public class JRenderOption_AlternativeFactory : IRenderOptionFactory
    {
        private GameObject Blueprint;

        public JRenderOption_AlternativeFactory(GameObject InBlueprint)
        {
            if (!InBlueprint.GetComponent<JRenderOption_Alternative>())
            {
                Debug.LogError("Expected URenderOption_Alternative component on Blueprint GameObject, not found");
            }
            else
            {
                Blueprint = InBlueprint;
            }
        }

        public JRenderOption[] BuildRenderOptions(JResource.ContentOption[] Options)
        {
            List<JRenderOption> RenderOptions = new List<JRenderOption>();

            UnityEngine.UI.ToggleGroup TGroup = null;
            for (int i = 0; i < Options.Length; i++)
            {
                JResource.ContentOption Opt = Options[i];
                GameObject instantiated = GameObject.Instantiate(Blueprint);
                JRenderOption_Alternative alternative = instantiated.GetComponent<JRenderOption_Alternative>();

                //We need some way to enforce the toggles to be setup one at a time, we use a toggle group on the first element
                if(i == 0)
                {
                    TGroup = instantiated.AddComponent<UnityEngine.UI.ToggleGroup>();
                    alternative.Toggle.group = TGroup;
                }
                else
                {
                    alternative.Toggle.group = TGroup;
                }

                alternative.Assign(Opt);
                RenderOptions.Add(alternative);
            }

            return RenderOptions.ToArray();
        }
    }

    [AddComponentMenu("J/Resources/RenderOptions/RenderOption_Alternative")]
    public class JRenderOption_Alternative : JRenderOption
    {

        public UnityEngine.UI.Toggle Toggle;
        public UnityEngine.UI.Text Label;
        public UnityEngine.UI.Text IndexLabel;

        private void Awake()
        {
            //Need to have bindings onto the Toggle
            Toggle.onValueChanged.AddListener(OnToggleValueChange);
        }

        private void OnToggleValueChange(bool Active)
        {
            AnswerValueChange(Active.ToString());
        }

        protected override void OnOwningOptionChanged(JResource.ContentOption Option)
        {
            //We should be assigned to a toggle, so we can search it and init the values
            Label.text = Option.GetValueAsString("texto");
            int asciiValue = (int)'A' + Option.Index;
            IndexLabel.text = ((char)asciiValue).ToString();

            //Check for answer
            bool bIsOn;
            bool bParseSuccess = bool.TryParse(Option.AnswerData,out bIsOn);
            Toggle.isOn = bParseSuccess ? bIsOn : false;
        }

        public override IRenderOptionFactory GetFactory()
        {
            return new JRenderOption_AlternativeFactory(this.gameObject);
        }

    }

}