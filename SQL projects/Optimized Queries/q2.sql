/* Q2: OPTIMIZED SQL CODE */
SELECT R.PublisherName as Publisher, SUM(L.OnHand)as TotalBooks
FROM ( SELECT LR.PublisherCode, LL.OnHand
       FROM ( SELECT LLL.BookCode, LLL.OnHand
	      FROM ( SELECT i.BookCode, i.OnHand, i.BranchNum
	             FROM Inventory i ) as LLL
	           JOIN
	           ( SELECT r.BranchNum
	             FROM Branch r 
	             WHERE r.BranchLocation = 'Eastshore Mall' ) as LLR
	           ON LLL.BranchNum = LLR.BranchNum ) as LL
	    JOIN
	    ( SELECT b.BookCode, b.PublisherCode
	      FROM Book b ) as LR
	    ON LL.BookCode = LR.BookCode ) as L
     JOIN
     ( SELECT p.PublisherCode, p.PublisherName
       FROM Publisher p ) as R
     ON L.PublisherCode = R.PublisherCode
GROUP BY R.PublisherName