/* Q8: For each book copy available at the Henry on the Hill branch whose quality is Excellent, 
list the book's title (sorted alphabetically) and author names (in the order listed on the cover). */

select title, authorlast, authorfirst
from branch, copy, book, wrote, author
where book.bookcode = copy.bookcode and branch.branchnum = copy.branchnum and book.bookcode = wrote.bookcode and wrote.authornum = author.authornum and branchname = 'Henry on the Hill' and quality = 'Excellent'
order by title 
