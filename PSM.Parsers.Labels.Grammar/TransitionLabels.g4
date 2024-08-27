grammar TransitionLabels;

label: orExpr EOF;
orExpr: andExpr (OR andExpr)*;
andExpr: negExpr (AND negExpr)*;
negExpr: (NOT negExpr) | val;

val: command | variable | LPAREN orExpr RPAREN;

variable: IDENTIFIER VARIABLE_OP VARIABLE_VAL;
command: CMDCHK IDENTIFIER RPAREN;

VARIABLE_VAL: BOOLEAN;
BOOLEAN: 'true' | 'false';
OR: '||';
AND: '&&';
NOT: '!';

VARIABLE_OP: '=';

IDENTIFIER: [A-Za-z0-9_]+;
LPAREN: '(';
RPAREN: ')';
CMDCHK: 'CmdChk(';


// Whitespace (skip)
WS: [ \t\r\n]+ -> skip;
