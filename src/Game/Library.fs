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

type Board = Env list list

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

let newBoard() = [
         [ Wall Corner; Wall EW; Wall EW; Wall EW; Wall Corner ];
         [ Wall NS; Floor; Floor; Floor; Wall NS ];
         [ Wall NS; Floor; Floor; Floor; Wall NS ];
         [ Wall NS; Floor; Floor; Floor; Wall NS ];
         [ Wall Corner; Wall EW; Wall EW; Wall EW; Wall Corner ]
     ]

type StateJSON =
    {
        board: string
        playerLocation: Location
    }

type State(board: Board) =
    let mutable _playerLocation = { x = 1; y = 1 }
    member this.boardString = floorToString board
    member this.playerLocation = _playerLocation
    member this.JSON = {
        board = this.boardString
        playerLocation = _playerLocation
    }
    member this.cmd cmd =
        match cmd with
            | Move d ->
                match d with
                    // Note that the coordinate system is pinned to NW
                    | North -> _playerLocation <- { x = _playerLocation.x; y = _playerLocation.y - 1 }
                    | South -> _playerLocation <- { x = _playerLocation.x; y = _playerLocation.y + 1 }
                    | East -> _playerLocation <- { x = _playerLocation.x + 1; y = _playerLocation.y }
                    | West -> _playerLocation <- { x = _playerLocation.x - 1; y = _playerLocation.y }
            | _ -> ()

let mutable Current = State(newBoard())
