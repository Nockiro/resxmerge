using System.Resources;

namespace ResXMergeTool
{
    public class ResXConflictNode
    {
        public ResXDataNode BaseNode;
        public ResXDataNode LocalNode;

        public ResXDataNode RemoteNode;
        public ResXConflictNode(ResXDataNode @base, ResXDataNode local, ResXDataNode remote)
        {
            BaseNode = @base;
            LocalNode = local;
            RemoteNode = remote;
        }
    }
}
