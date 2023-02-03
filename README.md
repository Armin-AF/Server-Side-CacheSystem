# Server-side Cache System in .Net using GitHub Api

## Introduction

This project is a sample project that demonstrates the use of an API client to retrieve information about a GitHub user. The API client communicates with the GitHub API to retrieve information about a user, such as their avatar URL, HTML URL, ID, login, and URL.

This project aims to build a server-side cache system for the GitHub API using in-memory caching technology in .Net, to improve API response times and enhance the overall performance of the application.

The cache system will be implemented using the System.Runtime.Caching namespace, and will store the retrieved data in a MemoryCache object, with an expiration policy to automatically evict stale data.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

## Prerequisites

What things you need to install the software and how to install them

* [.NET Core 6.0](https://dotnet.microsoft.com/download) - The .NET Core runtime

## Getting Started

To get started with this project, follow these steps:

* Clone the repository to your local machine using the following command:

```csharp
https://github.com/Armin-AF/server-side-cache-system.git
```

* Open the solution in Visual Studio or your preferred .NET development environment.

* Restore the NuGet packages required for the project.

* Build the solution to ensure that it compiles without errors.

* To build the library from source, you will need to have the .NET Core SDK installed.

```csharp
git clone https://github.com/Armin-AF/Server-Side-CacheSystem.git
cd github-api-client
dotnet build
```

#### Installing from NuGet

You can install the library as a NuGet package by running the following command in the Package Manager Console:

```csharp
Install-Package GitHubApiClient
```

## Usage

The library provides a simple and intuitive API for accessing the GitHub API.

```csharp
var client = new GitHubClient();
var user = await client.GetUserAsync("testuser");
```


## Using the API Client

The API client is implemented as a class called GitHubApiClient in the GitHubApiClient namespace. To use the API client, you can instantiate an instance of the class and call the GetUserAsync method, passing in a GitHub username as a parameter. The method returns a Task that, when awaited, will return a GitHubUser object containing information about the specified user.

* Here's an example of how to use the API client:

```csharp
using System;
using System.Threading.Tasks;
using GitHubApiClient;

namespace GitHubApiClientSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var apiClient = new GitHubApiClient();
            var user = await apiClient.GetUserAsync("testuser");
            Console.WriteLine(user.Login);
        }
    }
}
```



## Authenticating

If you need to access protected resources, you will need to authenticate with your GitHub account. You can do this by providing a personal access token when creating the GitHubClient instance.

```csharp
var client = new GitHubClient("your-personal-access-token");
```


## Testing the API Client

The project includes a set of tests for the API client, which can be found in the GitHubApiClient.Tests namespace. The tests use the xUnit testing framework and make use of a mock API client to simulate communication with the GitHub API.

To run the tests, use the test explorer in Visual Studio or run the following command in the package manager console:


```csharp
dotnet test
```

## Front End

This project also includes a front end, built with React, that utilizes the back end API to display information about a GitHub user. The user can enter a GitHub username and receive information such as the user's name, bio, location, number of public repositories, and profile URL. The front end also displays the time taken to fetch the data and whether or not the data was retrieved from cache.

The React code uses functional components and hooks, including useState and useEffect, to manage the local state and fetch data from the API. Axios is used for making the API calls. The UI is styled using a .css file.

The front end allows users to easily access and display information about GitHub users in a visually appealing way. It demonstrates the use of React, hooks, and API communication in a web application.

## Built With

- .NET Core - The .NET Core platform
- HttpClient - The HTTP client used to communicate with the API
- Newtonsoft.Json - The JSON library used to serialize and deserialize API responses
- React - A JavaScript library for building user interfaces
- TypeScript - A statically typed superset of JavaScript that adds features to the language

## License

This project is licensed under the MIT License - see the LICENSE.md file for details
