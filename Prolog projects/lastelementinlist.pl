last(X,[X]).
last(X,[_|T]) :- last(X,T).