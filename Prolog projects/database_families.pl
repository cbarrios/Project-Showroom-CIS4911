family(
	person(tom,fox,date(7,may,1950),works(bbc,152000)),
	person(ann,fox,date(9,may,1951),unemployed),
	[person(pat,fox,date(5,may,1973),works(ucc,12500)),
	 person(jim,fox,date(5,may,1973),unemployed)]
).

husband(X) :- family(X,_,_).
wife(X) :- family(_,X,_).
child(X) :- family(_,_,Children), member(X,Children).

member(X, [X|L]).
member(X, [Y|L]) :- member(X,L).

exist(Person) :- husband(Person); wife(Person); child(Person).

dateofbirth(person(_,_,Date,_),Date).

salary(person(_,_,_,works(_,S)),S).
salary(person(_,_,_,unemployed),0).

%Total income of a family
total([],0).
total([Person|List],Sum) :- salary(Person,S), total(List,Salary_remaining), Sum is S+Salary_remaining.