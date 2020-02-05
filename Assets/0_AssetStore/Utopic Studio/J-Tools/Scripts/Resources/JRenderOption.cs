using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    public interface IRenderOptionFactory
    {
        JRenderOption[] BuildRenderOptions(JResource.ContentOption[] Options);
    }

    public abstract class JRenderOption : MonoBehaviour
    {
        public delegate void OnAnswerValueChangeSignature();
        public event OnAnswerValueChangeSignature OnAnswerValueChange;

        /// <summary>
        /// Option that owns the data on this render option.
        /// </summary>
        protected JResource.ContentOption OwningOption;

        //public abstract void Assign(Recurso.OpcionContenido Option);
        public abstract IRenderOptionFactory GetFactory();

        /// <summary>
        /// Should be called on the inheriting render option to assign the ContentOption that owns the Data, and configure the render option itself.
        /// </summary>
        /// <param name="Option"></param>
        public void Assign(JResource.ContentOption Option)
        {
            OwningOption = Option;
            OnOwningOptionChanged(Option);
        }

        /// <summary>
        /// Called when Assigning the ContentOption of this RenderOption, so the inheriting classes can configure their answer and any data.
        /// </summary>
        /// <param name="Option"></param>
        protected virtual void OnOwningOptionChanged(JResource.ContentOption Option)
        {
            //To implement
        }

        /// <summary>
        /// Updates the answer value on the ContentOption asigned to this Render object
        /// </summary>
        /// <param name="Data"></param>
        protected void AnswerValueChange(string Data)
        {
            if(OwningOption != null)
            {
                OwningOption.AnswerData = Data;

                if(OnAnswerValueChange != null)
                {
                    OnAnswerValueChange();
                }
            }
        }
    }
}
