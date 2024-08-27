grammar TransitionLabels;

// The entry point rule
expr: orExpr EOF;

// Rule for OR expressions
orExpr: andExpr (OR andExpr)*;

// Rule for AND expressions
andExpr: notExpr (AND notExpr)*;

// Rule for NOT expressions
notExpr: NOT notExpr         // NOT has the highest precedence
       | atom;               // Fallback to atom

// Atom rule: a basic unit like a boolean literal or a parenthesized expression
atom: BOOLEAN
    | COMMAND
    | VARIABLE
    | LPAREN expr RPAREN;

// Lexer rules
COMMAND : 'CmdChk(' NAME ')';
VARIABLE : NAME OP (BOOLEAN | NUMBER);
BOOLEAN: 'true' | 'false';
NUMBER: [0-9]+;
OP : '=' | '<' | '>';
AND: 'AND';
OR: 'OR';
NOT: 'NOT';
NAME : [a-zA-Z]+;
LPAREN: '(';
RPAREN: ')';

// Whitespace (skip)
WS: [ \t\r\n]+ -> skip;
