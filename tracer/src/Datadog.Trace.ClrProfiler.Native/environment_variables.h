#ifndef SIGNALFX_CLR_PROFILER_ENVIRONMENT_VARIABLES_H_
#define SIGNALFX_CLR_PROFILER_ENVIRONMENT_VARIABLES_H_

#include "string.h" // NOLINT

namespace trace {
namespace environment {

    // Sets whether the profiler is enabled. Default is true.
    // Setting this to false disabled the profiler entirely.
    const WSTRING tracing_enabled = WStr("SIGNALFX_TRACE_ENABLED");

    // Sets whether debug mode is enabled. Default is false.
    const WSTRING debug_enabled = WStr("SIGNALFX_TRACE_DEBUG");

    // Sets the paths to integration definition JSON files.
    // Supports multiple values separated with comma, for example:
    // "C:\Program Files\SignalFx .NET Tracing\integrations.json,D:\temp\test_integrations.json"
    const WSTRING integrations_path = WStr("SIGNALFX_INTEGRATIONS");

    // Sets the path to the profiler's home directory, for example:
    // "C:\Program Files\SignalFx .NET Tracing\" or "/opt/signalfx/"
    const WSTRING profiler_home_path = WStr("SIGNALFX_DOTNET_TRACER_HOME");

    // Sets the filename of executables the profiler can attach to.
    // If not defined (default), the profiler will attach to any process.
    // Supports multiple values separated with comma, for example:
    // "MyApp.exe,dotnet.exe"
    const WSTRING include_process_names = WStr("SIGNALFX_PROFILER_PROCESSES");

    // Sets the filename of executables the profiler cannot attach to.
    // If not defined (default), the profiler will attach to any process.
    // Supports multiple values separated with comma, for example:
    // "MyApp.exe,dotnet.exe"
    const WSTRING exclude_process_names = WStr("SIGNALFX_PROFILER_EXCLUDE_PROCESSES");

    // Sets the Agent's host. Default is localhost.
    const WSTRING agent_host = WStr("SIGNALFX_AGENT_HOST");

    // Sets the Agent's port. Default is 8126.
    const WSTRING agent_port = WStr("SIGNALFX_TRACE_AGENT_PORT");

    // Sets the "env" tag for every span.
    const WSTRING env = WStr("SIGNALFX_ENV");

    // Sets the default service name for every span.
    // If not set, Tracer will try to determine service name automatically
    // from application name (e.g. entry assembly or IIS application name).
    const WSTRING service_name = WStr("SIGNALFX_SERVICE");

    // Sets the "service_version" tag for every span that belong to the root service (and not an external service).
    const WSTRING service_version = WStr("SIGNALFX_VERSION");

    // Sets a list of integrations to disable. All other integrations will remain
    // enabled. If not set (default), all integrations are enabled. Supports
    // multiple values separated with comma, for example:
    // "ElasticsearchNet,AspNetWebApi2"
    const WSTRING disabled_integrations = WStr("SIGNALFX_DISABLED_INTEGRATIONS");

    // Sets the path for the profiler's log file.
    // Environment variable SIGNALFX_TRACE_LOG_DIRECTORY takes precedence over this setting, if set.
    const WSTRING log_path = WStr("SIGNALFX_TRACE_LOG_PATH");

    // Sets the directory for the profiler's log file.
    // If set, this setting takes precedence over environment variable SIGNALFX_TRACE_LOG_PATH.
    // If not set, default is
    // "%ProgramData%"\SignalFx .NET Tracing\logs\" on Windows or
    // "/var/log/signalfx/dotnet/" on Linux.
    const WSTRING log_directory = WStr("SIGNALFX_TRACE_LOG_DIRECTORY");

    // Sets whether to disable all JIT optimizations.
    // Default value is false (do not disable all optimizations).
    // https://github.com/dotnet/coreclr/issues/24676
    // https://github.com/dotnet/coreclr/issues/12468
    const WSTRING clr_disable_optimizations = WStr("SIGNALFX_CLR_DISABLE_OPTIMIZATIONS");

    // Sets whether to intercept method calls when the caller method is inside a
    // domain-neutral assembly. This is dangerous because the integration assembly
    // OpenTelemetry.AutoInstrumentation.dll must also be loaded domain-neutral,
    // otherwise a sharing violation (HRESULT 0x80131401) may occur. This setting should only be
    // enabled when there is only one AppDomain or, when hosting applications in IIS,
    // the user can guarantee that all Application Pools on the system have at most
    // Default is false. Only used in .NET Framework 4.5 and 4.5.1.
    // https://github.com/DataDog/dd-trace-dotnet/pull/671
    const WSTRING domain_neutral_instrumentation = WStr("SIGNALFX_TRACE_DOMAIN_NEUTRAL_INSTRUMENTATION");

    // Indicates whether the profiler is running in the context
    // of Azure App Services
    const WSTRING azure_app_services = WStr("SIGNALFX_AZURE_APP_SERVICES");

    // The app_pool_id in the context of azure app services
    const WSTRING azure_app_services_app_pool_id = WStr("APP_POOL_ID");

    // The DOTNET_CLI_TELEMETRY_PROFILE in the context of azure app services
    const WSTRING azure_app_services_cli_telemetry_profile_value = WStr("DOTNET_CLI_TELEMETRY_PROFILE");

    // Determine whether to instrument calls into netstandard.dll.
    // Default to false for now to avoid the unexpected overhead of additional spans.
    const WSTRING netstandard_enabled = WStr("SIGNALFX_TRACE_NETSTANDARD_ENABLED");

    // Enable the profiler to dump the IL original code and modification to the log.
    const WSTRING dump_il_rewrite_enabled = WStr("SIGNALFX_DUMP_ILREWRITE_ENABLED");

    // Sets whether to enable JIT inlining
    const WSTRING clr_enable_inlining = WStr("SIGNALFX_CLR_ENABLE_INLINING");

    // Sets whether to enable the CallTarget instrumentation mode
    const WSTRING calltarget_enabled = WStr("SIGNALFX_TRACE_CALLTARGET_ENABLED");

    // Custom internal tracer profiler path
    const WSTRING internal_trace_profiler_path = WStr("SIGNALFX_INTERNAL_TRACE_NATIVE_ENGINE_PATH");

    // Sets whether to enable NGEN images.
    const WSTRING clr_enable_ngen = WStr("SIGNALFX_CLR_ENABLE_NGEN");

} // namespace environment
} // namespace trace

#endif
