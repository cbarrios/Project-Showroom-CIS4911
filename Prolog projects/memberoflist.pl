member(X, [X|Tail]).
member(X, [Head|Tail]) :- member(X, Tail).

set([]).
set([X|T]):- not(member(X,T)), set(T).