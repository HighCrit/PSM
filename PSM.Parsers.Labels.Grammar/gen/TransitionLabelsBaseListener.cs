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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ITransitionLabelsListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class TransitionLabelsBaseListener : ITransitionLabelsListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabel([NotNull] TransitionLabelsParser.LabelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabel([NotNull] TransitionLabelsParser.LabelContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.orExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOrExpr([NotNull] TransitionLabelsParser.OrExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.orExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOrExpr([NotNull] TransitionLabelsParser.OrExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.andExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAndExpr([NotNull] TransitionLabelsParser.AndExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.andExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAndExpr([NotNull] TransitionLabelsParser.AndExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.negExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNegExpr([NotNull] TransitionLabelsParser.NegExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.negExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNegExpr([NotNull] TransitionLabelsParser.NegExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVal([NotNull] TransitionLabelsParser.ValContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVal([NotNull] TransitionLabelsParser.ValContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.variable"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariable([NotNull] TransitionLabelsParser.VariableContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.variable"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariable([NotNull] TransitionLabelsParser.VariableContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.command"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCommand([NotNull] TransitionLabelsParser.CommandContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.command"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCommand([NotNull] TransitionLabelsParser.CommandContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="TransitionLabelsParser.variable_val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariable_val([NotNull] TransitionLabelsParser.Variable_valContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="TransitionLabelsParser.variable_val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariable_val([NotNull] TransitionLabelsParser.Variable_valContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}