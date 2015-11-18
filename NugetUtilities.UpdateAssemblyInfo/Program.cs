using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NugetUtilities.UpdateAssemblyInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = args[0];
            var attributeTypeName = args[1];
            var attributeValue = args[2];
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
            var rewriter = new Rewriter(attributeTypeName, attributeValue);

            var newTree = tree.WithRootAndOptions(rewriter.Visit(tree.GetRoot()), CSharpParseOptions.Default);
            var s = newTree.GetText().ToString();

            File.WriteAllText(file, s);
            Console.WriteLine($"Successfully updated the value of '{attributeTypeName}' to '{attributeValue}'");
        }

        private class Rewriter : CSharpSyntaxRewriter
        {
            private readonly string attributeTypeName;
            private readonly string attributeValue;

            public Rewriter(string attributeTypeName, string attributeValue) 
            {
                this.attributeTypeName = attributeTypeName;
                this.attributeValue = attributeValue;
            }

            public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node)
            {
                var attributeList = (AttributeArgumentListSyntax)node.Parent;
                var attribute = (AttributeSyntax)attributeList.Parent;
                var attributeName = ((IdentifierNameSyntax)attribute.Name).Identifier.ValueText;
                if (attributeName == attributeTypeName)
                {
                    var newArgument = node.WithExpression(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(attributeValue)));
                    return newArgument;
                }

                return base.VisitAttributeArgument(node);
            }
        }
    }
}
