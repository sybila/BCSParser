Parser for Biochemical Space language expressions and definhition files
=======================================================================

This project contains two main parts that combined together are capable of loading bcs format files. As en output iy produces semantic model of the file along with syntaktic trees for expressions defined in the file.

**BCS expresion parser and tokenizer** - Tokenizer creates tokens from bcs an expresions, parser then processes these tokens and builds syntax tree for the expression. 

**BCS file reader** - Loads rulse and entities from BCS format files

**BCS file handler** - Used BCS reader to read a bcs file, then creates used states, agents, somplexes and locations from entities. Then using BCS Pareser builds syntax trees for equations and modifiers in rules. Lastly it checks semantic corectnes of rules against entity definitions and fills semantic data from entities into the syntax tree.

Repository also contains sample console application and Sample WFP aplication.
Console aplication is inteneded for usage reference.
WPF aplication loads .bcs file and draws equations from rules in the file.

Usage
-----

1) Reference BcsResolver.dll from your project

2) Use BcsResolver.File namespace

Sample code
-----------

```C#
BcsDefinitionFile document;

using (var bcsHandler = new BcsFileHandler())
{
    bcsHandler.ProcessDefinitionFile("yamada.txt");
    document = bcsHandler.DefinitionFile;
}
```
