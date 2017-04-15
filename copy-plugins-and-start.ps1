$BECKYFOLDER="C:\temp\BeckyPluginTest\bk27300"
$DATAFOLDER="C:\temp\BeckyPluginTest\bkdata"
$BACKUPFOLDER="C:\temp\BeckyPluginTest\backup\"
$PLUGINFOLDER="C:\Sonstige\BeckyPlugin.Net\plugins"

$BECKY_PROCESS_NAME = "B2"

$datetime = Get-Date -f "yyyy-MM-dd hh_mm_ss"

$countprocesses = get-process | Where-Object {$_.ProcessName -eq $BECKY_PROCESS_NAME} | tee -Variable beckyprocesses | measure
if ($countprocesses.Count -gt 0) {
  write-host "Close all Becky Processes!"
  write-host "$beckyprocesses"
  pause 10000
}


$plugins = Get-Item "$PLUGINFOLDER/*"
foreach($plugin in $plugins) {
	$pluginname = $plugin.Name
	Write-Host "Copying: $pluginname"
	New-Item -ItemType directory -Path "$BACKUPFOLDER/$pluginname/$datetime"
	Move-Item "$BECKYFOLDER/plugins/$pluginname" "$BACKUPFOLDER/$pluginname/$datetime"
	Copy-Item "$PLUGINFOLDER/$pluginname/bin/Debug" "$BECKYFOLDER/plugins/$pluginname"  -Recurse
	
	# Putting the assembly into plugin folder
	Copy-Item "$PLUGINFOLDER/$pluginname/bin/Debug/$pluginname.dll*" "$BECKYFOLDER/plugins/"  -Recurse
	Copy-Item "$PLUGINFOLDER/$pluginname/bin/Debug/$pluginname.dll.config*" "$BECKYFOLDER/plugins/"  -Recurse
	Copy-Item "$PLUGINFOLDER/$pluginname/bin/Debug/$pluginname.pdb*" "$BECKYFOLDER/plugins/"  -Recurse
}

#foreach($plugin in $plugins) { echo $plugin.Name }

#. $BECKYFOLDER\b2.exe