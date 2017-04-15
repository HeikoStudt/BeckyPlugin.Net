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

function Copy-ItemExclude($source, $destination, $exclude) {
	$privexclude = $exclude -replace '/','\' #Where-Object FullName is built with \ instead
	Get-ChildItem $source -Recurse -Exclude $exclude `
	| Where-Object {$_.FullName -NotLike $exclude} `
	| Copy-Item -Destination {Join-Path $destination $_.FullName.Substring($source.length)}
}

$plugins = Get-Item "$PLUGINFOLDER/*"
foreach($plugin in $plugins) {
	$pluginname = $plugin.Name
	Write-Host "Copying: $pluginname"
	if (Test-Path "$BECKYFOLDER/plugins/$pluginname") {
		# There is always at least nlog.dll inside that folder => no special handling for pluginname.dll
		New-Item -ItemType directory -Path "$BACKUPFOLDER/$pluginname/$datetime"
		Move-Item "$BECKYFOLDER/plugins/$pluginname*" "$BACKUPFOLDER/$pluginname/$datetime"
	}
	
	New-Item -ItemType directory -Path "$BECKYFOLDER/plugins/$pluginname"
	#http://stackoverflow.com/questions/731752/exclude-list-in-powershell-copy-item-does-not-appear-to-be-working
	Copy-ItemExclude "$PLUGINFOLDER/$pluginname/bin/Debug" "$BECKYFOLDER/plugins/$pluginname" ("$PLUGINFOLDER/$pluginname/bin/Debug/$pluginname.*")

	
	# Putting the assembly into plugin folder
	Copy-Item "$PLUGINFOLDER/$pluginname/bin/Debug/$pluginname.*" "$BECKYFOLDER/plugins/"  -Recurse
}

#foreach($plugin in $plugins) { echo $plugin.Name }

. $BECKYFOLDER\b2.exe