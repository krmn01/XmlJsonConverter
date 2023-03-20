using System;
using System.IO;

namespace XmlJsonConverter.Core{

public class XmlJsonConverterViewModel
{
    public string filePath {get; set;}
    public string convertedFileName {get; set;}

    private void ConvertFile()
    {
        switch(Path.GetExtension(filePath))
        {
            case ".xml":
            break;

            case ".json":
            break;

            default:
            break;
        }
    }
    
}

}