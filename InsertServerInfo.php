<?php
	$ip=$_SERVER['REMOTE_ADDR'];
	$port=$_GET["port"];
	$code=$_GET["code"];
	
	$conn = mysqli_connect("your DB","your DB id","your DB pwd","your DB table");
	if(mysqli_connect_errno()){
		echo "mysql connect error : ".mysqli_connect_errno();
	}
	
	$query = "INSERT INTO asm(ip,port,code) VALUES('".$ip."',".$port.",'".$code."')";
	$result = mysqli_query($conn,$query);
?>