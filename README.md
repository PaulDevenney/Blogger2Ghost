# Blogger2Ghost.NET
A .NET Windows cmdline tool for converting Blogger XML exports to Ghost json Import files

###Usage
```
blogger2ghost <pathtobloggerexportfile> <pathforghostjsonimportfile>
```


##V0.1

* Works with Ghost 0.7.9 (Ghost export file version 004)
* Posts are successfully converted. The original HTML is not converted in any way

##Missing/Potential Future Features

* Blogger terms are not yet migrated to tags / matched to the original posts
* Post Images stored on blogger are not downloaded to disk
* No html is cleansed into markdown (e.g. change image links to `![]` format, remove `<br>` tags
* Better general error handling