add(X, L, [X | L]).
permutation([], []).
permutation([X|L],P) :- permutation(L,L1), add(X,L1,P).