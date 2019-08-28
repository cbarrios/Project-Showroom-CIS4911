<?php
# Q3: For a given publisher code (input from a browser), output:
# a) name of the publisher
# b) number of distinct authors who wrote book(s) for that publisher
# c) number of book titles published by the publisher
# d) total number of book copies on hand on all branches for that publisher

# I did all four queries separately using the same input variable $pubcode

print ("<br>");
$pubcode = isset($_POST['pubcode']) ? $_POST['pubcode'] : '';
$visited = isset($_POST['visited']) ? $_POST['visited'] : '';
$inputmsg = '';

if (!($pubcode )) {
  if ($visited) {	  
     $inputmsg = 'Please enter a publisher code value';
  }

 // printing the form to enter the user input
 print <<<_HTML_
 <FORM method="POST" action="{$_SERVER['PHP_SELF']}">
 Q3: For a given publisher code (input from a browser), output: <br>
 a) name of the publisher <br>
 b) number of distinct authors who wrote book(s) for that publisher <br>
 c) number of book titles published by the publisher <br>
 d) total number of book copies on hand on all branches for that publisher <br>
 <font color= 'red'>$inputmsg</font><br>
 Publisher Code: <input type="text" name="pubcode" size="9" value="$pubcode">
 <br/>
 <br>
 <INPUT type="submit" value=" Submit ">
 <INPUT type="hidden" name="visited" value="true">
 </FORM>
_HTML_;
 
}
else {
  $host = "localhost";
  $user="root";
  $password="";
  $dbname = "henry";
  $con=mysqli_connect($host, $user, $password, $dbname);
  if (mysqli_connect_errno()) {
    echo "Failed to connect to MariaDB: " . mysqli_connect_error();
    exit;
  }
  
  # a) name of the publisher
  $querystring_a = "SELECT publishername FROM publisher WHERE publishercode = '$pubcode'";
  $result_a = mysqli_query($con, $querystring_a);
  if (!$result_a) {
    print ( "Could not successfully run query ($querystring_a) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

  if (mysqli_num_rows($result_a) == 0) {
    print ("No publisher found for Publisher Code $pubcode. <br>");
    exit;
  }

  print("Q3 output for publisher code: $pubcode <br>");
   if ( $obj_a = @mysqli_fetch_object($result_a) ) {
    // Login good, create session variables
    $pubname = $obj_a->publishername;
    print("Publisher name: $pubname <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  # b) number of distinct authors who wrote book(s) for that publisher
  $querystring_b = "SELECT COUNT(DISTINCT w.authornum) as distauthors FROM book b, wrote w WHERE b.bookcode = w.bookcode and b.publishercode = '$pubcode'";
  $result_b = mysqli_query($con, $querystring_b);
  if (!$result_b) {
    print ( "Could not successfully run query ($querystring_b) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

   if ( $obj_b = @mysqli_fetch_object($result_b) ) {
    // Login good, create session variables
    $distauthors = $obj_b->distauthors;
    print("Number of distinct authors: $distauthors <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  # c) number of book titles published by the publisher
  $querystring_c = "SELECT COUNT(b.title) as booktitles FROM book b WHERE b.publishercode = '$pubcode'";
  $result_c = mysqli_query($con, $querystring_c);
  if (!$result_c) {
    print ( "Could not successfully run query ($querystring_c) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

   if ( $obj_c = @mysqli_fetch_object($result_c) ) {
    // Login good, create session variables
    $booktitles = $obj_c->booktitles;
    print("Number of book titles: $booktitles <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  # d) total number of book copies on hand on all branches for that publisher
  $querystring_d = "SELECT SUM(i.onhand) as totalcopies FROM book b, inventory i WHERE b.bookcode = i.bookcode and b.publishercode = '$pubcode'";
  $result_d = mysqli_query($con, $querystring_d);
  if (!$result_d) {
    print ( "Could not successfully run query ($querystring_d) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }
  
   if ( $obj_d = @mysqli_fetch_object($result_d) ) {
    // Login good, create session variables
    $totalcopies = $obj_d->totalcopies;
	if($totalcopies != NULL)
		print("Total number of book copies: $totalcopies <br>");
	else
		print("Total number of book copies: 0 <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
 
  mysqli_close($con);
}
?>