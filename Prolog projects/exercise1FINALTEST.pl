%interchange the "cut" symbol '!' to see diff results for the query ?- a(X).
a(X) :- b(X).
b(X) :- g(X),!, v(X).
b(X) :- f(X).
g(15).
g(4).
v(X).
f(5).
