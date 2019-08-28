power(N,0,1): - !.
power(N,K,P):- K1 is K-1,power(N,K1,P1),P is P1*N.