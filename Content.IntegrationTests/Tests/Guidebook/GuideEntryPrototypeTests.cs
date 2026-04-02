using System.Linq;
using Content.Client.Guidebook;
using Content.Client.Guidebook.Richtext;
using Content.Shared.Guidebook;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Guidebook;

[TestFixture]
[TestOf(typeof(GuidebookSystem))]
[TestOf(typeof(GuideEntryPrototype))]
[TestOf(typeof(DocumentParsingManager))]
public sealed class GuideEntryPrototypeTests
{
    [Test]
    [Description("Ensures a given guidebook entry is valid, checking the document/etc.")]
    public async Task ValidateAllGuideEntries()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings { Connected = true });
        var client = pair.Client;
        await client.WaitIdleAsync();
        var protoMan = client.ResolveDependency<IPrototypeManager>();
        var resMan = client.ResolveDependency<IResourceManager>();
        var parser = client.ResolveDependency<DocumentParsingManager>();
        var allProtos = protoMan.EnumeratePrototypes<GuideEntryPrototype>().ToList();
        Assert.That(allProtos, Is.Not.Empty, "No guidebook entries found.");

        foreach (var proto in allProtos)
        {
            await client.WaitAssertion(() =>
            {
                using var reader = resMan.ContentFileReadText(proto.Text);
                var text = reader.ReadToEnd();
                Assert.That(parser.TryAddMarkup(new Document(), text),
                    $"Failed to parse guide entry: {proto.Id}");
            });
        }

        await pair.CleanReturnAsync();
    }
}
