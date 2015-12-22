using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace K2.MonkeyLearn
{
   
    public class Class1
    {
    
       public static string getSentiment(string URI, string Authorization, string text)
       {
           System.Net.WebResponse resp = null;
           try
           {
               string jsonString = "{\"text_list\": [\"" + text + "\"]}";
               byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

               System.Net.WebRequest req = System.Net.WebRequest.Create(URI);

               //Add these, as we're doing a POST
               req.ContentType = "application/json";
               req.Method = "POST";
               req.Headers.Add("Authorization", Authorization);
               req.ContentLength = bytes.Length;

               System.IO.Stream os = req.GetRequestStream();
               os.Write(bytes, 0, bytes.Length); //Push it out there
               os.Close();
               resp = req.GetResponse();
               if (resp == null)
                   return null;
               System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());

               //{"result": [[{"probability": 1.0, "label": "negative"}]]}

               dynamic jsonDe = JsonConvert.DeserializeObject(sr.ReadToEnd().Trim().ToString());
               string result = String.Format("{0}", jsonDe["result"][0][0]["label"]);
               return result;
           }
           catch (Exception e)
           {
               if (resp != null)
               {
                   System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                   throw new Exception(sr.ReadToEnd().Trim(), e);
               }
               else
                   throw new Exception("URI:" + URI + "|Authorization:" + Authorization + "|text:" + text, e);
           }
       }
        
    }
}
