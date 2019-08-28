ctoo(one,first).
ctoo(two,second).
ctoo(three,third).
ctoo(four,fourth).
ctoo(five,fifth).
ctoo(six,sixth).
ctoo(seven,seventh).
ctoo(eight,eight).
ctoo(nine,ninth).

cardtoord([],[]).
cardtoord([X|T],[Y|L]) :- ctoo(X,Y), cardtoord(T,L).
cardtoord([X|T],[Y|L]) :- ctoo(Y,X), cardtoord(T,L).