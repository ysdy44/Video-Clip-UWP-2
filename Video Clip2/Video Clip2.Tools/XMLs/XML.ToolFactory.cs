using System.Collections.Generic;
using Video_Clip2.Tools.Models;

namespace Video_Clip2.Tools
{
    public static partial class XML
    {

        private static readonly IDictionary<ToolType, ITool> Tools = new Dictionary<ToolType, ITool>();

        public static ITool CreateTool(ToolType type)
        {
            if (XML.Tools.ContainsKey(type)) return XML.Tools[type];

            if (type != ToolType.None)
            {
                // TODO: Temp Code
                switch (type)
                {
                    case ToolType.Cursor:
                        {
                            ITool tool = new CursorTool();
                            XML.Tools.Add(type, tool);
                            return tool;
                        }
                }
            }

            return new NoneTool();
        }

    }
}