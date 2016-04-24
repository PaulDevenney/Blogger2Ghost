# Blogger2Ghost.NET
A .NET Windows cmdline tool for converting Blogger XML exports to Ghost json Import files

###Usage
```
blogger2ghost <pathtobloggerexportfile> <outputrootfolder>
```

* Import the `ghost.json` file found in the resulting sub folder
* Copy the images from the images folder into `/content/images/fromblogger/`

##V0.2
* Results are now output to a folder of the form `B2G_yyyyMMdd_HHmmss` underneath the output folder
* Images are now placed in a folder called `images` underneath this

##V0.1

* Works with Ghost 0.7.9 (Ghost export file version 004)
* Posts are successfully converted. `<br/>` tags are removed. `<img>` tags are replaced

##Missing/Potential Future Features

* Blogger terms are not yet migrated to tags / matched to the original posts
* Blogger images are wrapped in some additional `<div><a>` tags to launch the original image on the blogspot site. The hyperlink is directing to the blogger version, but the actual image used will be referencing a path under the ghost blog 
* Better HTML cleansing?
* Better general error handling