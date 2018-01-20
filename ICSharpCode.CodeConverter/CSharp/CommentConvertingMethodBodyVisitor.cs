﻿using System.Collections.Generic;
using System.Linq;
using ICSharpCode.CodeConverter.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using SyntaxFactory = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ICSharpCode.CodeConverter.CSharp
{
	public class CommentConvertingMethodBodyVisitor : VisualBasicSyntaxVisitor<SyntaxList<StatementSyntax>>
	{
		private readonly VisualBasicSyntaxVisitor<SyntaxList<StatementSyntax>> wrappedVisitor;

		public CommentConvertingMethodBodyVisitor(VisualBasicSyntaxVisitor<SyntaxList<StatementSyntax>> wrappedVisitor)
		{
			this.wrappedVisitor = wrappedVisitor;
		}

		public override SyntaxList<StatementSyntax> DefaultVisit(SyntaxNode node)
		{
			var cSharpSyntaxNodes = wrappedVisitor.Visit(node);
			// Port trivia to the last statement in the list
			var allButLast = cSharpSyntaxNodes.Take(cSharpSyntaxNodes.Count - 1);
			var lastWithTrivia = cSharpSyntaxNodes.Last().WithConvertedTriviaFrom(node);
			return SyntaxFactory.List(allButLast.Concat(lastWithTrivia));
		}
	}
}