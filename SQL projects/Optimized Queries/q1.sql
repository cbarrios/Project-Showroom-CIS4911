/* Q1: OPTIMIZED SQL CODE */
SELECT L.AuthorLast, R.AuthorLast
FROM ( SELECT LL.AuthorNum, LL.AuthorLast, LR.AuthorNum as w2AuthorNum
       FROM ( SELECT LLL.AuthorNum, LLL.AuthorLast, LLR.BookCode
	      FROM ( SELECT a1.AuthorNum, a1.AuthorLast
		     FROM Author a1 ) as LLL
		   JOIN
		   ( SELECT w1.AuthorNum, w1.BookCode
		     FROM Wrote w1 ) as LLR
		   ON LLL.AuthorNum = LLR.AuthorNum ) as LL
	    JOIN
	    ( SELECT w2.AuthorNum, w2.BookCode
	      FROM Wrote w2 ) as LR
	    ON LL.BookCode = LR.BookCode ) as L
     JOIN
     ( SELECT a2.AuthorNum, a2.AuthorLast
       FROM Author a2 ) as R
     ON L.w2AuthorNum = R.AuthorNum and L.AuthorNum < R.AuthorNum
