# Diffbot API client for .NET

## Preface

This API is developed for .NET 4.0 and later. It is developed taking into advantage the asynchronous methods introduced in .Net 4.0 with Tasks, So it is compatible with async/await structure.

If you are developing in .NET 4.0, it is recommended to install from NuGet the package Microsoft Async that will enable the use of async/await in this version of .NET. This would make much easier to work with Async methods. The examples below assumes that you have installed it.

## Installation

To install the API in your project you can use NuGet in Visual Studio or download the zip from: 

NuGet Install: 

`Install-Package Diffbot.Api.Client`

## Configuration

The API doesn't require any special configuration as all the parameters needed for it is passed on the constructor of the API (i.e. api url, version, token, etc)

## Usage

There is one class for each API. In general each instance of a class is created with three parameters:
**Base URL** is the base addres of the API. You should check the API documentation to know which one is the base url (Right now there are two possible values, http://www.diffbot.com or http://api.diffbot.com)
**Token** is your developer token
**Version** is the version of the API. This value is needed in case your base url is http://api.diffbot.com otherwise it should be "0".

### Article, Product, Image, FrontPage, PageClassifier API

All this APIs works in a similar way. The next samples uses ArticleAPI but the other APIs are similar except that it uses corresponding API.
The calls return a typed result instead of a JSON object.

Create an instance of the API.
```cs
ArticleApi api = new ArticleApi("http://api.diffbot.com", "<your developer token>", "2");
```

Now you can call the API passing the url to process and any additional parameter you might need (refer to the API documentation).
The parameters for this call are:
**URL** is the url to process
**fields** is a string array with the fields you want to retrive. Use "*" (asterisk) to retrieve all fields
**optionaParameter** is a string dictionary with additional parameters that can be passed to the api (see API documentation).
```cs
Article article = await api.GetArticleAsync("<url to process>", new string[] { "*" }, null);

if (article != null) {
  // Your code here
  string title = article.Title; // get the title of the article
}
```

Those APIs that allow to POST the HTML have the Get<api>Async method overloaded to be able to pass the html. This HTML could be passed as a string or as a stream (file).

Pass HTML as string:
```cs
string html = "<your html here>";
Article article = await api.GetArticleAsync("<url to process>", new string[] { "*" }, null, html);

if (article != null) {
  // Your code here
  string title = article.Title; // get the title of the article
}
```

Pass HTML as a file:
```cs
using (var file = System.IO.File.OpenRead("demo.html"))
{
  Article article = await api.GetArticleAsync("<url to process>", new string[] { "*" }, null, file);

  if (article != null)
  {
    // your code here
  }
}

```

### Custom API

Instantiating and calling a Custom API is similar to previous API except in this case it will return a JSON object JObject.

```cs
CustomApi api = new CustomApi("http://api.diffbot.com", "<your developer token>", "2");

JObject result = await api.CallCustomAPIAsync("<url to process>", "<name of the custom api", null);

if (result != null)
{
  // Your code here
  // string title = (string)result["Title"];
}
```

### Crawlbot API

Crawlbot API differs in from the previous API. This are the steps to use this api:

1. First we need to instantiate the class with two parameters, the base url and its version.
2. Create a CrawlbotSettings object to setup the call. The main properties to fill are Token, Name, Seeds and ApiUrl. Check the documentation of the API to know what each of this properties are and what additional properties do you have.
3. Call the CreateUpdateJobAsync method passing the settings as parameters. This will return a CrawlJob that you can use later to query the result, pause, stop and delete the crawl.
4. Process results calling using the url returned by the job: DownloadJson
5. Call any other method you need passing the CrawlJob as parameter. If you call CreateUpdateJobAsync with the same settings, it will update the job if it is already exists.
 

```cs
// Step 1
CrawlbotAPI api = new CrawlbotAPI("http://api.diffbot.com", "2");

 // Step 2
CrawlbotSettings settings = new CrawlbotSettings();
settings.Token = "Developer token";
settings.Name = "MyCrawlJob";
settings.Seeds.Add("<url1>");
settings.Seeds.Add("<url2>");
settings.ApiUrl = "http://api.diffbot.com/v2/article?.......";

// Step 3
CrawlJob job = await api.CreateUpdateJobAsync(settings);

// Step 4
var urlResult = job.DownloadJson;

// Step 5
CrawlJob job = await api.PauseAsync(job); // Pause a job
```

The available methods are:
```cs
api.PauseAsync(job);
api.ResumeAsync(job);
api.RestartAsync(job);
api.DeleteAsync(job);
```


### Bulk API

Bulk API is very similar to CrawlbotAPI. This are the steps to use this api:

1. First we need to instantiate the class with two parameters, the base url and its version.
2. Create a BulkJobSettings object to setup the call. The main properties to fill are Token, Name, Urls and ApiUrl. Check the documentation of the API to know what each of this properties are and what additional properties do you have.
3. Call the CreateJob method passing the settings as parameters. This will return a BulkJob that you can use later to query the result, pause, stop and delete the crawl.
4. Start the Job the results can be found in the property JobStatus
5. Call any other method you need passing the BulkJob as parameter. If you call CreateUpdateJobAsync with the same settings, it will update the job if it is already exists.
 

```cs
// Step 1
BulkApi api = new BulkApi("http://api.diffbot.com", "2");

 // Step 2
BulkJobSettings settings = new BulkJobSettings();
settings.Token = "Developer token";
settings.Name = "MyBulkJob";
settings.Urls.Add("<url1>");
settings.Urls.Add("<url2>");
settings.ApiUrl = "http://api.diffbot.com/v2/article?.......";

// Step 3
BulkJob job = api.CreateJob(settings);

// Step 4
await api.StartJobAsync(job);
if (!job.HasErrors) {
  // Your code here
  // job.JobStatus.DownloadJson is the URL to get the results of the Bulk operation
}

// Step 5
CrawlJob job = await api.PauseAsync(job); // Pause a job
```

The available methods are:
```cs
api.PauseAsync(job);
api.ResumeAsync(job);
api.DeleteAsync(job);
```

### Batch API

Batch API allows to submit multiple API calls at once. As in the previous APIs you need to instantiate the class BatchApi passing the base url and version and then call the CreateBatchAsync method
The difference is that in needs a list of BatchUrlRequest object with the info of each of the urls you want to process. 

```cs
BatchApi api = new BatchApi("http://api.diffbot.com", "2");

List<BatchUrlRequest> urls = new List<BatchUrlRequest>();

urls.Add(new BatchUrlRequest()
{
  Method = "GET",
  Url = "http://api.diffbot.com/v2/api/article?......."
});

BatchResult batchResult = await api.CreateBatchAsync("token", urls);
```

### Error handling

All the operations in the API are susceptible to errors so it is a good practice to put the calls in a try/catch block.

```cs
try
{
  Article article = await api.GetArticleAsync("<url to process>", new string[] { "*" }, null, file);
} 
catch (Exception ex) 
{
  Console.WriteLine(ex.ToString());
}
```

-Initial Commit by Diego Bao Montero-
