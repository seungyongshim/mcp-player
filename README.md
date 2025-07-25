# MCP Player

이 저장소는 Model Context Protocol C# SDK의 학습 및 테스트를 위한 프로젝트입니다.

## 개요

MCP (Model Context Protocol) C# SDK를 사용하여 다양한 MCP 서버와 클라이언트 구현을 실습하고 테스트하는 목적으로 만들어졌습니다.

## 프로젝트 구조

### `src/McpAspServer/`
- ASP.NET Core 기반 MCP 서버 구현
- 웹 API와 MCP 프로토콜을 결합한 예시

## 시작하기

### 필수 조건
- .NET 10.0 SDK 이상

### 빌드 및 실행

```pwsh
# 전체 솔루션 빌드
dotnet build src/mcp-player.sln

# McpAspServer 실행  
dotnet run --project src/McpAspServer/McpAspServer.csproj
```

## 참고 자료

- [MCP C# SDK 공식 샘플](https://github.com/modelcontextprotocol/csharp-sdk/tree/main/samples)
- [Model Context Protocol 공식 문서](https://modelcontextprotocol.io/)
- [MCP 사양](https://spec.modelcontextprotocol.io/)

## VS Code MCP 설정

`.vscode/mcp.json` 파일을 통해 VS Code에서 MCP 서버를 설정할 수 있습니다.

## 라이선스

이 프로젝트는 학습 목적으로 제공됩니다.
