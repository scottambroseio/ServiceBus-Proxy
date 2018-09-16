# ServiceBus Proxy

A proof of concept demonstrating a mechanism to get around the fact that azure functions aren't session aware when it comes to service bus triggers.

This is a simple dotnet core console application which acts as a session aware proxy, reading messages from service bus in a session aware manner before forwarding them to an azure function using HTTP requests. This example has been implemented with DotNet Core but this is by no means a requirement. Any language with a ServiceBus SDK exposing the required functionality would work. With this approach, this can be run on Linux or Windows, in the cloud or on prem, as a console application, Windows service, Linux daemon or simply run inside a Docker container.

Note: Running on Windows / Linux is obviously dependant of the choice of programming language used.
