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
    throw new InvalidOperationException("User secret 'GitHub:ApiKey' not found. Add it with 'dotnet user-secrets set \"GitHub:ApiKey\" \"<your-key>\"'.");
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

// French Translator Agent
AIAgent frenchAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "FrenchAgent",
        Instructions = "You are a translation assistant that translates the provided text to French."
    });

// Spanish Translator Agent
AIAgent spanishAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "SpanishAgent",
        Instructions = "You are a translation assistant that translates the provided text to Spanish."
    });

// Quality Reviewer Agent
string qualityReviewerAgentInstructions = """
You are a multilingual translation quality reviewer.
Check the translations for grammar accuracy, tone consistency, and cultural fit
compared to the original English text.

Give a brief summary with a quality rating (Excellent / Good / Needs Review).

Example output:
Quality: Excellent
Feedback: Accurate translation, friendly tone preserved, minor punctuation tweaks only.
""";

AIAgent qualityReviewerAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "QualityReviewerAgent",
        Instructions = qualityReviewerAgentInstructions
    });

// Summary Agent
string summaryAgentInstructions = """
You are a localization summary assistant.
Summarize the translation results below.
For each language, list:
- Translation quality
- Tone feedback
- Any corrections made

Then, provide an overall summary in 3-5 lines.

Example output:
=== Localization Summary ===
French: Excellent (minor punctuation fixes)
Spanish: Good (tone consistent)
All translations reviewed successfully.
""";

AIAgent summaryAgent = new ChatClientAgent(
    chatClient,
    new ChatClientAgentOptions
    {
        Name = "SummaryAgent",
        Instructions = summaryAgentInstructions
    });

AIAgent workflowAgent = AgentWorkflowBuilder
    .BuildSequential(frenchAgent, spanishAgent, qualityReviewerAgent, summaryAgent).AsAgent();

Console.Write("\nYou: ");
string userInput = Console.ReadLine() ?? string.Empty;

AgentRunResponse response = await workflowAgent.RunAsync(userInput);

Console.WriteLine();

foreach (var message in response.Messages)
{
    Console.WriteLine($"{message.AuthorName}: ");

    Console.WriteLine(message.Text);
    Console.WriteLine();
}