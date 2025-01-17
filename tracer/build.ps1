[CmdletBinding()]
Param(
    [Parameter(Position = 0, Mandatory = $false, ValueFromRemainingArguments = $true)]
    [string[]]$BuildArguments
)

Write-Output "PowerShell $($PSVersionTable.PSEdition) version $($PSVersionTable.PSVersion)"

Set-StrictMode -Version 2.0; $ErrorActionPreference = "Stop"; $ConfirmPreference = "None"; trap { Write-Error $_ -ErrorAction Continue; exit 1 }
$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent

###########################################################################
# CONFIGURATION
###########################################################################

$BuildProjectFile = "$PSScriptRoot\build\_build\_build.csproj"

###########################################################################
# EXECUTION
###########################################################################

function ExecSafe([scriptblock] $cmd, [int]$maxRetries = 0) {
    $tryCount = 0
    while ($true) {
        $tryCount++
        & $cmd
        if ($global:LASTEXITCODE -eq 0) { 
            break
        }
        if ($tryCount -gt $maxRetries) {
            exit $global:LASTEXITCODE 
        }  
    }
}

# If dotnet CLI is installed globally and it matches requested version, use for execution
$env:DOTNET_EXE = (Get-Command "dotnet").Path

Write-Output "Microsoft (R) .NET Core SDK version $(& $env:DOTNET_EXE --version)"

ExecSafe { & $env:DOTNET_EXE restore $BuildProjectFile -nologo -clp:NoSummary --verbosity quiet } -maxRetries 2
ExecSafe { & $env:DOTNET_EXE build $BuildProjectFile /nodeReuse:false /p:UseSharedCompilation=false -nologo -clp:NoSummary --verbosity quiet --no-restore }
ExecSafe { & $env:DOTNET_EXE run --project $BuildProjectFile --no-build -- $BuildArguments }
