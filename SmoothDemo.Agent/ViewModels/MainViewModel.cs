using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothDemo.Agent
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Actions = new List<SmoothDemo.Agent.Models.Action>();
            Tokens = new List<string>();
        }

        public List<SmoothDemo.Agent.Models.Action> Actions { get; set; }
        public List<string> Tokens { get; set; }
        public int ActionIndex { get; set; }
        public string ContentToShow { get; set; }
        public string ScriptFileName { get; set; }


        public void Init()
        {
            Tokens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(ConfigurationManager.AppSettings["tokens.json"]));
            Actions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmoothDemo.Agent.Models.Action>>(File.ReadAllText(ConfigurationManager.AppSettings["script.json"]));
        }


    }
}
