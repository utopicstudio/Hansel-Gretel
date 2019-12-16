using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    public class JRenderOption_AssertionFactory : IRenderOptionFactory
    {
        private GameObject Blueprint;

        public JRenderOption_AssertionFactory(GameObject InBlueprint)
        {
            if (!InBlueprint.GetComponent<JRenderOption_Assertion>())
            {
                Debug.LogError("Expected URenderOption_Assertion component on Blueprint GameObject, not found");
            }
            else
            {
                Blueprint = InBlueprint;
            }
        }

        public JRenderOption[] BuildRenderOptions(JResource.ContentOption[] Options)
        {
            List<JRenderOption> RenderOptions = new List<JRenderOption>();

            foreach (JResource.ContentOption Opt in Options)
            {
                GameObject instantiated = GameObject.Instantiate(Blueprint);
                JRenderOption_Assertion alternative = instantiated.GetComponent<JRenderOption_Assertion>();

                alternative.Assign(Opt);
                RenderOptions.Add(alternative);
            }

            return RenderOptions.ToArray();
        }
    }

    [AddComponentMenu("J/Resources/RenderOptions/RenderOption_Assertion")]
    public class JRenderOption_Assertion : JRenderOption
    {

        public UnityEngine.UI.Toggle Toggle_True;
        public UnityEngine.UI.Toggle Toggle_False;
        public UnityEngine.UI.Text Label;

        private void Awake()
        {
            //Need to have bindings onto the Toggle
            Toggle_True.onValueChanged.AddListener(OnToggleValueChange);
            Toggle_False.onValueChanged.AddListener(OnToggleValueChange);
        }

        private void OnToggleValueChange(bool Active)
        {
            AnswerValueChange(Toggle_True.isOn.ToString());
        }

        public override IRenderOptionFactory GetFactory()
        {
            return new JRenderOption_AssertionFactory(this.gameObject);
        }

        protected override void OnOwningOptionChanged(JResource.ContentOption Option)
        {
            //We should be assigned to a toggle, so we can search it and init the values
            Label.text = Option.GetValueAsString("texto");

            //Check for answer
            bool bIsTrue;
            if(bool.TryParse(Option.AnswerData, out bIsTrue))
            {
                Toggle_True.isOn = bIsTrue;
                Toggle_False.isOn = !bIsTrue;
            }
            else
            {
                Toggle_True.isOn = false;
                Toggle_False.isOn = false;
            }
            
        }
    }

}