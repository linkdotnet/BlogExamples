// See https://aka.ms/new-console-template for more information

using Raven.Client.Documents;
using RavenDBUnitTest;
using RavenDBUnitTest.Infrastructure;

var documentStore = new DocumentStore 
{
    Urls = new[] { "http://127.0.0.1:8080" },
    Database = "StevenSample",
};
documentStore.Initialize();

var repository = new Repository<BlogPost>(documentStore);
var blogPost = new BlogPost { Title = "Hello World", Content = "Some text" };
await repository.StoreAsync(blogPost);