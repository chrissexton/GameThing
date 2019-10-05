module Game.Game

type direction =
    | NS
    | EW
    | Corner

type env =
   | Wall of direction
   | Floor
   | Trap


let envToString e =
    match e with
        | Wall d ->
            match d with
                | NS -> "|"
                | EW -> "-"
                | Corner -> "+"
        | Floor -> "#"
        | Trap -> "!"

let rowToString = List.fold (fun r e -> r + envToString e) ""

let floorToString = List.fold (fun f r -> f + rowToString r + "\n") ""