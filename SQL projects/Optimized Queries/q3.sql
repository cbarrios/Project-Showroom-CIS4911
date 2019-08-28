/* Q3: OPTIMIZED SQL CODE */
SELECT Title
FROM ( SELECT LL.BookCode
       FROM ( SELECT w.BookCode, w.AuthorNum
	      FROM Wrote w ) as LL
            JOIN
            ( SELECT a.AuthorNum
	      FROM Author a 
	      WHERE a.AuthorLast = 'Rowling' ) as LR
            ON LL.AuthorNum = LR.AuthorNum ) as L
     JOIN
     ( SELECT b.BookCode, Title
       FROM Book b ) as R
     ON L.BookCode = R.BookCode
UNION
SELECT Title
FROM ( SELECT LL.BookCode
       FROM ( SELECT i.BookCode, i.BranchNum
	      FROM Inventory i 
	      WHERE i.OnHand > 0 ) as LL
            JOIN
            ( SELECT r.BranchNum
	      FROM Branch r 
	      WHERE r.BranchName = 'Henry Brentwood' ) as LR
            ON LL.BranchNum = LR.BranchNum ) as L
     JOIN
     ( SELECT b.BookCode, Title
       FROM Book b ) as R
     ON L.BookCode = R.BookCode