length([], 0).
length([H|Tail],N) :- length(Tail,N1), N is 1 + N1.