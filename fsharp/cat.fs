(**
 * Copyright (c) 2015 Scott Bennett
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * 
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 **)

(**
 * cat - A reimplimentation of the Unix utility in F#
 *
 * This program takes a list of files as its arguments and prints the 
 * contents to stdout.
 * 
 * Note: This program is not POSIX compliant.
 **)
module Cat
    open System
    open System.IO

    (* Usage information. *)
    let usage = "usage: cat [file ...]"

    (**
     * Test whether a file exists (given a fully qualified path).
     * 
     * param - full path name of the file
     * return - true if the file does exist; false otherwise
     **)
    let exists filePath = 
        if not(File.Exists(filePath)) then
            false
        else 
            true

    (**
     * Print a character byte to stdout.
     * 
     * param - a character byte in the form of an integer
     * return - true if more bytes should be read from a file stream; false 
     *     otherwise
     **)
    let printByte b = 
        match b with
        | -1 -> 
            printfn ""
            false
        | _ -> 
            Char.ConvertFromUtf32 b |> printf "%s"
            true

    (**
     * Try to print the contents of a file (given a fully qualified path name).
     * 
     * param - full path to a file
     **)
    let printFile filePath = 
        if exists filePath then
            use stream = File.OpenRead(filePath)
            let mutable keepReading = true

            while keepReading do
                keepReading <- printByte <| stream.ReadByte()

        else
            printfn "cat: \'%s\' does not exist" filePath

    (**
     * Attempt to read and print the contents of the specified file(s).
     * 
     * param - list of files to print to stdout
     * return - 0 for successful execution; non-zero otherwise
     **)
    [<EntryPoint>]
    let main argv = 
        if argv.Length < 1 then 
            printfn "%s" usage
            1
        else
            Array.iter printFile argv
            0
