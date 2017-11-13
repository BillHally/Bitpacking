module Bitpacking.Tests.TestSignArray

open System

open Bitpacking

open Xunit

open Hedgehog

[<Fact>]
let ``All possible combinations of 0 to 10 sign values can be set and correctly retrieved`` () =
    property
        {
            let! signs =
                Gen.array
                    (Range.constant 0 10)
                    (
                        Gen.choice
                            [|
                                Gen.sbyte (Range.singleton -1y)
                                Gen.sbyte (Range.singleton  0y)
                                Gen.sbyte (Range.singleton  1y)
                            |]
                    )

            let bytes = SignArray.create signs.Length
            SignArray.setSigns (bytes, signs)

            let actual = Array.zeroCreate signs.Length
            SignArray.getSigns (bytes, actual)

            let input = signs |> Array.map int

            return actual = input
        }
        |> Property.check
