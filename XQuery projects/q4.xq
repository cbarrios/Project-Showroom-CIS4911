(: 
  (Q4) For each author, output:
    a) author number, firstname and lastname of the author
    b) title of each book written by the author
    c) total number of book copies on hand from all branches written by the author
:)

<results>
  {
    for $author in doc("../henry/Author.xml")//Author
    let $wrote := doc("../henry/Wrote.xml")//Wrote[AuthorNum = $author/AuthorNum]
    let $book := doc("../henry/Book.xml")//Book[BookCode = $wrote/BookCode]
    let $inventory := doc("../henry/Inventory.xml")//Inventory[BookCode = $wrote/BookCode]
    
    return
     <Author number="{$author/AuthorNum}" firstname="{$author/AuthorFirst}" lastname="{$author/AuthorLast}">
       {
         for $title in $book/Title
         return <Book title = "{$title}"/>
       }
       <Stock BookCopies="{sum($inventory/OnHand)}"/>
     </Author>    
  }
</results>