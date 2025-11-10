// See https://aka.ms/new-console-template for more information
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;
using System.Reflection;

Console.WriteLine("AI Agent Workflow demo");

var config = new ConfigurationBuilder()
    .AddUserSecrets(Assembly.GetEntryAssembly()!)
    .Build();

var apiKey = config["GitHub:ApiKey"];
if (string.IsNullOrWhiteSpace(apiKey))
{
    throw new InvalidOperationException("GitHub ApiKey not found.");
}

IChatClient chatClient =
    new ChatClient(
        "gpt-4o-mini",
        new ApiKeyCredential(apiKey),
        new OpenAIClientOptions
        {
            Endpoint = new Uri("https://models.github.ai/inference")
        })
    .AsIChatClient();

AIAgent csharpWriter = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "CSharpWriterAgent",
        Instructions = "You are an expert C# programmer. Write code snippets based on the user's requests."
    });

AIAgent codeReviewer = new ChatClientAgent(chatClient,
    new ChatClientAgentOptions
    {
        Name = "CodeReviewerAgent",
        Instructions = """
        You are a code reviewer. Review the provided C# code snippets for best practices,
        performance, and readability. Suggest improvements where necessary.
        """
    });

AIAgent javaWriter = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "JavaWriterAgent",
        Instructions = "You are an expert Java programmer. Translate provided code snippets to Java."
    });

AIAgent workflow = AgentWorkflowBuilder.BuildSequential("CSharpCodeWorkflow", new List<AIAgent>
{
    csharpWriter,
    codeReviewer,
    javaWriter
}).AsAgent();

var input = "Create a C# function that lists all the prime numbers till the specifed one.";

var result = await workflow.RunAsync(input);

Console.WriteLine("Input:");
Console.WriteLine(input);

Console.WriteLine("Output:");
Console.WriteLine(result);