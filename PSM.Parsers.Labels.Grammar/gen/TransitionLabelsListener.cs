//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/ddortmont/GitHub/PSM/PSM.Parsers.Labels.Grammar/TransitionLabels.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="TransitionLabelsParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface ITransitionLabelsListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLabel([NotNull] TransitionLabelsParser.LabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.label"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLabel([NotNull] TransitionLabelsParser.LabelContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.orExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOrExpr([NotNull] TransitionLabelsParser.OrExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.orExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOrExpr([NotNull] TransitionLabelsParser.OrExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.andExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAndExpr([NotNull] TransitionLabelsParser.AndExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.andExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAndExpr([NotNull] TransitionLabelsParser.AndExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.negExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNegExpr([NotNull] TransitionLabelsParser.NegExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.negExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNegExpr([NotNull] TransitionLabelsParser.NegExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVal([NotNull] TransitionLabelsParser.ValContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVal([NotNull] TransitionLabelsParser.ValContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariable([NotNull] TransitionLabelsParser.VariableContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariable([NotNull] TransitionLabelsParser.VariableContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCommand([NotNull] TransitionLabelsParser.CommandContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCommand([NotNull] TransitionLabelsParser.CommandContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.variable_val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariable_val([NotNull] TransitionLabelsParser.Variable_valContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.variable_val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariable_val([NotNull] TransitionLabelsParser.Variable_valContext context);
}