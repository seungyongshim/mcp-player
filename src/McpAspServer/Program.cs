using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithTools<RandomNumberTools>();

var app = builder.Build();

app.MapMcp();

app.Run();


[McpServerToolType]
internal class RandomNumberTools
{
    [McpServerTool]
    [Description("작성한 글을 평가합니다.")]
    public async Task<object> GetRandomNumber(
           IMcpServer mcpServer,
           [Description("글")] string document = "",
           CancellationToken ct = default)
    {
        ChatMessage[] messages =
        [
            new()
            {
                Role = ChatRole.System,
                Contents = 
                [
                     new TextContent("""
                    다음 글을 평가해 주세요. 0 ~ 1 사이 소수로 평가합니다.

                    {
                        "창의성" : {{creativity}},
                        "유용성" : {{usefulness}},
                        "명확성" : {{clarity}},
                        "전문성" : {{expertise}},
                        "전반적인 품질" : {{overall_quality}}
                    }
                    ---
                    """)
                     {
                         
                     }
                ]
            },
        ];


        SamplingMessage[] messages =
        [
            new()
            {
                Role = Role.Assistant,
                Content = new TextContentBlock()
                {
                    Text = document
                }
            },
        ];

        var ret = await mcpServer.SampleAsync(new CreateMessageRequestParams()
        {
            Messages = messages,
            IncludeContext = ContextInclusion.AllServers,
            MaxTokens = 256,
            Temperature = 0.75f,
            SystemPrompt = 
        }, ct);

        return ret;
    }
}