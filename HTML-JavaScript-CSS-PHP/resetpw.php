<?php

	$email = isset($_POST['email']) ? $_POST['email'] : '';
	$token = isset($_POST['token']) ? $_POST['token'] : '';
	$newpw = isset($_POST['newPassword']) ? $_POST['newPassword'] : '';

	if($email && $token && $newpw) 
	{
		$host = 'localhost';
		$user = 'xyz';
		$pw = 'xyz';
		$db = 'xyz';
		$conn = new mysqli($host,$user,$pw,$db);

		$sql = $conn->query("SELECT username,token FROM user WHERE
			email='$email' AND token<>'' AND tokenExpire > NOW()
		");
		
		if($sql->num_rows > 0) 
		{
			$obj=mysqli_fetch_object($sql);
			$hashed_token = $obj->token;
			if(!password_verify($token, $hashed_token)) 
			{
				exit(json_encode(array("status" => 0, "msg" => "Oops, something went wrong while resetting your password. Please try again later.")));
			}
			
			$newPasswordEncrypted = password_hash($newpw, PASSWORD_DEFAULT);
			$conn->query("UPDATE user SET token='', password = '$newPasswordEncrypted'
				WHERE email='$email'
			");

			exit(json_encode(array("status" => 1, "msg" => "It's all set! Go ahead and login with your new password: $newpw")));
		} 
		else
		{
			exit(json_encode(array("status" => 0, "msg" => "Oops, something went wrong while resetting your password. Please try again later.")));
		}
	} 
?>
<!doctype html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport"
          content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Cauldron - Reset Password</title>
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" integrity="sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm" crossorigin="anonymous">
</head>
<body>
    <div class="container" style="margin-top: 100px;">
        <div class="row justify-content-center">
            <div class="col-md-6 col-md-offset-3" align="center">
                <img src="cauldron_logo_login.png"><br><br>
				<input type="email"  class="form-control" id="email" placeholder="Your E-mail Address"><br>
				<input class="form-control" id="token" placeholder="Your Token"><br>
				<input class="form-control" id="newPassword" placeholder="Your New Password"><br>
                <input type="button" class="btn btn-primary" value="RESET PASSWORD"><br>
                <br><br>
                <p id="response"></p>
            </div>
        </div>
    </div>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js" integrity="sha256-FgpCb/KJQlLNfOu91ta32o/NMZxltwRo8QtmkMRdAu8=" crossorigin="anonymous"></script>
    <script type="text/javascript">
        var newPassword = $("#newPassword");
		var email = $("#email");
		var token = $("#token");
		
		function ValidateEmail(inputText)
		{
			//var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
			// As per the HTML5 Specification
			var emailRegExp = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
			if(emailRegExp.test(inputText))
			{
				return true;
			}
			else
			{
				//alert("You have entered an invalid email address!\nPlease try again.");
				return false;
			}
		}
		
		function ValidatePassword(inputText)
		{
			var passformat = /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[._$*()#@!%])[A-Za-z0-9._$*()#@!%]{8,}$/;
			if(passformat.test(inputText))
			{
				return true;
			}
			else
			{
				//alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
				return false;
			}
		}
		
        $(document).ready(function () {
            $('.btn-primary').on('click', function () {
				if( (email.val() == "") && (token.val() == "") && (newPassword.val() == "") ){
					email.css('border', '1px solid red'); 
					token.css('border', '1px solid red');
					newPassword.css('border', '1px solid red');
					alert("All fields are required!\nPlease fill them out.");
				}
				else if( (email.val() != "") && (token.val() == "") && (newPassword.val() == "") ){
					if(ValidateEmail(email.val())){
						email.css('border', '1px solid green'); 
						token.css('border', '1px solid red');	
						newPassword.css('border', '1px solid red');
					}else{
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid red');	
						newPassword.css('border', '1px solid red');
						alert("You have entered an invalid email address!\nPlease try again.");
					}
					
				}
				else if( (email.val() == "") && (token.val() != "") && (newPassword.val() == "") ){
					email.css('border', '1px solid red'); 
					token.css('border', '1px solid green');	
					newPassword.css('border', '1px solid red');
				}
				else if( (email.val() == "") && (token.val() == "") && (newPassword.val() != "") ){
					if(ValidatePassword(newPassword.val())){
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid red');	
						newPassword.css('border', '1px solid green');
					}else{
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid red');	
						newPassword.css('border', '1px solid red');
						alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
					}
					
				}
				else if( (email.val() != "") && (token.val() != "") && (newPassword.val() == "") ){
					if(ValidateEmail(email.val())){
						email.css('border', '1px solid green'); 
						token.css('border', '1px solid green');	
						newPassword.css('border', '1px solid red');
					}else{
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid green');	
						newPassword.css('border', '1px solid red');
						alert("You have entered an invalid email address!\nPlease try again.");
					}
				}
				else if( (email.val() != "") && (token.val() == "") && (newPassword.val() != "") ){
					if( (ValidateEmail(email.val())) && (ValidatePassword(newPassword.val())) ) {
						email.css('border', '1px solid green'); 
						token.css('border', '1px solid red');	
						newPassword.css('border', '1px solid green');
					}else{
						if(ValidateEmail(email.val())){
							email.css('border', '1px solid green'); 
							token.css('border', '1px solid red');	
							newPassword.css('border', '1px solid red');
							alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
						}
						if(ValidatePassword(newPassword.val())){
							email.css('border', '1px solid red'); 
							token.css('border', '1px solid red');	
							newPassword.css('border', '1px solid green');
							alert("You have entered an invalid email address!\nPlease try again.");
						}
						if( !(ValidateEmail(email.val())) && !(ValidatePassword(newPassword.val())) ){
								email.css('border', '1px solid red'); 
								token.css('border', '1px solid red');	
								newPassword.css('border', '1px solid red');
								alert("You have entered an invalid email address!\nPlease try again.");
								alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
							}
					}
				}
				else if( (email.val() == "") && (token.val() != "") && (newPassword.val() != "") ){
					if(ValidatePassword(newPassword.val())){
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid green');	
						newPassword.css('border', '1px solid green');
					}else{
						email.css('border', '1px solid red'); 
						token.css('border', '1px solid green');	
						newPassword.css('border', '1px solid red');
						alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
					}
				}
				else if( (email.val() != "") && (token.val() != "") && (newPassword.val() != "") ){
					if( (ValidateEmail(email.val())) && (ValidatePassword(newPassword.val())) ){
						email.css('border', '1px solid green'); 
						token.css('border', '1px solid green');	
						newPassword.css('border', '1px solid green');
						
						$.ajax({
							url: 'resetPassword.php',
							method: 'POST',
							dataType: 'json',
							data: 
							{
								newPassword: newPassword.val(),
								email: email.val(),
								token: token.val()
						   
							}, success: function (response) 
							{
								if (!response.success)
									$("#response").html(response.msg).css('color', "red");
								else if (response.success)
									$("#response").html(response.msg).css('color', "green");
							}
						});
					}else{
						
							if(ValidateEmail(email.val())){
								email.css('border', '1px solid green'); 
								token.css('border', '1px solid green');	
								newPassword.css('border', '1px solid red');
								alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
							}
							if(ValidatePassword(newPassword.val())){
								email.css('border', '1px solid red'); 
								token.css('border', '1px solid green');	
								newPassword.css('border', '1px solid green');
								alert("You have entered an invalid email address!\nPlease try again.");
							}
							if( !(ValidateEmail(email.val())) && !(ValidatePassword(newPassword.val())) ){
								email.css('border', '1px solid red'); 
								token.css('border', '1px solid green');	
								newPassword.css('border', '1px solid red');
								alert("You have entered an invalid email address!\nPlease try again.");
								alert("You have entered an invalid password!\nA valid one must contain the following:\n(1) At least 8 characters.\n(2) At least one uppercase letter.\n(3) At least one lowercase letter.\n(4) At least one number.\n(5) At least one special symbol.");
							}
					}
				}
				
            });
        });
    </script>
</body>
</html>
