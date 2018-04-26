using System;
using System.Resources;

namespace ResXMergeTool
{
    [Flags()]
    public enum ResXSourceType
    {
        UNKOWN = -1,
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
        public ResXSourceType Source;

        public ResXDataNode Node;
        public ResXSourceNode(ResXSourceType source, ResXDataNode node)
        {
            this.Source = source;
            this.Node = node;
        }

        public static string GetStringFromEnum(ResXSourceType source)
        {

            switch (source)
            {
                case ResXSourceType.ALL: return "ALL";
                case ResXSourceType.BASE: return "BASE";
                case ResXSourceType.CONFLICT: return "XCONFLICT";
                case ResXSourceType.BASE_LOCAL: return "LOCAL";
                case ResXSourceType.BASE_REMOTE: return "REMOTE";
                case ResXSourceType.LOCAL: return "LOCAL ONLY";
                case ResXSourceType.LOCAL_REMOTE: return "BOTH";
                case ResXSourceType.MANUAL: return "MANUAL";
                case ResXSourceType.REMOTE: return "REMOTE ONLY";
                default: return "????";
            }

        }
    }
}
