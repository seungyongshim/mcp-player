using System.ComponentModel;
using McpAspServer;
using Microsoft.Extensions.AI;
using ModelContextProtocol;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly()
    .WithPromptsFromAssembly();

var app = builder.Build();

app.MapMcp();
app.Run();


[McpServerToolType]
internal class RandomNumberTools
{
    [McpServerTool]
    [Description("소설을 작성합니다.")]
    public async static Task<object> Write(
        IMcpServer mcpServer,
        [Description("작성지침")] string document,
        [Description("outputfilefullpath")] string outputfilefullpath)
    {
        var ret = await mcpServer.SamplingAsync("""
            글을 작성해 주세요.
            """, document, CopilotModelType.Claude37SonnetThought);
        
        var file = new FileInfo(outputfilefullpath);

        if (!file.Directory.Exists)
            file.Directory.Create();

        await File.WriteAllTextAsync(file.FullName, ret);

        return new
        {
            file.FullName,
            file.Length,
            file.LastWriteTimeUtc
        };
    }

    [McpServerTool]
    [Description("작성한 글을 평가합니다.")]
    public async Task<object> GetRandomNumber(
        IMcpServer mcpServer,
        [Description("글")] string document)
    {
        return await mcpServer.SamplingAsync("""
            다음 글을 평가해 주세요. 0 ~ 1 사이 소수로 평가합니다.
            
            {
                "창의성" : {{creativity}},
                "유용성" : {{usefulness}},
                "명확성" : {{clarity}},
                "전문성" : {{expertise}},
                "전반적인 품질" : {{overall_quality}}
            }
            """, document, CopilotModelType.Claude37SonnetThought);
    }
}

