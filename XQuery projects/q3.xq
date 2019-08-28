(: 
  (Q3) For each publisher, output:
    a) publisher code and publisher name
    b) number of distinct authors who wrote book(s) for that publisher
    c) number of book titles published by the publisher
    d) total number of book copies on hand on all branches for that publisher
:)

<results>
  {
    for $publisher in doc("../henry/Publisher.xml")//Publisher
    let $book := doc("../henry/Book.xml")//Book[PublisherCode = $publisher/PublisherCode]
    let $wrote := doc("../henry/Wrote.xml")//Wrote[BookCode = $book/BookCode]
    let $inventory := doc("../henry/Inventory.xml")//Inventory[BookCode = $book/BookCode]
    
    return
     <row>
       <Publisher Code = "{$publisher/PublisherCode}" Name = "{$publisher/PublisherName}"/>
       <Authors count = "{count(distinct-values($wrote/AuthorNum))}"/>
       <Book TitleCount = "{count($book/Title)}"/>
       <Stock BookCopies = "{sum($inventory/OnHand)}"/>
     </row>
   }
</results>