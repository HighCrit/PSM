grammar TransitionLabels;

label: orExpr EOF;
orExpr: andExpr (OR andExpr)*;
andExpr: negExpr (AND negExpr)*;
negExpr: (NOT negExpr) | val;

val: command | variable | LPAREN orExpr RPAREN;

variable: IDENTIFIER (VARIABLE_OP VARIABLE_VAL)?;
command: CMDCHK IDENTIFIER RPAREN;

variable_val: BOOLEAN | INTEGER | DECIMAL | IDENTIFIER;
BOOLEAN: 'true' | 'false';
INTEGER: '-'? DIGITS;
DECIMAL: '-'? DIGITS '.' DIGITS;
DIGITS: [0-9]+;
OR: '||';
AND: '&&';
NOT: '!';

VARIABLE_OP: '=' | '<' | '>';

IDENTIFIER: [A-Za-z0-9_]+;
LPAREN: '(';
RPAREN: ')';
CMDCHK: 'CmdChk(';

// Whitespace (skip)
WS: [ \t\r\n]+ -> skip;
