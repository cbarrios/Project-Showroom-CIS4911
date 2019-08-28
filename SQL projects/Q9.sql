/* Q9: Create a new table named FictionCopies using the data in the bookCode, title, 
branchNum, copyNum, quality, and price columns for those books that have the type FIC. 
You should do this in two steps: 9A) Create the table, and 9B) populate it with the said data. */

CREATE TABLE FictionCopies 
(bookCode CHAR(4),
title CHAR(40),
branchNum DECIMAL(2,0),
copyNum DECIMAL(2,0),
quality CHAR(20),
price DECIMAL(8,2)
);

INSERT INTO FictionCopies
(select copy.bookcode, title, branchnum, copynum, quality, price
from book, copy
where book.bookcode = copy.bookcode and type = 'FIC'
);

