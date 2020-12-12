# YiSA.FontAwesome5.WPF

[ss1]: https://github.com/p4j4dyxcry/YiSA.FontAwesome5.WPF/blob/main/resources/screenshots/1.gif
[ss2]: https://github.com/p4j4dyxcry/YiSA.FontAwesome5.WPF/blob/main/resources/screenshots/2.gif
[nuget]: https://www.nuget.org/packages/YiSA.FontAwesome5.WPF/

![.NET Core Version: >= 3.1](https://img.shields.io/badge/.NET%20Core-%3E%3D%203.1-brightgreen) 
![.NET Framework version: >= 4.61](https://img.shields.io/badge/.NET%20Framework-%3E%3D%204.61-brightgreen) [![MIT License](http://img.shields.io/badge/license-MIT-lightgray)](LICENSE)  
# What's This?
1. A tool to easily use [Font Awesome5](https://fontawesome.com/) in WPF
2. Automatically generate a C # file from an svg file in a form that can be used in WPF. It does not depend on Font Awesome.

## auto-generated code sample
https://raw.githubusercontent.com/p4j4dyxcry/YiSA.FontAwesome5.WPF/main/sources/YiSA.FontAwesome5.WPF/fa5.autogen.cs

You can incorporate an auto-generated C # file into your project using the command line tools shown below.

YiSA.FontAwesome5.WPF/tools/FontAwesomeIconGenerator/generate.bat

However, this does not have a drawing function, so you need to use [control](https://github.com/p4j4dyxcry/YiSA.FontAwesome5.WPF/blob/main/sources/YiSA.FontAwesome5.WPF/GeometryIcon.cs) or create your own.
If you get it from nuget, it comes with drawing controls.

Since the drawing control is one file, it is easy to incorporate it statically.

download from [nuget](https://www.nuget.org/packages/YiSA.FontAwesome5.WPF/) or Powershell .

`PM >Install-Package YiSA.FontAwesome5.WPF`

# How do you use it?
There is a [built-in sample](https://github.com/p4j4dyxcry/YiSA.FontAwesome5.WPF/tree/main/samples/SampleApp). Probably the best way to see this.

![ss1]
![ss2]