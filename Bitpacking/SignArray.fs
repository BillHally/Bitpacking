module Bitpacking.SignArray

open System

let inline pack (signs : _[])  n =
    (byte (signs.[n    ] + 1y) * 81uy) +
    (byte (signs.[n + 1] + 1y) * 27uy) +
    (byte (signs.[n + 2] + 1y) *  9uy) +
    (byte (signs.[n + 3] + 1y) *  3uy) +
    (byte (signs.[n + 4] + 1y)       )

let lut =
    let values = [| -1; 0; 1 |]

    [|
        for s0 in values do
            for s1 in values do
                for s2 in values do
                    for s3 in values do
                        for s4 in values do
                            yield [| s0; s1; s2; s3; s4 |]
    |]

let unpack (signs : _[]) n (x : byte) =
    Array.Copy(lut.[int x], 0, signs, n, min 5 (signs.Length - n))

let create n : byte[] = Array.zeroCreate ((n + 4) / 5)

let setSigns (xs : byte[], signs : sbyte[]) : unit =
    for i in 0..5..(signs.Length - 5) do
        xs.[i / 5] <- pack signs i

    match signs.Length % 5 with
    | 0 -> ()
    | remaining ->
        let signs =
            Array.init
                5
                (
                    fun i ->
                        if i < remaining then
                            signs.[signs.Length - remaining + i]
                        else
                            0y
                )

        xs.[xs.Length - 1] <- pack signs 0

let getSigns (xs : byte[], signs : int[]) : unit =
    for i = 0 to xs.Length - 1 do
        unpack signs (i * 5) xs.[i]
