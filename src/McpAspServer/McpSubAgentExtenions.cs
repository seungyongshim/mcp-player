namespace McpAspServer;

using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

public static class McpSubAgentExtenions
{
    
    public static async ValueTask<string> ModifiedStringAsync(this IMcpServer mcp, string message)
    {
        var ret = await mcp.SamplingAsync("""

            """, message, CopilotModelType.Gpt41);




        return ret;
    }
}
