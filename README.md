# Run the presentations

The presentations use the [Reveal JS](https://github.com/hakimel/reveal.js) framework.  In order for the presentations to work correctly, the html pages need to be served up via a web server.

A very simple web server writen in Dotnet Core is packaged here.  To run it, you will need the Dot Net CLI.

From your command line, change into the `localserver` folder, and run

``` txt
dotnet run
```

Once that is running, open the main presentation by browsing to `http://localhost:5000/lessons/index.html`