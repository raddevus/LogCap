#### Helpful Queries
-- displays all ipaddresses that have accessed the site, along with an access count<br>
`select count(*), sitedesc, ipaddress  from webinfo group by sitedesc, ipaddress order by count(*);`
