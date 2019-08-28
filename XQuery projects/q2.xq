(:
  (Q2)
  Output the titles of each pair of books that have the same price and are published by the same publisher.
  There should not be any duplicate reversed pair and no title be paired with the same title.
:)

<results>
  {
    for $b1 in doc("../henry/Book.xml")//Book,
        $b2 in doc("../henry/Book.xml")//Book
        
    where $b1/BookCode != $b2/BookCode and 
          $b1/Price = $b2/Price and 
          $b1/PublisherCode = $b2/PublisherCode and 
          $b1/Title < $b2/Title
          
    return <pair> { $b1/Title/text() ," - ", $b2/Title/text() } </pair>   
  }
</results>