using System;
using System.Resources;

namespace ResXMergeTool
{
    [Flags()]
    public enum ResXSource
    {
        CONFLICT = 0,
        BASE = 1,
        LOCAL = 2,
        REMOTE = 4,
        MANUAL = 8,
        BASE_LOCAL = 3,
        BASE_REMOTE = 5,
        LOCAL_REMOTE = 6,
        ALL = 7
    }
    public class ResXSourceNode
    {
        public ResXSource Source;

        public ResXDataNode Node;
        public ResXSourceNode(ResXSource source, ResXDataNode node)
        {
            this.Source = source;
            this.Node = node;
        }
    }
}
