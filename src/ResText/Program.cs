using ResText;
using System;
using System.IO;
using System.Resources;

using StreamReader textReader = new StreamReader(path: "Exceptions.restext");
using TextResourceReader reader = new TextResourceReader(textReader)
{
    UseTextResourceDataNodes = true
};
using StreamWriter textWriter = new StreamWriter(path: "Exceptions.txt");
using TextResourceWriter writer = new TextResourceWriter(textWriter);

foreach (TextResourceDataNode entry in reader)
{
    writer.AddResource(name: entry.Name, entry);
    writer.Generate();
}

Console.WriteLine(Exceptions.AbstractDelegateString);
Console.WriteLine(Exceptions.GetMissingGetAwaiterMethodString(typeof(int)));