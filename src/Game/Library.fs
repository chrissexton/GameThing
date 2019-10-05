module Game.Game


type WallType =
    | NS
    | EW
    | NE
    | NW
    | SE
    | SW
    | Corner

type Direction =
| North
| South
| East
| West

type Env =
   | Wall of WallType
   | Floor
   | Trap
   | Player
   
type Cast = {
    spell: string
    direction: Direction
}
type Command =
    | Move of Direction
    | Refresh
    | Cast of Cast

type Location =
    {
        x: int
        y: int
    }
type State =
    {
        board: string
        playerLocation: Location
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

let rowToStringList = List.map envToString

let floorToStringLists = List.map rowToStringList