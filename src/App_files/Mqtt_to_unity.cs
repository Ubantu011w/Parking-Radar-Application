using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples
{
    public class Mqtt_to_unity : M2MqttUnityClient
    {
        [Tooltip("Set this to true to perform a testing cycle automatically on startup")]
        [Header("User Interface")]
        public InputField consoleInputField;

        public InputField addressInputField;
        public InputField portInputField;
        public InputField topicInputField;
        public InputField userInputField;
        public InputField passwordInputField;

        public Text status;

        public InputField distance1;
        public InputField distance2;
        public InputField distance3;
        public int distance1_int;
        public int distance2_int;
        public int distance3_int;

        private List<string> eventMessages = new List<string>();

        
        // Följande funktioner körs när vi skriver in något i inputfields i "Settings" panellen.
        public void SetBrokerAddress()
        {
            if (addressInputField)
            {
                this.brokerAddress = addressInputField.text;
            }
        }
        public void SetBrokerUsername() 
        {
            if (addressInputField)
            {
                this.mqttUserName = userInputField.text;
            }
        }
        public void SetBrokerPassword()
        {
            if (passwordInputField)
            {
                this.mqttPassword = passwordInputField.text;
            }
        }
        public void SetBrokerPort()
        {
            if (portInputField)
            {
                int.TryParse(portInputField.text, out this.brokerPort);
            }
        }

         // Ändrar värdet på status i Settings panel.
        public void SetUiMessage(string msg)
        {
            if (status.text != null)
            {
                
                status.text = msg;
            }
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            SetUiMessage("Connected to broker on " + brokerAddress + "\n");

        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { topicInputField.text }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { topicInputField.text });
        }


        protected override void OnConnectionFailed(string errorMessage)
        {
            SetUiMessage("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            SetUiMessage("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            SetUiMessage("CONNECTION LOST!");
        }

                
        //När appen startas.
        protected override void Start()
        {
            retrieve(); 
            SetUiMessage("Ready.");
            base.Start();
        }      

        // Konvertera värdet vi får till en läsbar text.
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            StoreMessage(msg);

        }

        // Addera texten till en lista
        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        /*Här processerar vi "msg" vi får via mqtt.
        Vi Loopar igenom "msg". Om vi hittar ";" då vet vi att värdet 
        på första distansen är slut.
        Sen assignar vi denna till en "local" array av strings och till 
        en global int värde så att vi kan skicka
        den till UI_scripting.cs och jämför mellan värdena.*/
        private void ProcessMessage(string msg)
        {
            SetUiMessage("Received: " + msg);
            string[] distances = {"0", "0", "0"};
            string divide="";

            foreach (char c in msg)
            {

            if (c.ToString()==";")
                {
                if (distances[0]=="0")
                {
                    distances[0]=divide;
                    distance1_int= int.Parse(divide);
                    if (distance1_int<=250)
                        {
                            distance1.text=distances[0] + " cm";
                        }
                    else
                        {
                            distance1.text=">250 cm";
                        }
                        

                    divide="";
                }
                else if (distances[1]=="0")
                {
                    distances[1]=divide;
                    distance2_int= int.Parse(divide);
                    if (distance2_int<=250)
                        {
                            distance2.text=distances[1] + " cm";
                        }
                    else
                        {
                            distance2.text=">250 cm";
                        }

                    divide="";
                }
                else if (distances[2]=="0")
                {
                    distances[2]=divide;
                    distance3_int= int.Parse(divide);
                    if (distance3_int<=250)
                        {
                            distance3.text=distances[2] + " cm";
                        }
                    else
                        {
                            distance3.text=">250 cm";
                        }
                    divide="";
                }
                divide="";
                }
            else
                {
                divide+=c;
                }

            }
        }

        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()
            
            // Om listan som har msg har mer än ett värde då kör vi funktionen ProcessMessage().
            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }

        }

        private void OnDestroy() // Om vi stänger appen.
        {
            Disconnect();
        }

        public void clear() //Rensar ut alla inputs om vi trycker på knappen "Clear".
        {
            addressInputField.text = "";
            userInputField.text = "";
            passwordInputField.text = "";
            portInputField.text = "";
            topicInputField.text = "";
        }

        public void retrieve()   //Fyller upp alla inputfields om vi trycker på knappen "Retrieve".
        {
            addressInputField.text = "io.adafruit.com";
            userInputField.text = "ubantu011";
            passwordInputField.text = "aio_btUr85N6wp5RGRCgGfnkV8poj1pr";
            portInputField.text = "1883";
            topicInputField.text = "ubantu011/feeds/parking";
        }

        
    }
}
