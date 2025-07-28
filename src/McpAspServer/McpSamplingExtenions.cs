namespace McpAspServer;

using System.Text.Json;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

public static class McpSamplingExtenions
{
    public static ValueTask<string> SamplingAsync
    (
        this IMcpServer mcp, 
        string system, 
        string user, 
        CopilotModelType modelType
    ) => SamplingAsync(mcp, system, user, modelType, []);

    // 정상동작 안함
    public static async ValueTask<T?> SamplingAsync<T>
    (
        this IMcpServer mcp,
        string system,
        string user,
        CopilotModelType modelType,
        IEnumerable<string> reference
    ) where T: class
    {
        var chatClient = mcp.AsSamplingChatClient();

        var ret = await chatClient.GetResponseAsync<T>(messages: [
            new()
            {
                Role = ChatRole.System,
                Contents = [new TextContent(system)]
            },
            new()
            {
                Role = ChatRole.Assistant,
                Contents = [..reference.Select(x => new TextContent(x))],
            },
            new()
            {
                Role = ChatRole.User,
                Contents = [new TextContent(user)]
            }], options: new()
            {
                ModelId = modelType.ToRawCopilotModel()
            });
        return ret?.Result;
    }

    public static async ValueTask<string> SamplingAsync
    (
        this IMcpServer mcp, 
        string system, 
        string user, 
        CopilotModelType modelType,
        IEnumerable<string> reference,
        object? ReturnType = null
    )
    {
        var response = await mcp.SampleAsync([
            new()
            {
                Role = ChatRole.System,
                Contents = [new TextContent(system)]
            },
            new()
            {
                Role = ChatRole.Assistant,
                Contents = [..reference.Select(x => new TextContent(x))],
            },
            new()
            {
                Role = ChatRole.User,
                Contents = [new TextContent(user)]
            }
        ], new()
        {
            ModelId = modelType.ToRawCopilotModel()
        });
        return response.Text;
    }
}

file static class A
{
    public static string ToRawCopilotModel(this CopilotModelType v) => v switch
    {
        CopilotModelType.Gpt41 => "copilot/gpt-4.1",
        CopilotModelType.Claude35Sonnet => "copilot/claude-3.5-sonnet",
        CopilotModelType.Claude37Sonnet => "copilot/claude-3.7-sonnet",
        CopilotModelType.Claude37SonnetThought => "copilot/claude-3.7-sonnet-thought",
        CopilotModelType.ClaudeSonnet4 => "copilot/claude-sonnet-4",
        CopilotModelType.Gemini20Flash001 => "copilot/gemini-2.0-flash-001",
        CopilotModelType.Gemini25Pro => "copilot/gemini-2.5-pro",
        CopilotModelType.Gpt35Turbo => "copilot/gpt-3.5-turbo",
        CopilotModelType.Gpt4 => "copilot/gpt-4",
        CopilotModelType.Gpt40125Preview => "copilot/gpt-4-0125-preview",
        CopilotModelType.Gpt4o => "copilot/gpt-4o",
        CopilotModelType.Gpt4oMini => "copilot/gpt-4o-mini",
        CopilotModelType.O3Mini => "copilot/o3-mini",
        CopilotModelType.O4Mini => "copilot/o4-mini",
        _ => throw new ArgumentOutOfRangeException(nameof(v), v, null)
    };
}

public enum CopilotModelType
{
    Gpt41 = 0,
    Claude35Sonnet = 1,
    Claude37Sonnet = 2,
    Claude37SonnetThought = 3,
    ClaudeSonnet4 = 4,
    Gemini20Flash001 = 5,
    Gemini25Pro = 6,
    Gpt35Turbo = 7,
    Gpt4 = 8,
    Gpt40125Preview = 9,
    Gpt4o = 10,
    Gpt4oMini = 11,
    O3Mini = 12,
    O4Mini = 13
}
