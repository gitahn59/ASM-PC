<?php
	$conn = mysqli_connect("your DB","your DB id","your DB pwd","your DB table");
	if(mysqli_connect_errno()){
		echo "mysql connect error : ".mysqli_connect_errno();
	}

	$code=$_GET["code"];
	$query = "SELECT ip,port FROM asm WHERE code = '".$code."'";
	$result = mysqli_query($conn,$query);
	
	$jsonObject;
	while($row = mysqli_fetch_array($result)){
		$jsonObject = array("ip" => $row["ip"], "port" => $row["port"]);
	}

	$query = "DELETE FROM asm WHERE code = '".$code."'";
	$result = mysqli_query($conn,$query);

    header('Content-Type: application/json');
    	
    $json = array($jsonObject);
    $output =  json_encode($json);

    echo  urldecode($output);
?>