module Game.Game

type direction =
    | NS
    | EW
    | NE
    | NW
    | SE
    | SW
    | Corner

type env =
   | Wall of direction
   | Floor
   | Trap
   | Player


let envToString e =
    match e with
        | Wall d ->
            match d with
                | NS -> "|"
                | EW -> "-"
                | NE -> "\\"
                | NW -> "/"
                | SE -> "/"
                | SW -> "\\"
                | Corner -> "+"
        | Floor -> "."
        | Trap -> "!"
        | Player -> "@"

let rowToString = List.fold (fun r e -> r + envToString e) ""

let floorToString = List.fold (fun f r -> f + rowToString r + "\n") ""

let rowToStringList = List.map (fun e -> envToString e)

let floorToStringLists = List.map rowToStringList