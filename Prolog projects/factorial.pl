factorial(0, 1).
factorial(N, NFact) :-
    N > 0,
    Nminus1 is N - 1,
    factorial(Nminus1, Nminus1Fact),
    NFact is Nminus1Fact * N.