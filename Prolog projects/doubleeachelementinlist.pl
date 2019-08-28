%this will double a list [a,b,c] to another list [a,a,b,b,c,c]
double([],[]).
double([X|T],[X,X|L]) :- double(T,L).