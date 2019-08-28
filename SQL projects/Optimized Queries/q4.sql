/* Q4: OPTIMIZED SQL CODE */
SELECT PublisherName
FROM ( SELECT LR.PublisherCode
       FROM ( SELECT LLR.BookCode
	      FROM ( SELECT a.AuthorNum
		     FROM Author a
	             WHERE a.AuthorLast = 'King') as LLL
	           JOIN
	           ( SELECT w.AuthorNum,w.BookCode 
                     FROM Wrote w 
                     WHERE w.Sequence = 1 ) as LLR
                   ON LLL.AuthorNum = LLR.AuthorNum ) as LL
            JOIN
            ( SELECT b.BookCode, b.PublisherCode
	      FROM Book b ) as LR
            ON LL.BookCode = LR.BookCode ) as L
     JOIN
     ( SELECT p.PublisherCode, PublisherName
       FROM Publisher p ) as R
     ON L.PublisherCode = R.PublisherCode