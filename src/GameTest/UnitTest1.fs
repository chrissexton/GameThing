module GameTest

open NUnit.Framework
open Game.Game

[<SetUp>]
let Setup () =
    ()

[<Test>]
let TestFloorToString() =
    let floor = [
        [ Wall Corner; Wall EW; Wall Corner ];
        [ Wall NS; Floor; Wall NS ];
        [ Wall Corner; Wall EW; Wall Corner ]
    ]
    let actual = floorToString floor
    let expected = "+-+\n|.|\n+-+\n"
    Assert.AreEqual(expected, actual)
    
[<Test>]
let TestStateMovement() =
    let current = State(newBoard())
    let expected = 2
    current.cmd (Move South) |> ignore
    let actual = current.playerLocation.y
    Assert.AreEqual(expected, actual)