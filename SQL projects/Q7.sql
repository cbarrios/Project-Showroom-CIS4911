/* Q7: For each publisher, list the publisher name and the number of books 
published by it, only if the publisher publishes at least 2 books. */

select publishername, COUNT(*)
from book, publisher
where book.publishercode = publisher.publishercode
group by book.publishercode, publishername
having COUNT(*) >= 2