module Game.Game


type wallType =
    | NS
    | EW
    | NE
    | NW
    | SE
    | SW
    | Corner

type direction =
| North
| South
| East
| West

type env =
   | Wall of wallType
   | Floor
   | Trap
   | Player
   
type cast = {
    spell: string
    direction: direction
}
type command =
    | Move of direction
    | Refresh
    | Cast of cast

type location =
    {
        x: int
        y: int
    }
type state =
    {
        board: string
        playerLocation: location
    }

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