using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace J
{
    public class JLogin : MonoBehaviour
    {
        /// <summary>
        /// Called when the system realizes that a login operation was succesfull OR that the game already holds valid seession information
        /// </summary>
        public UnityEvent OnLogged;

        /// <summary>
        /// Contains the user field data
        /// </summary>
        public InputField UserField;

        /// <summary>
        /// The password for the account
        /// </summary>
        public InputField PasswordField;

        /// <summary>
        /// Button that calls the login function, should be disabled until both input fields are setup
        /// </summary>
        public Button LoginButton;

        /// <summary>
        /// Contains the type of user that logs in.
        /// </summary>
        public Dropdown TypeDropdown;

        /// <summary>
        /// Shows the error when a login attempt fails
        /// </summary>
        public Text FeedbackText;

        /// <summary>
        /// Widget used for showing that login is in progress.
        /// </summary>
        public Image LoadingImage;

        /// <summary>
        /// If when starting, the login method should look for a valid previous session and use that instead
        /// </summary>
        public bool UsePreviousSession = true;

        /// <summary>
        /// If we should fetch the resource data as part of the login attempt.
        /// </summary>
        public bool AutomaticResourceFetch = true;

        /// <summary>
        /// Feedback color for the text when an error happens
        /// </summary>
        public Color FeedbackTextErrorColor = new Color(159.0f, 0, 0);

        /// <summary>
        /// Normal text feedback color
        /// </summary>
        public Color FeedbackTextColor = new Color(200.0f, 200.0f, 0200.0f);

        /// <summary>
        /// If the login should disable XR when starting
        /// </summary>
        public bool DisableXRForLogin = true;

        //Init
        private void Start()
        {
            LoginButton.interactable = false;
            FeedbackText.enabled = false;
            LoadingImage.enabled = false;

#if !UNITY_EDITOR
            if(DisableXRForLogin)
            {
                UnityEngine.XR.XRSettings.enabled = false;
            }
#endif 

            if (UsePreviousSession && JRemoteSession.Instance.SessionData.IsValidSession() && J.Instance.AppMode == ApplicationMode.Normal)
            {
                Debug.Log("Previous session found, continuing");
                OnLogged?.Invoke();
            }
        }

        //Called when the values of the text fields change, to validate and enable/disable the "Login" button
        public void OnInputFieldUpdated()
        {
            string RegexPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                       + "@"
                                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))\z";

            //Only enable Login button when both fields are valid
            if (Regex.IsMatch(UserField.text, RegexPattern) && PasswordField.text.Length > 0)
            {
                LoginButton.interactable = true;
            }
            else
            {
                LoginButton.interactable = false;
            }
        }

        //Calls the login method from the SessionManager
        public void PerformLogin()
        {
            //Prepare the UI
            JRemoteSession.SessionType Type = (JRemoteSession.SessionType) TypeDropdown.value;
            JRemoteSession.Instance.AsyncLogin(UserField.text, PasswordField.text, Type, true, OnLoggin, OnLoginFailed);

            StartLoginFeedback();
        }

        private void OnLoggin()
        {
            if(AutomaticResourceFetch && JResourceManager.Instance != null)
            {
                JResourceManager.Instance.OnFetchComplete.AddListener(OnManagerFetchComplete);
                JResourceManager.Instance.Fetch();

                //Change feedback
                FeedbackText.text = "Descargando información de curso...";
            }
            else
            {
                NotifyLogged();
                StopLoginFeedback();
            }
        }

        private void NotifyLogged()
        {
#if !UNITY_EDITOR
            if(DisableXRForLogin)
            {
                UnityEngine.XR.XRSettings.enabled = true;
            }
#endif

            OnLogged?.Invoke();
        }

        private void OnLoginFailed(int Code, string Error)
        {
            FeedbackText.enabled = true;
            FeedbackText.color = FeedbackTextErrorColor;
            FeedbackText.text = "No se pudo iniciar sesión.";
            StopLoginFeedback();
        }

        private void OnManagerFetchComplete()
        {
            //Remove listener, no longer needed
            JResourceManager.Instance.OnFetchComplete.RemoveListener(OnManagerFetchComplete);
            NotifyLogged();
            StopLoginFeedback();
        }

        private void StartLoginFeedback()
        {
            //Prepare UI
            FeedbackText.enabled = true;
            FeedbackText.color = FeedbackTextColor;
            FeedbackText.text = "Iniciando sesión...";
            LoginButton.interactable = false;
            LoadingImage.enabled = true;
            StartCoroutine(SpinLoginWidget());
        }

        private void StopLoginFeedback()
        {
            StopCoroutine(SpinLoginWidget());
            LoginButton.interactable = true;
            LoadingImage.enabled = false;
        }

        private IEnumerator SpinLoginWidget()
        {
            //Speed per frame, defined statically
            float Speed = 250.0f;
            while(true)
            {
                LoadingImage.rectTransform.Rotate(Vector3.back, Speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

}
