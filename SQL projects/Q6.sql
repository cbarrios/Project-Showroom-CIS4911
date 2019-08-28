/* Q6: How many book copies have a price that is greater than $20 and less than $25? */

select COUNT(*)
from copy
where price > 20 and price < 25
