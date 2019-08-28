<?php
# Q1: For a given branch number (input from a browser), output:
# a) branch name
# b) total number of all book copies on hand in that branch

# I did both a) and b) queries separately using the same input variable $branchnum 

print ("<br>");
$branchnum = isset($_POST['branchnum']) ? $_POST['branchnum'] : '';
$visited = isset($_POST['visited']) ? $_POST['visited'] : '';
$inpbranchmsg = '';

if (!($branchnum )) {
  if ($visited) {	  
     $inpbranchmsg = 'Please enter a branch number value';
  }

 // printing the form to enter the user input
 print <<<_HTML_
 <FORM method="POST" action="{$_SERVER['PHP_SELF']}">
 Q1 : For a given branch number, output:  <br>
 a) branch name  <br>
 b) total number of all book copies on hand in that branch  <br>
 <font color= 'red'>$inpbranchmsg</font><br>
 Branch Number: <input type="text" name="branchnum" size="9" value="$branchnum">
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
  
  $querystring = "SELECT branchname FROM branch WHERE branchnum = $branchnum";
  $result = mysqli_query($con, $querystring);
  if (!$result) {
    print ( "Could not successfully run query ($querystring) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

  if (mysqli_num_rows($result) == 0) {
    print ("No branch found for branch number $branchnum. <br>");
    exit;
  }

  print("Q1 output for branch number: $branchnum <br>");
   if ( $obj = @mysqli_fetch_object($result) ) {
    // Login good, create session variables
    $branchname = $obj->branchname;
    print("Branch name: $branchname <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  $querystring2 = "SELECT SUM(onhand) as totalcopies FROM inventory WHERE branchnum = $branchnum";
  $result2 = mysqli_query($con, $querystring2);
  if (!$result2) {
    print ( "Could not successfully run query ($querystring2) from DB: " . mysqli_error($con) . "<br>");
    exit;
  }

   if ( $obj2 = @mysqli_fetch_object($result2) ) {
    // Login good, create session variables
    $totalcopies = $obj2->totalcopies;
    print("Total number of book copies: $totalcopies <br>");
  }
  else {
    // Query not successful
    die("Sorry, Query has some error.<br>");
  }
  
  mysqli_close($con);
}
?>
