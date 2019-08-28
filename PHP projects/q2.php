<?php
# Q2: Output the titles of each pair of books that have the same price and are published by the same publisher.
# There should not be any duplicate reversed pair and no title be paired with the same title.

# I did this query statically since it does not require user input
# Returns two columns and multiple rows

print ("<br>");

  $host = "localhost";
  $user = "root";
  $password = "";
  $dbname = "henry";
  $con=mysqli_connect($host, $user, $password, $dbname);
  if (mysqli_connect_errno()) {
    echo "Failed to connect to MariaDB: " . mysqli_connect_error();
    exit;
  }

  $querystring = "SELECT b1.title as t1, b2.title as t2 FROM book b1, book b2 WHERE b2.bookcode != b1.bookcode and b1.publishercode = b2.publishercode and b1.price = b2.price and b1.title < b2.title";
  $result = mysqli_query($con, $querystring);
  if (!$result) {
    print ( "Could not successfully run query ($querystring) from DB: " . mysqli_error($con));
    exit;
  }

  if (mysqli_num_rows($result) == 0) {
    print ("No rows found, nothing to print so am exiting<br>");
    exit;
  }

  print("Q2 output: <br>");
  while ($rows = mysqli_fetch_assoc($result)) {
	 printf ("%s - %s",$rows["t1"],$rows["t2"]);
	 print ("<br>");
  }
  
  mysqli_close($con);
?>