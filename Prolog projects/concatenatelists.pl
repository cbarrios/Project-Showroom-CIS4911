conc([], L, L).
conc([X | L1], L2, [X | L3]) :- conc(L1,L2,L3).

member1(X,L) :- conc(L1, [X | L2], L).

rotate([H|T],R) :- conc(T,[H],R).

sublist(S,L) :- conc(L1,L2,L), conc(S,L3,L2).