@ECHO OFF
ECHO\
ECHO Usage:
ECHO     DDProf-SetEnv.bat [Bit-ness] [Deploy Directory]

ECHO\
IF "%1"=="" (
    ECHO Bit-ness optional command line parameter is not specified:
    ECHO     The bit-ness is required to populate the XXX_PROFILER_PATH environment variables
    ECHO     "COR_PROFILER_PATH" and "CORECLR_PROFILER_PATH".
    ECHO     These environment variables will not be defined.
    ECHO\
    ECHO     The environment variables "COR_PROFILER_PATH_64" and "CORECLR_PROFILER_PATH_64", and
    ECHO     the environment variables "COR_PROFILER_PATH_32" and "CORECLR_PROFILER_PATH_32" will be used.
    ECHO     In most cases this is sufficient.
    ECHO     However, older .NET Framework versions may require the XXX_PROFILER_PATH setting.
    ECHO\
    ECHO     Specify "x64" or "x86", without the quotes, for the first command line parameter
    ECHO     to populate the XXX_PROFILER_PATH variables.

    SET SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS=
) ELSE (
    ECHO Bit-ness specified.
    ECHO     The XXX_PROFILER_PATH environment variables
    ECHO     "COR_PROFILER_PATH" and "CORECLR_PROFILER_PATH" will be defined.

    SET SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS=%1
)

ECHO\
IF "%2"=="" (
    ECHO Deploy Directory optional command line parameter is not specified:
    ECHO     Use the second command line parameter to specify the DD-Prof-DotNet Alpha Deploy Directory.
    ECHO     Using default.

    SET SIGNALFX_DOTNET_PROFILER_HOME=%~dp0
) ELSE (
    SET SIGNALFX_DOTNET_PROFILER_HOME=%2

    ECHO Deploy Directory specified.
)

SET COR_ENABLE_PROFILING=1
SET COR_PROFILER={BD1A650D-AC5D-4896-B64F-D6FA25D6B26A}
SET COR_PROFILER_PATH_64=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.x64.dll
SET COR_PROFILER_PATH_32=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.x86.dll

SET CORECLR_ENABLE_PROFILING=1
SET CORECLR_PROFILER={BD1A650D-AC5D-4896-B64F-D6FA25D6B26A}
SET CORECLR_PROFILER_PATH_64=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.x64.dll
SET CORECLR_PROFILER_PATH_32=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.x86.dll

IF DEFINED SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS (
    SET COR_PROFILER_PATH=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.%SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS%.dll
    SET CORECLR_PROFILER_PATH=%SIGNALFX_DOTNET_PROFILER_HOME%Datadog.AutoInstrumentation.Profiler.Native.%SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS%.dll
) ELSE (
    SET COR_PROFILER_PATH=
    SET CORECLR_PROFILER_PATH=
)

SET COMPlus_EnableDiagnostics=1
SET SIGNALFX_PROFILING_ENABLED=1

ECHO\
ECHO SIGNALFX_DOTNET_PROFILER_HOME=%SIGNALFX_DOTNET_PROFILER_HOME%
ECHO\
ECHO COR_ENABLE_PROFILING=%COR_ENABLE_PROFILING%
ECHO COR_PROFILER=%COR_PROFILER%
ECHO COR_PROFILER_PATH_64=%COR_PROFILER_PATH_64%
ECHO COR_PROFILER_PATH_32=%COR_PROFILER_PATH_32%
ECHO\
ECHO CORECLR_ENABLE_PROFILING=%CORECLR_ENABLE_PROFILING%
ECHO CORECLR_PROFILER=%CORECLR_PROFILER%
ECHO CORECLR_PROFILER_PATH_64=%CORECLR_PROFILER_PATH_64%
ECHO CORECLR_PROFILER_PATH_32=%CORECLR_PROFILER_PATH_32%

IF DEFINED SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS (
    ECHO\
    ECHO SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS=%SIGNALFX_DOTNET_CONTINUOUS_PROFILER_BITNESS%
    ECHO\
    ECHO COR_PROFILER_PATH=%COR_PROFILER_PATH%
    ECHO CORECLR_PROFILER_PATH=%CORECLR_PROFILER_PATH%
)

ECHO\
ECHO ON
