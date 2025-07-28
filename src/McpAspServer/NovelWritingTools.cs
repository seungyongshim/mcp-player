using System.ComponentModel;
using McpAspServer;
using ModelContextProtocol.Server;

[McpServerToolType]
internal class NovelWritingTools
{
    [McpServerTool]
    [Description("소설을 작성합니다.")]
    public async static Task<object> WriteNovel(
        IMcpServer mcpServer,
        [Description("작성지침")] string document,
        [Description("outputfilefullpath")] string outputfilefullpath,
        [Description("참고자료들의 파일 fullpath 경로")] string[] referenceFileFullPaths)
    {
        referenceFileFullPaths = referenceFileFullPaths ?? [];

        var ret = await mcpServer.SamplingAsync("""
            글을 작성해 주세요.
            """, document, CopilotModelType.Claude37SonnetThought);
        
        var file = new FileInfo(outputfilefullpath);

        if (!file.Directory?.Exists ?? false)
            file.Directory?.Create();

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
    public async Task<object> EvaluateDocument(
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

