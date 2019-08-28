sumto(0,0).
sumto(N,R) :- N > 0, N1 is N-1, sumto(N1,R1), R is R1+N.