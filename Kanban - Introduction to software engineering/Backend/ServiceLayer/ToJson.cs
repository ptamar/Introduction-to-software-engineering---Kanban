using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ToJson
    {
        /// <summary>
        /// A class to convert a response object into a JSON
        /// </summary>
        public static string toJson(Response i)
        {
            return JsonConvert.SerializeObject(i,
                Formatting.Indented,
                new JsonSerializerSettings{NullValueHandling = NullValueHandling.Ignore});
            //return JsonSerializer.Serialize<Response>(i); 
            // Which one should we use?
        }
        
    }
}
