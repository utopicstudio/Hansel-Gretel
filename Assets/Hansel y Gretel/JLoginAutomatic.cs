using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text.RegularExpressions;

namespace J
{
    public class JLoginAutomatic : MonoBehaviour
    {
        /// <summary>
        /// Called when the system realizes that a login operation was succesfull OR that the game already holds valid seession information
        /// </summary>
        public UnityEvent OnLogged;

        public string user;
        public string pswd;
        public JRemoteSession.SessionType userType;
        public bool onStart = true;
        

        /// <summary>
        /// If when starting, the login method should look for a valid previous session and use that instead
        /// </summary>
        public bool UsePreviousSession = true;

        //Init
        private void Start()
        {
            if (onStart)
                this.PerformLogin();
            if (UsePreviousSession && JRemoteSession.Instance.SessionData.IsValidSession())
            {
                OnLogged?.Invoke();
            }
        }

        //Called when the values of the text fields change, to validate and enable/disable the "Login" button
        public void OnInputFieldUpdated()
        {
            
        }

        //Calls the login method from the SessionManager
        public void PerformLogin()
        {
            JRemoteSession.Instance.AsyncLogin(user, pswd, userType, true, OnLoggin, OnLoginFailed);
        }

        private void OnLoggin()
        {
            OnLogged?.Invoke();
        }

        private void OnLoginFailed(int Code, string Error)
        {
            
        }
    }

}
