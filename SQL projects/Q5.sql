/* Q5: For each book, list the title (sorted alphabetically), publisher
code, type and author names (in the order listed on the cover). */

select title, publishercode, book.type, authorlast, authorfirst
from book, author, wrote
where book.bookcode = wrote.bookcode and wrote.authornum = author.authornum
order by title 

