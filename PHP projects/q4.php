<?php
# Q4: For a given author number (input from a browser), output:
# a) firstname and lastname of the author
# b) title of each book written by the author
# c) total number of book copies on hand from all branches written by the author

# I did all three queries separately using the same input variable $authornum

print ("<br>");
$authornum = isset($_POST['authornum']) ? $_POST['authornum'] : '';
$visited = isset($_POST['visited']) ? $_POST['visited'] : '';
$inputmsg = '';

if (!($authornum )) {
  if ($visited) {	  
     $inputmsg = 'Please enter an author number value';
  }

 // printing the form to enter the user input
 print <<<_HTML_
 <FORM method="POST" action="{$_SERVER['PHP_SELF']}">
 Q4: For a given author number (input from a browser), output: <br>
 a) firstname and lastname of the author <br>
 b) title of each book written by the author <br>
 c) total number of book copies on hand from all branches written by the author <br>
 <font color= 'red'>$inputmsg</font><br>
 Author Number: <input type="text" name="authornum" size="9" value="$authornum">
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
  
  # a) firstname and lastname of the author
  $querystring_a = "SELECT authorfirst, authorlast FROM author WHERE authornum = $authornum";
  $result_a = mysqli_query($con, $querystring_a);
  if (!$result_a) {
    print ( "Could not successfully run query ($querystring_a) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

  if (mysqli_num_rows($result_a) == 0) {
    print ("No author found for author number $authornum. <br>");
    exit;
  }

  print("Q4 output for author number: $authornum <br>");
   if ( $obj_a = @mysqli_fetch_object($result_a) ) {
    // Login good, create session variables
    $fname = $obj_a->authorfirst;
	$lname = $obj_a->authorlast;
    print("Author's name: $fname $lname<br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  # b) title of each book written by the author
  $querystring_b = "SELECT b.title FROM book b, wrote w WHERE b.bookcode = w.bookcode and w.authornum = $authornum";
  $result_b = mysqli_query($con, $querystring_b);
  if (!$result_b) {
    print ( "Could not successfully run query ($querystring_b) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }
  
  if (mysqli_num_rows($result_b) == 0) {
    print ("No book found for author number $authornum. <br>");
    exit;
  }else {

   print("Book Titles: <br>");
  while ($rows = mysqli_fetch_assoc($result_b)) {
    foreach ($rows as $row) {
	   echo str_repeat("&nbsp;", 8); 
	   print $row;
	}
	print "<br>";
  }
  
  # c) total number of book copies on hand from all branches written by the author
  $querystring_c = "SELECT SUM(i.onhand) as totalcopies FROM wrote w, inventory i WHERE w.bookcode = i.bookcode and w.authornum = $authornum";
  $result_c = mysqli_query($con, $querystring_c);
  if (!$result_c) {
    print ( "Could not successfully run query ($querystring_c) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }
  
   if ( $obj_c = @mysqli_fetch_object($result_c) ) {
    // Login good, create session variables
    $totalcopies = $obj_c->totalcopies;
	if($totalcopies != NULL)
		print("Total number of book copies: $totalcopies <br>");
	else
		print("Total number of book copies: 0 <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
 }
  
  mysqli_close($con);
}
?>