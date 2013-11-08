using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchRobot.Library.Global
{
    public sealed class TextContent : Dictionary<String, String>
    {
        private static readonly Lazy<TextContent> lazy = new Lazy<TextContent>(() => new TextContent());
    
        public static TextContent Instance { get { return lazy.Value; } }

        private TextContent()
        {
            this["Simulation-Button-Load"] = "Load";
            this["Simulation-Button-Start"] = "Start";
            this["Simulation-Button-Pause"] = "Pause";
            this["Simulation-Button-Reset"] = "Reset";
            this["Simulation-Button-Resume"] = "Resume";
        }
    }
}
