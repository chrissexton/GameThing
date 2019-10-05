namespace Server

open Giraffe
open GiraffeViewEngine

// ---------------------------------
// Views
// ---------------------------------

module Views =
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
        div [ _id "app" ] [
            div [ _id "game" ] []
            button [ _id "south"; _value "south" ] [ Text "South" ]
        ]

    let index() =
        [
            partial()
            display()
        ] |> layout