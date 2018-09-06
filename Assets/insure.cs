using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace unity {

    public class insure : MonoBehaviour
    {
        public TextMesh text_mesh;
        void Start()
        {
            this.CreateQuote();
        }

        [System.Serializable]
        public class Quotes
        {
            public Quote[] values;
        }

        [System.Serializable]
        public class Quote
        {
            public string package_name;
            public string sum_assured;
            public int base_premium;
            public string suggested_premium;
            public string created_at;
            public string quote_package_id;
            public QuoteModule module;
        }

        [System.Serializable]
        public class PolicyHolder
        {
            public string first_name;
            public string last_name;
        }

        [System.Serializable]
        public class Identity
        {
            public string type;
            public string number;
            public string country;
        }

        [System.Serializable]
        public class QuoteModule
        {
            public string type;
            public string make;
            public string model;
        }

        [Serializable]
        public class Param
        {
            public Param(string _key, string _value)
            {
                key = _key;
                value = _value;
            }

            public string key;
            public string value;
        }

        public void CreateQuote()
        {
            List<Param> parameters = new List<Param>();
            parameters.Add(new Param("type", "root_funeral"));
            parameters.Add(new Param("cover_amount", "10000000"));
            parameters.Add(new Param("age", "45"));
            parameters.Add(new Param("spouse_included", "false"));
            parameters.Add(new Param("children_included", "false"));
            parameters.Add(new Param("extended_family_included", "false"));
            
            StartCoroutine(CallAPICoroutine("https://sandbox.root.co.za/v1/insurance/quotes", parameters));
        }


        public IEnumerator CallAPICoroutine(String url, List<Param> parameters)
        {
            string auth = "sandbox_OGU5YzdjNGQtN2FhOC00N2UyLWI2MWEtOGM3MTRhMzRlM2ZhLmE4RVRRSHVxOE5wSGQ1QUpWS2xXcUp3RHoxalB2ak9y:";
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;

            WWWForm form = new WWWForm();

            foreach (var param in parameters)
            {
                form.AddField(param.key, param.value);
            }
            
            UnityWebRequest www = UnityWebRequest.Post(url, form);
            www.SetRequestHeader("AUTHORIZATION", auth);
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Quotes quote = JsonUtility.FromJson<Quotes>("{\"values\":" + www.downloadHandler.text + "}");
                string text = "***Make:" + quote.values[0].package_name;
                Debug.Log(text);
                this.CreatePolicyHolder(quote);
            }
            yield return true;
        }

        public class Id
        {
            public string type { get; set; }
            public string number { get; set; }
            public string country { get; set; }
        }

        public class CreatedBy
        {
            public string type { get; set; }
            public string id { get; set; }
            public string owner_id { get; set; }
        }

        public class PolicyHolderResponse
        {
            public string policyholder_id { get; set; }
            public string type { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public Id id { get; set; }
            public object email { get; set; }
            public object cellphone { get; set; }
            public string date_of_birth { get; set; }
            public string gender { get; set; }
            public DateTime created_at { get; set; }
            public object app_data { get; set; }
            public List<object> policy_ids { get; set; }
            public CreatedBy created_by { get; set; }
        }

        public IEnumerator CreatePolicyHolder(Quotes quote)
        {
            string auth = "sandbox_OGU5YzdjNGQtN2FhOC00N2UyLWI2MWEtOGM3MTRhMzRlM2ZhLmE4RVRRSHVxOE5wSGQ1QUpWS2xXcUp3RHoxalB2ak9y:";
            auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            auth = "Basic " + auth;

            UnityWebRequest www = UnityWebRequest.Post("https://sandbox.root.co.za/v1/insurance/policyholders", "{  \"id\": {    \"type\": \"id\",    \"number\": \"6802015800084\",    \"country\": \"ZA\"  },  \"first_name\": \"Erlich\",  \"last_name\": \"Bachman\"}");
            www.SetRequestHeader("AUTHORIZATION", auth);

            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                PolicyHolderResponse policyHolder = JsonUtility.FromJson<PolicyHolderResponse>("{\"values\":" + www.downloadHandler.text + "}");

                string text = "***Make:" + policyHolder.policyholder_id;
                Debug.Log(text);

                Debug.Log(JsonUtility.ToJson(policyHolder));
            }

            yield return true;
        }
    }

}