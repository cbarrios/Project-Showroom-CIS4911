/* Q3: List the title of each book that has the type CMP, HIS, or SCI. 
You are allowed to use only one condition term in any WHERE clause. */

select title
from book
where type = 'CMP'
UNION
select title
from book
where type = 'HIS'
UNION
select title
from book
where type = 'SCI'