Parser for Biochemical Space language expressions and definition files
=======================================================================

This project contains two main parts. Combined together, they are capable of loading BCS files. As an output it produces semantic model of the file along with syntactic trees for expressions defined in the file.

**BCS expression parser and tokenizer** - Tokenizer creates tokens from BCS expresion, parser then proceeds to read these tokens and builds syntax tree for the expression. 

**BCS file reader** - Loads rules and entities from BCS files

**BCS file handler** - Uses BCS reader to read a BCS file, then creates lists of states, agents, complexes and locations used in the file. Then, using BCS Parser, it builds syntax trees for equations and modifiers contained in rules. Finally, it checks semantic correctnes of rules against entity definitions and fills semantic data from entities into the syntax tree.

Repository also contains sample console application and sample WPF application.
Console application is inteneded solely for usage reference.
WPF aplication loads BCS file and draws equations from rules in the file.

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
    bcsHandler.ProcessDefinitionFile("yamada.bcs");
    document = bcsHandler.DefinitionFile;
}
```