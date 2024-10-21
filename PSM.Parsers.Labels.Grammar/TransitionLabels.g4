grammar TransitionLabels;

label: orExpr EOF;
orExpr: andExpr (OR andExpr)*;
andExpr: negExpr (AND negExpr)*;
negExpr: (NOT negExpr) | val;

val: command | variable | LPAREN orExpr RPAREN;

variable: PATH (VARIABLE_OP variable_val)?;
command: CMDCHK PATH RPAREN;

variable_val: BOOLEAN | INTEGER | DECIMAL | PATH;
BOOLEAN: 'true' | 'false';
INTEGER: '-'? DIGITS;
DECIMAL: '-'? DIGITS '.' DIGITS;
DIGITS: [0-9]+;
OR: 'OR';
AND: 'AND';
NOT: 'NOT';

VARIABLE_OP: '==' | '<' | '>' | '<=' | '>=';

PATH: IDENTIFIER ('.' IDENTIFIER)*;

IDENTIFIER: [A-Za-z0-9_]+;
LPAREN: '(';
RPAREN: ')';
CMDCHK: 'CmdChk(';

// Whitespace (skip)
WS: [ \t\r\n]+ -> skip;
