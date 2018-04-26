using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Resources;

namespace ResXMergeTool
{
    public class FileParser
    {
        private Dictionary<string, ResXConflictNode> mConflicts = new Dictionary<string, ResXConflictNode>();
        private Dictionary<string, ResXSourceNode> mOutput = new Dictionary<string, ResXSourceNode>();
        private String WorkingPath = Directory.GetCurrentDirectory();

        public SortedList<string, ResXConflictNode> NodeConflicts
        {
            get => new SortedList<string, ResXConflictNode>(mConflicts);
        }
        public SortedList<string, ResXSourceNode> OriginNodes
        {
            get => new SortedList<string, ResXSourceNode>(mOutput);
        }

        public FileParser(string path)
        {
            WorkingPath = path;
        }

        public static ResXSourceType GetResXSourceTypeFromFileName(string file)
        {

            if (file.Contains(".resx", StringComparison.OrdinalIgnoreCase))
            {
                if (file.Contains("base", StringComparison.OrdinalIgnoreCase))
                    return ResXSourceType.BASE;
                else if (file.Contains("local", StringComparison.OrdinalIgnoreCase))
                    return ResXSourceType.LOCAL;
                else if (file.Contains("remote", StringComparison.OrdinalIgnoreCase))
                    return ResXSourceType.REMOTE;
                else return ResXSourceType.UNKOWN;
            }
            else return ResXSourceType.UNKOWN;
        }

        /// <summary>
        /// Parses all given files and compares them with other read files
        /// Code by B.O.B. (https://www.codeproject.com/script/Membership/View.aspx?mid=3598385) under CPOL License (http://www.codeproject.com/info/cpol10.aspx)
        /// </summary>
        /// <param name="files"></param>
        public void ParseResXFiles(params string[] files)
        {
            foreach (String fileName in files)
            {
                String name;
                ResXSourceType rSource = ResXSourceType.CONFLICT;

                // get relative or absolute file path (as proposed by https://www.codeproject.com/Messages/5323362/Some-fixes.aspx)
                String file = Path.Combine(WorkingPath, fileName);

                rSource = GetResXSourceTypeFromFileName(fileName);
                if (rSource == ResXSourceType.UNKOWN)
                    return;

                ResXResourceReader resx = new ResXResourceReader(file)
                {
                    UseResXDataNodes = true
                };

                IDictionaryEnumerator dict = resx.GetEnumerator();


                while (dict.MoveNext())
                {
                    ResXDataNode node = (ResXDataNode)dict.Value;
                    name = node.Name.ToLower();

                    if (mConflicts.ContainsKey(name))
                    {

                        switch (rSource)
                        {
                            case ResXSourceType.BASE:
                                mConflicts[name].BaseNode = node;
                                break;
                            case ResXSourceType.LOCAL:
                                mConflicts[name].LocalNode = node;
                                break;
                            case ResXSourceType.REMOTE:
                                mConflicts[name].RemoteNode = node;
                                break;
                            default:
                                continue;
                        }
                    }
                    else
                    {
                        if (mOutput.ContainsKey(name))
                        {
                            object objA = mOutput[name].Node.GetValue((ITypeResolutionService)null);
                            object objB = node.GetValue((ITypeResolutionService)null);
                            bool objE = false;

                            objE = string.Equals(objA.GetType().ToString(), objB.GetType().ToString());
                            if (objE && objA is string)
                                objE = string.Equals(objA, objB);

                            if (objE && string.Equals(mOutput[name].Node.Name, node.Name) && string.Equals(mOutput[name].Node.Comment, node.Comment))
                                mOutput[name].Source = mOutput[name].Source | rSource;
                            else
                            {
                                switch (mOutput[name].Source)
                                {
                                    case ResXSourceType.BASE:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, null, null));
                                        break;
                                    case ResXSourceType.BASE_LOCAL:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, mOutput[name].Node, null));
                                        break;
                                    case ResXSourceType.BASE_REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(mOutput[name].Node, null, mOutput[name].Node));
                                        break;
                                    case ResXSourceType.LOCAL:
                                        mConflicts.Add(name, new ResXConflictNode(null, mOutput[name].Node, null));
                                        break;
                                    case ResXSourceType.LOCAL_REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(null, mOutput[name].Node, mOutput[name].Node));
                                        break;
                                    case ResXSourceType.REMOTE:
                                        mConflicts.Add(name, new ResXConflictNode(null, null, mOutput[name].Node));
                                        break;
                                    default:
                                        continue;
                                }

                                switch (rSource)
                                {
                                    case ResXSourceType.BASE:
                                        mConflicts[name].BaseNode = node;
                                        break;
                                    case ResXSourceType.LOCAL:
                                        mConflicts[name].LocalNode = node;
                                        break;
                                    case ResXSourceType.REMOTE:
                                        mConflicts[name].RemoteNode = node;
                                        break;
                                    default:
                                        continue;
                                }
                            }
                        }
                        else
                            mOutput.Add(name, new ResXSourceNode(rSource, node));
                    }
                }

            }
        }
    }
}
