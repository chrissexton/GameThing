module Server.App

open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.V2.ContextInsensitive
open Microsoft.Extensions.DependencyInjection
open Giraffe

open Game.Game

// ---------------------------------
// Models
// ---------------------------------

type Message =
    {
        Text: string
    }

// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title [] [ encodedText "Server" ]
                link [ _rel "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
                script [ _src "//cdnjs.cloudflare.com/ajax/libs/rot.js/0.6.0/rot.js" ] []
                script [ _src "//unpkg.com/vue" ] []
                script [ _src "//unpkg.com/axios/dist/axios.min.js" ] []
                script [ _src "/main.js"; _async ] []
            ]
            body [] content
        ]

    let partial() =
        h1 [] [ encodedText "Dis my dumb game" ]

    let display() =
        div [ _id "game" ] []

    let index () =
        [
            partial()
            display()
        ] |> layout

// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name: string) =
    let view = Views.index ()
    htmlView view

type mapResponse =
    {
        view: string
    }

let mapHandler() =
    let floor = [
        [ Wall Corner; Wall EW; Wall EW; Wall EW; Wall Corner ];
        [ Wall NS; Floor; Floor; Floor; Wall NS ];
        [ Wall NS; Floor; Player; Floor; Wall NS ];
        [ Wall NS; Floor; Floor; Floor; Wall NS ];
        [ Wall Corner; Wall EW; Wall EW; Wall EW; Wall Corner ]
    ]
    json (floorToString floor)

let webApp =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler "world"
                routef "/hello/%s" indexHandler
                route "/map" >=> mapHandler()
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app: IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseGiraffe(webApp)

let configureServices (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0
