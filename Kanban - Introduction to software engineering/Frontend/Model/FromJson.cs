using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend.ModelView;
using IntroSE.Kanban.Backend.ServiceLayer;
using Newtonsoft.Json;

namespace Frontend.Model
{
    internal class FromJson
    {
        
        // 
        public static string Deserialise(string response)
        {
            // Deserialize JSON from string
            return JsonConvert.DeserializeObject<string>(response);
        }
    }
}

