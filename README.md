## ResxMergeTool
Originally forked from https://www.codeproject.com/Articles/1145375/ResX-Merge-Tool as of version published on 24 Oct 2016, then modified and extended with a better GUI a.o.t.

### What does it do?
It compares typical merge conflicted .NET Resource (.resx) files through BASE, LOCAL and REMOTE and starts alternative merging tool (TortoiseGit Merge or KDiff3) if not all three files are given

### How do I use it?
For this you have multiple possibilities:
* Start the executable without console or without parameters, it will then show a GUI where you select your BASE, LOCAL and REMOTE file together with the desired output file
* Let the command line interpreter handle your merge, e.g. GitExtensions in Visual Studio (see codeproject article for reasons to do that)  
Call it as follows:  
`ResXMergeTool.exe File.resx.BASE[.resx] File.resx.LOCAL[.resx] File.resx.REMOTE[.resx] -o File.resx`  
Note that on any other usage KDIFF or TortoiseGit Merge are called. If Tortoise Git Merge is called, either the first two arguments will be passed as MINE and THEIRS or the first three arguments as BASE, MINE and THEIRS

### Why?
Again: Have a look at the original codeproject article, the author wrote it very well  
Basically, Visual Studio messes up Resource files sometimes, which makes a normal diff unpossible and often results in a unresolvable merge conflict
