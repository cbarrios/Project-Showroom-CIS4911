train(a,b).
train(c,a).
train(d,e).
train(e,a).
train(b,f).
train(g,d).
train(h,g).

travel(X,Y) :- train(X,Y).
travel(X,Y) :- train(X,Z), travel(Z,Y).
travel(X,Y) :- train(Y,X).
travel(X,Y) :- train(Y,Z), travel(Z,X).
