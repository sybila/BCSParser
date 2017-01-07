using BcsResolver.Common;
using BcsResolver.Extensions;
using BcsResolver.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BcsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            BcsDefinitionFile document;

            using (var bcsHandler = new BcsWorkspace())
            {
                bcsHandler.ProcessDefinitionFile("yamada.txt");
                document = bcsHandler.DefinitionFile;
            }

            //categorize
            var malformedLines = document.Entities.SelectMany(e => e.MalformedLines).Concat(document.Rules.SelectMany(r => r.MalformedLines));
            var equations = document.Rules.Select(r => r.Equation);

            //write debug output
            using (var writer = new System.IO.StreamWriter("eq.txt"))
            {
                foreach (var equation in equations)
                {
                    writer.WriteLine(equation.SourceText);

                    var stringifiedTokens = equation.SourceTokens.Select(token => $"[{token.Type.GetDescription()}]");

                    writer.WriteLine(string.Join("", stringifiedTokens));
                }
                writer.Close();
            }
        }
    }
}
