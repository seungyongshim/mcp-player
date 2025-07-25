using System.ComponentModel;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

/// <summary>
/// Sample MCP tools for demonstration purposes.
/// These tools can be invoked by MCP clients to perform various operations.
/// </summary>
internal class RandomNumberTools
{
    [McpServerTool]
    [Description("작성한 글을 평가합니다.")]
    public async Task<string> GetRandomNumber(
        IMcpServer mcpServer,
        [Description("글")] string document = "",
        CancellationToken ct = default)
    {

        ChatMessage[] messages =
        [
            new(ChatRole.User, """
            다음 글을 평가해 주세요. 0 ~ 1 사이 소수로 평가합니다.

            {
                "창의성" : {{creativity}},
                "유용성" : {{usefulness}},
                "명확성" : {{clarity}},
                "전문성" : {{expertise}},
                "전반적인 품질" : {{overall_quality}}
            }
            
            ---
            """),
            new(ChatRole.User, document)
        ];

        return $"{await mcpServer.AsSamplingChatClient().GetResponseAsync(messages, new()
        {
            MaxOutputTokens = 256,
            Temperature = 0.3f
        }, ct)}";
    }
}
