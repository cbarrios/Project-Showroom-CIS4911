/* Q4: List the title of each book written by John Steinbeck and that has the type FIC. */

select title
from book, wrote, author
where book.bookcode = wrote.bookcode and wrote.authornum = author.authornum and authorfirst = 'John' and authorlast = 'Steinbeck' and type = 'FIC'