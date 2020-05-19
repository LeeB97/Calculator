# Calculator client-server 

## How to build the application and deploy

The app has three parts:
- Client (in console): sends the http requests to the server.
- Server: recive the request and sends a response.
- Library: set of models that the client and servers use.

Requirements:
- Install RestShap (for the http requests) and NLog (for logs).
- On the server in "App_Start/WebApiConfig.cs" change the routeTemplate to "api/{controller}/{action}/{id}" (add action for the correct readability of the routes)

## How to run the resulting application.

Just execute the app on VS starting the client and server at the same time.
