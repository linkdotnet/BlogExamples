# Blazor Client - Loading Screen

If you are using Blazor WebAssembly aka client-side Blazor you are faced with an issue: The .NET runtime including your assemblies has to be downloaded first. We are taking about some megabytes as the initial load.

Depending on the connection of your client there is a time where basically nothing happens. The default template just has a simple "Loading..." text. So let's change that.

Found [here](https://steven-giesel.com/update/6dc1be8f-159c-45fb-90c3-7ec87bfd7e3b)