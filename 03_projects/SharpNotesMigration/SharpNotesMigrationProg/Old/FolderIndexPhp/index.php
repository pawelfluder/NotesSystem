<?php
	define('TYPE', "folder");
	define('INDEX', "index.php");
	define('S', DIRECTORY_SEPARATOR);
	function GetRootPath()
	{
		$root = $_SERVER['DOCUMENT_ROOT'];
		$root = str_replace("/",S,$root);
		$root = str_replace('\\',S,$root);
		if (substr($root, -1) != S)
		{
			$root .= S;
		}
		//$root .= "public_html".S;
		//test
		return $root;
	}
	define('ROOT', GetRootPath());
	
	
	if (session_status() != PHP_SESSION_ACTIVE) 
	{
		session_start();
	}
	OnStart();
	
	function OnStart()
	{	
		$commonPath = ROOT."items".S."commonStart.php";
		include "$commonPath";

		$lastFolderPathForCommon = FindLastFolderPathForCommon();
		include "$lastFolderPathForCommon";

		$lastFolderPathForIndex = FindLastFolderPathForIndex();
		$isUpToDate = IsIndexUpToDate($lastFolderPathForIndex);
		UpdateIndexIfNotUpToDate($isUpToDate, $lastFolderPathForIndex);
		//EchoUpToDateStatement($isUpToDate);

		$lastFolderPathForTypeCommon = FindLastFolderPathForTypeCommon();		
		include $lastFolderPathForTypeCommon;		
		Engine();
	}
?>
