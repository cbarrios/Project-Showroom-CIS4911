/* Q1: List the title of each book published by Penguin USA. 
You are allowed to use only 1 table in any FROM clause. */

select title
from book
where publishercode IN (select publishercode
			from publisher
			where publishername = 'Penguin USA')
