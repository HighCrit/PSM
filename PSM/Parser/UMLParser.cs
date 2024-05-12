using System.Xml;

namespace PSM.Parser;

class UMLParser {
    /// <summary>
    /// Parses the provided UML state-machine diagram to a mu-calculus formula.
    /// </summary>
    /// <param name="filePath"></param>
    public void Parse(string filePath){
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(filePath);
    }
}
